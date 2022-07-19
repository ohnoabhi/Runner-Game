using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelViewer
{
    private Transform parent;
    private List<LevelObject> LevelObjects;
    private LevelEditorUpdated levelEditor;

    public LevelViewer(LevelEditorUpdated levelEditor)
    {
        this.levelEditor = levelEditor;
        parent = new GameObject("Level").transform;
        LevelObjects = new List<LevelObject>();
    }

    public void ClearLevel()
    {
        if (parent)
            Object.DestroyImmediate(parent.gameObject);
        LevelObjects.Clear();
    }

    public void CreateScene()
    {
        ClearLevel();

        foreach (var levelItem in levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel].LevelItems)
        {
            AddObject(levelItem, false);
        }
    }

    public void AddObject(LevelItemData levelItemData, bool checkPlatform = true)
    {
        if (!parent)
            parent = new GameObject("Level").transform;

        var itemPrefab = levelItemData.Item;
        if (!itemPrefab) return;
        var levelObject = GameObject.Instantiate(itemPrefab, levelItemData.Position, Quaternion.identity, parent);
        if (checkPlatform && levelObject.IsPlatform)
        {
            var hasPlatform = false;
            for (var i = LevelObjects.Count - 1; i >= 0; i--)
            {
                if (!LevelObjects[i].IsPlatform) continue;
                hasPlatform = true;
                levelItemData.Position = new Vector3(0, 0, LevelObjects[i].End.transform.position.z);
                break;
            }

            if (!hasPlatform)
            {
                levelItemData.Position = new Vector3(0, 0, 0);
            }

            var levelData =
                levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel];
            levelData.LevelItems[levelData.LevelItems.Count - 1] = levelItemData;
#if UNITY_EDITOR
            {
                EditorUtility.SetDirty(levelData);
            }
#endif
        }

        levelObject.SetData(levelItemData);


        LevelObjects.Add(levelObject);
    }

    public void ReorganizePlatforms()
    {
        var position = Vector3.zero;
        for (var i = 0; i < LevelObjects.Count; i++)
        {
            var levelObject = LevelObjects[i];
            if (!levelObject.IsPlatform) continue;

            var levelItemData = levelObject.GetData();
            levelItemData.Position = position;

            levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel].LevelItems[i] = levelItemData;

            levelObject.SetData(levelItemData);
#if UNITY_EDITOR
            {
                EditorUtility.SetDirty(levelEditor.LevelDatabase);
            }
#endif

            position = new Vector3(0, 0, levelObject.End.position.z);
        }
    }

    public void Remove(int i)
    {
        if (LevelObjects.Count <= i) return;
        var isPlatform = LevelObjects[i].IsPlatform;
        Object.DestroyImmediate(LevelObjects[i].gameObject);
        LevelObjects.RemoveAt(i);

        if (isPlatform) ReorganizePlatforms();
    }

    public void OnDestroy()
    {
    }

    public void SetPosition(int i, LevelObject levelObject, Vector3 position)
    {
        var levelItemData = levelObject.GetData();
        levelItemData.Position = position;

        levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel].LevelItems[i] = levelItemData;

        levelObject.SetData(levelItemData);
#if UNITY_EDITOR
        {
            EditorUtility.SetDirty(levelEditor.LevelDatabase);
        }
#endif
    }

    public List<LevelObject> GetLevelObjects()
    {
        return LevelObjects;
    }

    public void Reset(int i)
    {
        LevelObjects[i].SetData(levelEditor.LevelDatabase.Levels[levelEditor.Selection.SelectedLevel].LevelItems[i]);
    }
}