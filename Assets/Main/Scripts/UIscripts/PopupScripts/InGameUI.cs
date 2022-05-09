using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    Text levelNo;

    [SerializeField]
    Slider healthMeter;

    [SerializeField]
    KeyCode plus;

    [SerializeField]
    KeyCode minus;

    public static Action RefreshStats;
    private void Start()
    {
        RefreshValues();
    }
    private void OnEnable()
    {
        RefreshStats += RefreshValues;
    }

    private void OnDisable()
    {
        RefreshStats -= RefreshValues;
    }

    private void RefreshValues()
    {
        levelNo.text = Player.Instance.Health.GetHealthLevel().ToString();

        healthMeter.value = Player.Instance.CurrentHealth;

        healthMeter.maxValue = Player.Instance.Health.GetMaxLevelHealth();
    }
}
