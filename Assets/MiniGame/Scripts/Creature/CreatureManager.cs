using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CreatureManager : MonoBehaviour
{
    [SerializeField]
    CreatureItem[] creatureItems;

    [SerializeField]
    TMP_Text unlockedCountTxt;

    int unlockedCount;

    private void Start()
    {
        
        
    }

    private void OnEnable()
    {
        RefreshStats();
    }

    public void RefreshStats()
    {
        foreach (var creature in creatureItems)
        {
            if (creature.isUnlocked == 1)
            {
                unlockedCount++;
            }
        }
        unlockedCountTxt.text = unlockedCount + "/" + creatureItems.Length;
    }
}
