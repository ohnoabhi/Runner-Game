using UnityEditor;
using UnityEngine;

public class LevelEditorLevelsList
{
    private Vector2 levelScrollPosition;
    private LevelEditorUpdated levelEditor;

    public LevelEditorLevelsList(LevelEditorUpdated levelEditor)
    {
        this.levelEditor = levelEditor;
    }

    public void DisplayLevelsTab()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);

        var index = 1;
        levelScrollPosition = EditorGUILayout.BeginScrollView(levelScrollPosition);
        foreach (var level in levelEditor.LevelDatabase.Levels)
        {
            DrawLevel(index, level);
            GUILayout.Space(5);
            index++;
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
        var style = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 120
        };
        if (GUILayout.Button("Add new", style))
        {
            AddLevel();
        }

        EditorGUILayout.EndVertical();
    }

    private void AddLevel()
    {
        var maxNumber = 0;
        string name;

        for (var i = 0; i < levelEditor.LevelsSerializedProperty.arraySize; i++)
        {
            if (levelEditor.LevelsSerializedProperty.GetArrayElementAtIndex(i).objectReferenceValue == null) continue;
            name = levelEditor.LevelsSerializedProperty.GetArrayElementAtIndex(i)
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


        var level = ScriptableObject.CreateInstance<LevelData>();
        level.name = name;

        AssetDatabase.CreateAsset(level, LevelEditorUpdated.LevelsFolderPath + name + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        levelEditor.LevelsSerializedProperty.arraySize++;
        levelEditor.LevelsSerializedProperty.GetArrayElementAtIndex(levelEditor.LevelsSerializedProperty.arraySize - 1)
            .objectReferenceValue = level;
    }


    private void DrawLevel(int index, LevelData level)
    {
        GUILayout.BeginHorizontal(GUI.skin.box);
        GUILayout.Label(index + ". " + level.name);

        var endType = EditorGUILayout.EnumPopup(level.EndType, GUILayout.Width(150));
        if ((LevelEndType) endType != level.EndType)
        {
            level.EndType = (LevelEndType) endType;
            EditorUtility.SetDirty(level);
            EditorUtility.SetDirty(levelEditor.LevelDatabase);
            return;
        }

        var style = new GUIStyle(GUI.skin.button);
        style.fixedWidth = 80;
        if (GUILayout.Button("Edit", style))
        {
            levelEditor.Selection.SelectedLevel = index - 1;
            if (levelEditor.LevelsSerializedProperty
                .GetArrayElementAtIndex(levelEditor.Selection.SelectedLevel)
                .objectReferenceValue != null)
            {
                levelEditor.SelectedLevelSerializedObject = new SerializedObject(
                    (LevelData) levelEditor.LevelsSerializedProperty
                        .GetArrayElementAtIndex(levelEditor.Selection.SelectedLevel).objectReferenceValue);
                levelEditor.levelViewer.CreateScene();
            }

            SceneView.RepaintAll();
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
            var level = (LevelData) levelEditor.LevelsSerializedProperty.GetArrayElementAtIndex(index)
                .objectReferenceValue;
            RemoveFromObjectArrayAt(levelEditor.LevelsSerializedProperty, index);

            if (level != null)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(level));
                AssetDatabase.Refresh();
            }

            levelEditor.RenameLevels();
        }
    }

    private static void RemoveFromObjectArrayAt(SerializedProperty arrayProperty, int index)
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
}