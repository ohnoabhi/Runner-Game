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
        MapManager.OnCreatureUnlockUI += RefreshStats;
    }
    private void OnDisable()
    {
        MapManager.OnCreatureUnlockUI -= RefreshStats;
    }
    private void RefreshStats(int unlockedCount,int creaturesCount)
    {
        //int unlockedCount = MapManager.instance.getMapUnlockedCount();
        unlockedCountTxt.text = unlockedCount + "/" + creaturesCount;
    }
}
