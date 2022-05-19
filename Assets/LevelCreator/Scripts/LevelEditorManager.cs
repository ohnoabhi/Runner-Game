using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class LevelEditorManager : MonoBehaviour
{
    public class SelectionInfo
    {
        public bool HasSelection => SelectedLevel >= 0;
        public int SelectedLevel = -1;
    }

    public List<LevelObject> LevelObjects = new List<LevelObject>();

    private Transform parent;
    private LevelObjectDatabase database;
    public LevelDatabase LevelDatabase { get; private set; }

    public SelectionInfo Selection { get; private set; }


    public void ClearLevel()
    {
        if (parent)
            DestroyImmediate(parent.gameObject);
        LevelObjects.Clear();
    }


    public void CreateScene()
    {
        ClearLevel();

        foreach (var levelItem in LevelDatabase.Levels[Selection.SelectedLevel].LevelItems)
        {
            AddObject(levelItem, false);
        }
    }

    public void AddObject(LevelItemData levelItemData, bool checkPlatform = true)
    {
        if (!parent)
            parent = new GameObject("Level").transform;

        var itemPrefab = database.Get(levelItemData.ReferenceID);
        if (!itemPrefab) return;
        var levelObject = Instantiate(itemPrefab, levelItemData.Position, Quaternion.identity, parent);
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
                LevelDatabase.Levels[Selection.SelectedLevel];
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

            var levelData =
                LevelDatabase.Levels[Selection.SelectedLevel];

            levelData.LevelItems[i] = levelItemData;

            levelObject.SetData(levelItemData);
#if UNITY_EDITOR
            {
                EditorUtility.SetDirty(levelData);
            }
#endif

            position = new Vector3(0, 0, levelObject.End.position.z);
        }
    }

    public void Init()
    {
        database = LevelObjectDatabase.Get();
        LevelDatabase = LevelDatabase.Get();
        Selection = new SelectionInfo();
    }

    public void Remove(int i)
    {
        if (LevelObjects.Count <= i) return;
        var isPlatform = LevelObjects[i].IsPlatform;
        DestroyImmediate(LevelObjects[i].gameObject);
        LevelObjects.RemoveAt(i);

        if (isPlatform) ReorganizePlatforms();
    }
}