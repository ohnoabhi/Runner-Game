using System;
using UnityEditor;
using UnityEngine;

public class LevelEditorSceneView
{
    private readonly LevelEditorUpdated levelEditor;

    private float buttonSize = 0.25f;
    private float xPosition = 2f;

    public void OnScene(SceneView obj)
    {
        if (!levelEditor.Selection.HasSelection)
        {
            Handles.BeginGUI();

            var rect = new Rect(10, 10, 130, 50);

            var guiStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.red
                },
                padding =
                {
                    top = 20
                }
            };
            GUI.Label(rect, "Select a level to edit", guiStyle);
            Handles.EndGUI();
        }
        else
        {
            Handles.BeginGUI();
            DisplayLevelObjects();
            Handles.EndGUI();
        }

        try
        {
            DrawButtons();
        }
        catch (Exception e)
        {
            if (!(e is ArgumentOutOfRangeException))
            {
                Debug.LogWarning(e);
            }
        }

        DrawPlayer();
    }

    private void DrawPlayer()
    {
        Handles.color = Color.green;
        Handles.DrawWireCube(new Vector3(0, 1f, 6), new Vector3(1, 2, 1));
    }

    private void DrawButtons()
    {
        var i = 0;
        foreach (var levelObject in levelEditor.levelViewer.GetLevelObjects())
        {
            if (!levelObject.IsPlatform)
            {
                levelEditor.levelViewer.SetPosition(i, levelObject,
                    ZPositionHandle.Move(GUIUtility.GetControlID(levelObject.GetHashCode(), FocusType.Passive),
                        levelObject.transform.position));
            }

            var buttonPosition = levelObject.transform.position;
            Handles.color = Color.yellow;
            switch (levelObject.Layout)
            {
                case LevelObject.HorizontalLayout.Single:
                    break;
                case LevelObject.HorizontalLayout.Double:
                    if (levelObject.transform.position.x < 0)
                    {
                        buttonPosition.x = xPosition;
                        if (Handles.Button(buttonPosition, Quaternion.identity, buttonSize, buttonSize,
                            Handles.SphereHandleCap))
                        {
                            levelEditor.levelViewer.SetPosition(i, levelObject, buttonPosition);
                        }
                    }

                    if (levelObject.transform.position.x > 0)
                    {
                        buttonPosition.x = -xPosition;
                        if (Handles.Button(buttonPosition, Quaternion.identity, buttonSize, buttonSize,
                            Handles.SphereHandleCap))
                        {
                            levelEditor.levelViewer.SetPosition(i, levelObject, buttonPosition);
                        }
                    }

                    break;
                case LevelObject.HorizontalLayout.Triple:
                    if (levelObject.transform.position.x <= 0)
                    {
                        buttonPosition.x = xPosition;
                        if (Handles.Button(buttonPosition, Quaternion.identity, buttonSize, buttonSize,
                            Handles.SphereHandleCap))
                        {
                            levelEditor.levelViewer.SetPosition(i, levelObject, buttonPosition);
                        }
                    }

                    if (levelObject.transform.position.x != 0)
                    {
                        buttonPosition.x = 0;
                        if (Handles.Button(buttonPosition, Quaternion.identity, buttonSize, buttonSize,
                            Handles.SphereHandleCap))
                        {
                            levelEditor.levelViewer.SetPosition(i, levelObject, buttonPosition);
                        }
                    }

                    if (levelObject.transform.position.x >= 0)
                    {
                        buttonPosition.x = -xPosition;
                        if (Handles.Button(buttonPosition, Quaternion.identity, buttonSize, buttonSize,
                            Handles.SphereHandleCap))
                        {
                            levelEditor.levelViewer.SetPosition(i, levelObject, buttonPosition);
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!levelObject.IsPlatform)
            {
                Handles.color = Color.red;
                if (Handles.Button(levelObject.transform.position, Quaternion.identity,
                    buttonSize, buttonSize,
                    Handles.SphereHandleCap))
                {
                    levelEditor.RemoveLevelObject(i);
                }
            }

            if (levelObject.ShowDamage)
            {
                Handles.color = Color.red;
                var levelData = levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];
                var item = levelData.LevelItems[i];
                Handles.Label(levelObject.transform.position + new Vector3(0, 1.5f, 0), item.Damage + "");
                Handles.color = Color.grey;
                if (Handles.Button(levelObject.transform.position + new Vector3(0, 1, 0), Quaternion.identity,
                    buttonSize, buttonSize,
                    Handles.SphereHandleCap))
                {
                    var tempIndex = i;
                    LevelObjectEditorWindow.Show(item.Damage,
                        (value) =>
                        {
                            item.Damage = value;
                            levelData.LevelItems[tempIndex] = item;
                            levelEditor.ResetItem(tempIndex);
                            EditorUtility.SetDirty(levelData);
                            EditorUtility.SetDirty(levelEditor.LevelDatabase);
                        });
                }
            }

            if (levelObject.IsMultiplier)
            {
                var levelData = levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];
                var item = levelData.LevelItems[i];
                Handles.color = Color.grey;
                if (Handles.Button(levelObject.transform.position + new Vector3(-2, 1, 0), Quaternion.identity,
                    buttonSize, buttonSize,
                    Handles.SphereHandleCap))
                {
                    var tempIndex = i;
                    LevelObjectEditorWindow.Show(item.MultiplierValue.LeftValue,
                        (value) =>
                        {
                            item.MultiplierValue.LeftValue = value;
                            levelData.LevelItems[tempIndex] = item;
                            levelEditor.ResetItem(tempIndex);
                            EditorUtility.SetDirty(levelData);
                            EditorUtility.SetDirty(levelEditor.LevelDatabase);
                        });
                }

                if (Handles.Button(levelObject.transform.position + new Vector3(2, 1, 0), Quaternion.identity,
                    buttonSize, buttonSize,
                    Handles.SphereHandleCap))
                {
                    var tempIndex = i;
                    LevelObjectEditorWindow.Show(item.MultiplierValue.RightValue,
                        (value) =>
                        {
                            item.MultiplierValue.RightValue = value;
                            levelData.LevelItems[tempIndex] = item;
                            levelEditor.ResetItem(tempIndex);
                            EditorUtility.SetDirty(levelData);
                            EditorUtility.SetDirty(levelEditor.LevelDatabase);
                        });
                }
            }

            // if (levelObject.IsCharacter)
            // {
            //     var levelData = levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];
            //     var item = levelData.LevelItems[i];
            //     Handles.color = Color.grey;
            //     if (Handles.Button(levelObject.transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity,
            //         buttonSize, buttonSize,
            //         Handles.SphereHandleCap))
            //     {
            //         var tempIndex = i;
            //         LevelCharacterObstacleEditorWindow.Show(item.MultiplierValue.LeftValue,
            //             (value) =>
            //             {
            //                 item.CharacterId = value;
            //                 levelData.LevelItems[tempIndex] = item;
            //                 levelEditor.ResetItem(tempIndex);
            //                 EditorUtility.SetDirty(levelData);
            //                 EditorUtility.SetDirty(levelEditor.LevelDatabase);
            //             });
            //     }
            // }

            i++;
        }
    }

    private void DisplayLevelObjects()
    {
        GUILayout.BeginVertical(GUILayout.Height(500), GUILayout.Width(150));

        if (levelEditor.Selection.SelectedCategory > -1)
        {
            if (GUILayout.Button("Back"))
            {
                levelEditor.Selection.SelectedCategory = -1;
            }

            var itemID = 0;
            itemListScroll = EditorGUILayout.BeginScrollView(itemListScroll);
            EditorGUILayout.BeginVertical();

            foreach (var levelObject in levelEditor.LevelObjectDatabase.LevelObjects)
            {
                if (levelObject.ObjectType != (LevelObject.LevelObjectType) levelEditor.Selection.SelectedCategory)
                {
                    itemID++;
                    continue;
                }

                if (GUILayout.Button(AssetPreview.GetAssetPreview(levelObject.gameObject), GUILayout.Width(120),
                    GUILayout.Height(120)))
                {
                    AddItem(itemID);
                }

                itemID++;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
        else
        {
            var i = 0;
            foreach (var levelObjectType in Enum.GetValues(typeof(LevelObject.LevelObjectType)))
            {
                if (GUILayout.Button(levelObjectType.ToString()))
                {
                    levelEditor.Selection.SelectedCategory = i;
                }

                GUILayout.Space(5);
                i++;
            }
        }

        GUILayout.EndVertical();
    }

    private void AddItem(int index = 0)
    {
        var levelData = levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];

        var itemPrefab = levelEditor.LevelObjectDatabase.Get(index);
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
            levelItemData.Position = new Vector3(x, 0, GetZPosition());
        }

        levelData.LevelItems.Add(levelItemData);
        levelEditor.levelViewer.AddObject(levelItemData);
        EditorUtility.SetDirty(levelData);

        levelEditor.SelectedLevelSerializedObject.ApplyModifiedProperties();
    }

    private Vector2 itemListScroll;

    public LevelEditorSceneView(LevelEditorUpdated levelEditor)
    {
        this.levelEditor = levelEditor;
    }

    private static float GetZPosition()
    {
        var ray = HandleUtility.GUIPointToWorldRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
        var center = ray.GetPoint((0 - ray.origin.y) / ray.direction.y);
        return center.z;
    }
}