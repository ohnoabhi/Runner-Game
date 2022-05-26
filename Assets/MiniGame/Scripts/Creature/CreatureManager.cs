using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CreatureManager : MonoBehaviour
{
    
    public CreatureItem[] creatureItems;

    [SerializeField]
    TMP_Text unlockedCountTxt;

    [HideInInspector]
    public int mapId;

    int unlockedCount;

    private void Start()
    {
        mapId = MapManager.instance.maps[MapManager.instance.currentMap].mapId;

        //MoveCamera.instance.MapSwitch(this.GetComponent<CreatureManager>());
        
    }

    private void OnEnable()
    {
        RefreshStats();
    }

    public void RefreshStats()
    {
        unlockedCount = 0;
        foreach (var creature in creatureItems)
        {
            if (creature.isUnlocked == 1)
            {
                unlockedCount++;
            }
        }
        unlockedCountTxt.text = unlockedCount + "/" + creatureItems.Length;

        if (unlockedCount == creatureItems.Length)
        {
            StartCoroutine(Waitor());        
        }
    }

    IEnumerator Waitor() //wait time before map switch
    {
        yield return new WaitForSeconds(3f);
        MapManager.instance.OnMapComplete();
    }
}
