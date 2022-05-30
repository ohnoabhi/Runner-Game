using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using maps;
using System.Threading.Tasks;

public class MapManager : MonoBehaviour
{
    public Map[] maps;
    public static MapManager instance;
    public static Action<int, int> OnCreatureUnlockUI;
    public static Action<int> OnMapLoaded;

    public Map CurrentMap { get; private set; }

    private int currentMapIndex
    {
        get => PlayerPrefs.GetInt("currentMap", 0);
        set => PlayerPrefs.SetInt("currentMap", value);
    }

    private void Awake()
    {
        instance = this;
        var mapID = 0;
        foreach (var map in maps)
        {
            var creatureID = 0;
            foreach (var creature in map.Creatures)
            {
                creature.CreatureKey = "Creature_" + mapID + creatureID;
                creatureID++;
            }

            mapID++;
        }
    }

    private void Start()
    {
        LoadMap();
        OnCreatureUnlockUI?.Invoke(getMapUnlockedCount(), CurrentMap.Creatures.Length);
    }

    private void LoadMap()
    {
        if (maps.Length <= currentMapIndex)
        {
            currentMapIndex = 0;
        }
        else if (maps[currentMapIndex].completed)
        {
            currentMapIndex++;
            if (maps.Length <= currentMapIndex)
            {
                currentMapIndex = 0;
            }
        }

        CurrentMap = Instantiate(maps[currentMapIndex]);
        OnMapLoaded?.Invoke(CurrentMap.GetCameraStartPosition());
    }

    private void OnMapComplete()
    {
        if (CurrentMap)
            Destroy(CurrentMap.gameObject);
        currentMapIndex++;
        LoadMap();
        OnCreatureUnlockUI?.Invoke(getMapUnlockedCount(), CurrentMap.Creatures.Length);
    }

    private void OnDisable()
    {
        if (CurrentMap)
            Destroy(CurrentMap.gameObject);
    }

    public async void OnUnlock()
    {
        int unlocked = getMapUnlockedCount();
        var creatures = CurrentMap.Creatures;
        OnCreatureUnlockUI?.Invoke(unlocked, creatures.Length);
        if (unlocked == creatures.Length)
        {
            await Task.Delay(3000);
            OnMapComplete();
        }
    }

    public int getMapUnlockedCount()
    {
        var creatures = CurrentMap.Creatures;
        var unlocked = creatures.Count(creature => creature.IsUnlocked);
        return unlocked;
    }
}

//[System.Serializable]
/*public class Map 
{
    


    
}*/