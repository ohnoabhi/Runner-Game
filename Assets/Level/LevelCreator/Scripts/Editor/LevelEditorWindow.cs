using System;
using System.Collections.Generic;
using System.Linq;
using CharacterBase;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorWindow : EditorWindow
{
    private const string LevelsFolderPath = "Assets/Level/Levels/";
    private const string LevelScenePath = "Assets/Level/LevelCreator/LevelCreator.unity";
    private static LevelEditorWindow _window;

    public SerializedObject LevelDatabaseSerializedObject;
    public SerializedProperty LevelsSerializedProperty;
    public SerializedObject SelectedLevelSerializedObject;


    private LevelEditorManager _levelEditorManager;

    private string[] characters;

    [MenuItem("Tools/Edit Level Items")]
    public static void EditLevelItems()
    {
        var levelObjectDatabase = LevelObjectDatabase.Get();
        if (!levelObjectDatabase) return;
        // EditorGUIUtility.PingObject(levelObjectDatabase);
        Selection.activeObject = levelObjectDatabase;
    }

    [MenuItem("Tools/Level Editor")]
    public static void Open()
    {
        if (!SceneManager.GetSceneByName("LevelCreator").isLoaded)
        {
            if (EditorUtility.DisplayDialog("Level Scene Not Loaded!",
                "Level editor requires level scene to be loaded!", "Load Scene", "cancel"))
            {
                EditorSceneManager.OpenScene(LevelScenePath);
            }
        }

        if (!_window)
            _window = GetWindow<LevelEditorWindow>("Level Editor");
        _window.Show();
    }

    private void OnEnable()
    {
        _levelEditorManager = FindObjectOfType<LevelEditorManager>();
        if (!_levelEditorManager)
        {
            _levelEditorManager = new GameObject("Level Manager").AddComponent<LevelEditorManager>();
        }

        Selection.selectionChanged += OnSelectionChange;
        Selection.activeObject = _levelEditorManager;
        _levelEditorManager.Init();

        _levelObjectDatabase = LevelObjectDatabase.Get();
        if (_levelEditorManager.LevelDatabase == null)
        {
            Debug.LogError("LevelDatabase is missing!");
            return;
        }

        LevelDatabaseSerializedObject = new SerializedObject(_levelEditorManager.LevelDatabase);
        LevelsSerializedProperty = LevelDatabaseSerializedObject.FindProperty("Levels");

        itemListScroll = new Vector2[Enum.GetValues(typeof(LevelObject.LevelObjectType)).Length];

        characters = CharacterDatabase.Get().Items.Select(shopItem => shopItem.Key).ToArray();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChange;

        _levelEditorManager.ClearLevel();
        DestroyImmediate(_levelEditorManager.gameObject);
    }

    private void OnSelectionChange()
    {
        Selection.activeObject = _levelEditorManager;
    }

    private void OnGUI()
    {
        if (_levelEditorManager.Selection.HasSelection)
            DisplayLevelDetails();
        else
        {
            _levelEditorManager.ClearLevel();
            DisplayLevelsTab();
        }

        LevelDatabaseSerializedObject?.ApplyModifiedProperties();
        SelectedLevelSerializedObject?.ApplyModifiedProperties();
    }

    private Vector2 _levelScrollPosition;

    private void DisplayLevelsTab()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        var index = 1;
        _levelScrollPosition = EditorGUILayout.BeginScrollView(_levelScrollPosition);
        foreach (var level in _levelEditorManager.LevelDatabase.Levels)
        {
            DrawLevel(index, level);
            GUILayout.Space(5);
            index++;
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
        var style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = 120;
        if (GUILayout.Button("Add new", style))
        {
            AddLevel();
        }

        EditorGUILayout.EndVertical();
    }

    private void AddLevel()
    {
        int maxNumber = 0;
        string name;

        for (var i = 0; i < LevelsSerializedProperty.arraySize; i++)
        {
            if (LevelsSerializedProperty.GetArrayElementAtIndex(i).objectReferenceValue == null) continue;
            name = LevelsSerializedProperty.GetArrayElementAtIndex(i)
                .objectReferenceValue
                .name;
            var levelNumber = int.Parse(name.Substring(6));

            if (levelNumber > maxNumber)
            {
                maxNumber = levelNumber;
            }
        }

        maxNumber++;
        name = "Level " + maxNumber;


        var level = CreateInstance<LevelData>();
        level.name = name;

        AssetDatabase.CreateAsset(level, LevelsFolderPath + name + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        LevelsSerializedProperty.arraySize++;
        LevelsSerializedProperty.GetArrayElementAtIndex(LevelsSerializedProperty.arraySize - 1)
            .objectReferenceValue = level;
    }


    private void DrawLevel(int index, LevelData level)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("#" + index + " [" + level.name + "]");

        var temp = EditorGUILayout.EnumPopup(level.EndType);
        if (level.EndType != (LevelEndType) temp)
        {
            level.EndType = (LevelEndType) temp;
            EditorUtility.SetDirty(level);
        }

        var style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = 80;
        if (GUILayout.Button("Edit", style))
        {
            _levelEditorManager.Selection.SelectedLevel = index - 1;
            if (LevelsSerializedProperty
                .GetArrayElementAtIndex(_levelEditorManager.Selection.SelectedLevel)
                .objectReferenceValue != null)
            {
                SelectedLevelSerializedObject = new SerializedObject(
                    (LevelData) LevelsSerializedProperty
                        .GetArrayElementAtIndex(_levelEditorManager.Selection.SelectedLevel).objectReferenceValue);
                _levelEditorManager.CreateScene();
            }
        }

        style.normal.textColor = Color.red;
        style.hover.textColor = Color.red;
        style.active.textColor = Color.red;
        style.fixedWidth = 30;
        if (GUILayout.Button("X", style))
        {
            RemoveLevel(index - 1);
        }

        GUILayout.EndHorizontal();
    }


    private void RemoveLevel(int index)
    {
        if (EditorUtility.DisplayDialog("Warning",
            "Are you sure you want to remove level #" + (index + 1) + " ?", "ok", "cancel"))
        {
            var level = (LevelData) LevelsSerializedProperty.GetArrayElementAtIndex(index)
                .objectReferenceValue;
            RemoveFromObjectArrayAt(LevelsSerializedProperty, index);

            if (level != null)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(level));
                AssetDatabase.Refresh();
            }

            RenameLevels();
        }
    }

    private void RenameLevels()
    {
        var index = 1;
        foreach (var level in _levelEditorManager.LevelDatabase.Levels)
        {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(level), "Level " + index);
            index++;
        }

        AssetDatabase.Refresh();
    }

    public static void RemoveFromObjectArrayAt(SerializedProperty arrayProperty, int index)
    {
        // If the index is not appropriate or the serializedProperty this is being called from is not an array, throw an exception.
        if (index < 0)
            throw new UnityException("SerializedProperty " + arrayProperty.name +
                                     " cannot have negative elements removed.");

        if (!arrayProperty.isArray)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " is not an array.");

        if (index > arrayProperty.arraySize - 1)
            throw new UnityException("SerializedProperty " + arrayProperty.name + " has only " +
                                     arrayProperty.arraySize + " elements so element " + index +
                                     " cannot be removed.");

        // Pull all the information from the target of the serializedObject.
        arrayProperty.serializedObject.Update();

        // If there is a non-null element at the index, null it.
        if (arrayProperty.GetArrayElementAtIndex(index).objectReferenceValue)
            arrayProperty.DeleteArrayElementAtIndex(index);

        // Delete the null element from the array at the index.
        arrayProperty.DeleteArrayElementAtIndex(index);

        // Push all the information on the serializedObject back to the target.
        arrayProperty.serializedObject.ApplyModifiedProperties();
    }

    private Vector2 _itemsScrollPosition;
    private LevelObjectDatabase _levelObjectDatabase;

    private void DisplayLevelDetails()
    {
        var levelData = _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];
        var style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = 120;
        if (GUILayout.Button("Back", style))
        {
            _levelEditorManager.Selection.SelectedLevel = -1;
            _levelEditorManager.ClearLevel();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Level " + (_levelEditorManager.Selection.SelectedLevel + 1));
        GUILayout.Space(10);

        _itemsScrollPosition = EditorGUILayout.BeginScrollView(_itemsScrollPosition);

        var levelItems = levelData.LevelItems;

        if (levelData == null || levelItems.Count == 0)
        {
            GUILayout.Label("No items in level!", new GUIStyle(GUIStyle.none)
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.red
                }
            });
        }
        else
        {
            for (var i = 0; i < levelItems.Count; i++)
            {
                GUILayout.BeginHorizontal();
                // var levelObject = _levelObjectDatabase.Get(levelItems[i].ReferenceID);
                var levelObject = levelItems[i].Item;
                GUILayout.Label((i + 1) + ". " + levelObject.ObjectType + "[" + levelObject.name + "]");

                if (levelObject.ShowDamage)
                {
                    var damage = EditorGUILayout.IntField("Damage", levelItems[i].Damage);
                    if (damage != levelItems[i].Damage)
                    {
                        var item = levelData.LevelItems[i];
                        item.Damage = damage;
                        levelData.LevelItems[i] = item;
                        EditorUtility.SetDirty(levelData);
                    }
                }

                if (levelObject.IsMultiplier)
                {
                    var left = EditorGUILayout.IntField("Left", levelItems[i].MultiplierValue.LeftValue);
                    if (left != levelItems[i].MultiplierValue.LeftValue)
                    {
                        var item = levelData.LevelItems[i];
                        item.MultiplierValue.LeftValue = left;
                        levelData.LevelItems[i] = item;
                        EditorUtility.SetDirty(levelData);
                    }

                    var right = EditorGUILayout.IntField("Right", levelItems[i].MultiplierValue.RightValue);
                    if (right != levelItems[i].MultiplierValue.RightValue)
                    {
                        var item = levelData.LevelItems[i];
                        item.MultiplierValue.RightValue = right;
                        levelData.LevelItems[i] = item;
                        EditorUtility.SetDirty(levelData);
                    }
                }

                if (levelObject.IsCharacter)
                {
                    var index = EditorGUILayout.Popup(levelItems[i].CharacterId, characters);
                    if (index != levelItems[i].CharacterId)
                    {
                        var item = levelData.LevelItems[i];
                        item.CharacterId = index;
                        levelData.LevelItems[i] = item;
                        EditorUtility.SetDirty(levelData);
                    }
                }

                if (GUILayout.Button("X", new GUIStyle(GUI.skin.button)
                {
                    fixedWidth = 30,
                    normal =
                    {
                        textColor = Color.red
                    },
                    hover =
                    {
                        textColor = Color.red
                    },
                    active =
                    {
                        textColor = Color.red
                    }
                }))
                {
                    Remove(i);
                }

                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        DisplayLevelObjects();

        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }

    private void DisplayLevelObjects()
    {
        GUILayout.BeginVertical(GUILayout.Height(360));
        var i = 0;
        foreach (var levelObjectType in Enum.GetValues(typeof(LevelObject.LevelObjectType)))
        {
            GUILayout.Label(levelObjectType.ToString());
            GUILayout.Space(5);
            itemListScroll[i] = EditorGUILayout.BeginScrollView(itemListScroll[i]);
            EditorGUILayout.BeginHorizontal(GUI.skin.box);

            var itemID = 0;
            foreach (var levelObject in _levelObjectDatabase.LevelObjects)
            {
                if (levelObject.ObjectType != (LevelObject.LevelObjectType) levelObjectType)
                {
                    itemID++;
                    continue;
                }

                if (GUILayout.Button(AssetPreview.GetAssetPreview(levelObject.gameObject), GUILayout.Width(50),
                    GUILayout.Height(50)))
                {
                    AddItem(itemID);
                }

                itemID++;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            i++;
        }

        GUILayout.EndVertical();
    }

    private Vector2[] itemListScroll;

    private void Remove(int i)
    {
        var levelData = _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];

        _levelEditorManager.Remove(i);
        levelData.LevelItems.RemoveAt(i);
        EditorUtility.SetDirty(levelData);
        SelectedLevelSerializedObject.ApplyModifiedProperties();
    }

    private void AddItem(int index = 0)
    {
        var levelData = _levelEditorManager.LevelDatabase.Levels[_levelEditorManager.Selection.SelectedLevel];

        var itemPrefab = _levelObjectDatabase.Get(index);
        var levelItemData = new LevelItemData()
        {
            Item = itemPrefab,
            MultiplierValue = new Multiplier.MultiplierValue()
        };
        if (!itemPrefab.IsPlatform)
        {
            var x = itemPrefab.Layout == LevelObject.HorizontalLayout.Single ||
                    itemPrefab.Layout == LevelObject.HorizontalLayout.Triple
                ? 0
                : LevelObject.LevelSize;
            levelItemData.Position = new Vector3(x, 0, 0);
        }

        levelData.LevelItems.Add(levelItemData);
        _levelEditorManager.AddObject(levelItemData);
        EditorUtility.SetDirty(levelData);

        SelectedLevelSerializedObject.ApplyModifiedProperties();
    }
}