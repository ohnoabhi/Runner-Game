using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseScreen
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider levelProgress;
    [SerializeField] private Slider bossLevelProgress;
    [SerializeField] private Slider chestLevelProgress;

    private void Start()
    {
        levelText.text = GameManager.Level.ToString();

        var count = 10;
        levelProgress.value = GameManager.Level % count;
        var start = Mathf.FloorToInt((GameManager.Level / (float) count) * count);

        var hasBoss = false;
        var hasChest = false;
        var levelDatas = LevelDatabase.Get().Levels;
        for (var i = start; i < Mathf.Min(start + (count - 1), levelDatas.Count); i++)
        {
            if (!hasBoss && levelDatas[i].EndType == LevelEndType.BossMatch)
            {
                hasBoss = true;
                bossLevelProgress.value = i + 1 - start;
            }

            if (!hasChest && levelDatas[i].EndType == LevelEndType.Chest)
            {
                hasChest = true;
                chestLevelProgress.value = i + 1 - start;
            }
        }

        bossLevelProgress.gameObject.SetActive(hasBoss);
        chestLevelProgress.gameObject.SetActive(hasChest);
    }

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
    }
}