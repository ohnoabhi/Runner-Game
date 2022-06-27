using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private LevelObjectDatabase levelObjectDatabase;
    private Dictionary<string, List<LevelObject>> availableLevelObjects;
    private Dictionary<string, List<LevelObject>> usingLevelObjects;
    private Transform AvailableItems;

    private void Awake()
    {
        Instance = this;
        levelObjectDatabase = LevelObjectDatabase.Get();

        AvailableItems = new GameObject("AvailableItems").transform;

        availableLevelObjects = new Dictionary<string, List<LevelObject>>();
        usingLevelObjects = new Dictionary<string, List<LevelObject>>();
    }

    public async void ClearLevelObjects()
    {
        foreach (var key in usingLevelObjects.Keys)
        {
            // if (key == "BrickWall Big" || key == "BrickWall" || key == "GlassObstacle" || key == "FloorGlass")
            // {
            //     foreach (var levelObject in usingLevelObjects[key].Where(levelObject => levelObject))
            //     {
            //         Destroy(levelObject.gameObject);
            //         await Task.Yield();
            //     }
            // }
            // else
            // {
            foreach (var levelObject in usingLevelObjects[key].Where(levelObject => levelObject))
            {
                if (levelObject.ShouldClear)
                {
                    Destroy(levelObject.gameObject);
                }
                else
                {
                    levelObject.gameObject.SetActive(false);
                    availableLevelObjects[key].Add(levelObject);
                }
            }
            // }

            usingLevelObjects[key].Clear();
        }
    }


    public LevelObject GetLevelObject(LevelObject levelObject)
    {
        var key = levelObject.name;
        if (availableLevelObjects[key].Count > 0)
        {
            var item = availableLevelObjects[key][availableLevelObjects[key].Count - 1];
            item.gameObject.SetActive(true);
            item.ResetObject();
            availableLevelObjects[key].Remove(item);
            usingLevelObjects[key].Add(item);
            return item;
        }
        else
        {
            var item = Instantiate(levelObject, AvailableItems);
            item.gameObject.SetActive(true);
            usingLevelObjects[key].Add(item);
            return item;
        }
    }

    public static async Task LoadItems()
    {
        foreach (var levelObject in Instance.levelObjectDatabase.LevelObjects)
        {
            var key = levelObject.name;
            Instance.availableLevelObjects[key] = new List<LevelObject>();
            Instance.usingLevelObjects[key] = new List<LevelObject>();
            for (var i = 0; i < 10; i++)
            {
                var item = Instantiate(levelObject, Instance.AvailableItems);
                item.gameObject.SetActive(false);
                Instance.availableLevelObjects[key].Add(item);
            }

            await Task.Yield();
        }
    }
}