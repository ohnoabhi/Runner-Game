using UnityEditor;
using UnityEngine;

public class LevelEditorLevelObjectsList
{
    private readonly LevelEditorUpdated levelEditor;
    private Vector2 itemsScrollPosition;

    public LevelEditorLevelObjectsList(LevelEditorUpdated levelEditor)
    {
        this.levelEditor = levelEditor;
    }

    public void DisplayLevelDetails()
    {
        var levelData = levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];
        var style = new GUIStyle(GUI.skin.button)
        {
            fixedWidth = 120
        };
        if (GUILayout.Button("Back", style))
        {
            levelEditor.Selection.SelectedLevel = -1;
            levelEditor.levelViewer.ClearLevel();
            SceneView.RepaintAll();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("Level " + (levelEditor.Selection.SelectedLevel + 1));
        GUILayout.Space(10);

        itemsScrollPosition = EditorGUILayout.BeginScrollView(itemsScrollPosition);

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
                if (!levelItems[i].Item.IsPlatform) continue;

                GUILayout.BeginHorizontal(GUI.skin.box);
                // var levelObject = _levelObjectDatabase.Get(levelItems[i].ReferenceID);
                var levelObject = levelItems[i].Item;
                GUILayout.Label((i + 1) + ". " + levelObject.name);

                if (levelObject.ShowDamage)
                {
                    var damage = EditorGUILayout.IntField("Damage", levelItems[i].Damage);
                    if (damage != levelItems[i].Damage)
                    {
                        var item = levelData.LevelItems[i];
                        item.Damage = damage;
                        levelData.LevelItems[i] = item;
                        EditorUtility.SetDirty(levelData);
                        EditorUtility.SetDirty(levelEditor.LevelDatabase);
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
                    var index = EditorGUILayout.Popup(levelItems[i].CharacterId, levelEditor.Characters);
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
                    levelEditor.RemoveLevelObject(i);
                }

                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // DisplayLevelObjects();

        GUILayout.Space(10);
        EditorGUILayout.EndVertical();
    }
}