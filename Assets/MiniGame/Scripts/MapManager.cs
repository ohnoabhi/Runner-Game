using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using maps;
using System.Threading.Tasks;

public class MapManager : MonoBehaviour
{
    public Map[] maps;
    public static MapManager instance;
    public static Action<int, int> OnCreatureUnlock;
    Map currentMapGameObj;
    public int currentMap
    {
        get => PlayerPrefs.GetInt("currentMap", 0);
        set => PlayerPrefs.SetInt("currentMap", value);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        LoadMap();
    }

    private void LoadMap()
    {
        if(maps.Length <= currentMap)
        {
            Debug.LogWarning("All maps completed");
            return;
        }
        currentMapGameObj = Instantiate(maps[currentMap]);       
    }

    public void OnMapComplete()
    {
        Destroy(currentMapGameObj.gameObject);
        currentMap++;
        LoadMap();
    }

    private void OnDisable()
    {
        if(currentMapGameObj)
            Destroy(currentMapGameObj.gameObject);
    }

    public Map GetCurrentMap()
    {
        return maps[currentMap];
    }

    public async void OnUnlock()
    {
        var creatures = GetCurrentMap().Creatures;
        var unlocked = 0;
        foreach(var creature in creatures)
        {
            if (creature.IsUnlocked)
            {
                unlocked++;
            }
        }
        OnCreatureUnlock?.Invoke(creatures.Length, unlocked);

        if(unlocked == creatures.Length)
        {
            await Task.Delay(3000);
            OnMapComplete();
        }
    }
}

//[System.Serializable]
/*public class Map 
{
    


    
}*/
