using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUI : MonoBehaviour
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

    /*private void Update()
    {
        if(Input.GetKeyDown(plus))
        {
            PlayerLevelManager.instance.Add(10);
            RefreshValues();
        }

        if(Input.GetKeyDown(minus))
        {
            PlayerLevelManager.instance.Remove(10);
            RefreshValues();
        }
    }*/

    private void RefreshValues()
    {
        levelNo.text = PlayerLevelManager.instance.GetLevel().ToString();

        healthMeter.value = PlayerLevelManager.instance.GetHealth();

        healthMeter.maxValue = PlayerLevelManager.instance.GetMaxHealth();
    }
}
