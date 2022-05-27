using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameTopHUD : MonoBehaviour
{

    [SerializeField]
    TMP_Text unlockedCountTxt;
    private void OnEnable()
    {
        MapManager.OnCreatureUnlock += RefreshStats;
    }
    private void OnDisable()
    {
        MapManager.OnCreatureUnlock -= RefreshStats;
    }
    private void RefreshStats(int creaturesCount, int unlockedCount)
    {
        unlockedCountTxt.text = unlockedCount + "/" + creaturesCount;
    }
}
