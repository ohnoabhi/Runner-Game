using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public Map[] maps;
    public static MapManager instance;

    GameObject currentMapGameObj;
    public int currentMap
    {
        get => PlayerPrefs.GetInt("currentMap", 0);
        set => PlayerPrefs.SetInt("currentMap", value);
    }

    private void Awake()
    {
        instance = this;
        LoadMap();


    }
    private void Start()
    {
        
    }

    private void LoadMap()
    {
        currentMapGameObj = Instantiate(maps[currentMap].mapPrefab);       
    }

    public void OnMapComplete()
    {
        currentMap++;
        switchMap();
    }

    private void switchMap()
    {
        Destroy(currentMapGameObj.gameObject);
        LoadMap();
        MoveCamera.instance.getCreatureManager();
    }

    private void OnDisable()
    {
        Destroy(currentMapGameObj.gameObject);
    }
}

[System.Serializable]
public class Map
{
    public int mapId;
    public GameObject mapPrefab;
    public bool completed = false;
}
