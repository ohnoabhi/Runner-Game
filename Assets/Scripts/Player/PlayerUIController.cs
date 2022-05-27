using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNo;

    [SerializeField] Slider healthMeter;
    private new Transform camera;

    private void Start()
    {
        SetPlayerHealth(0, 0, 1);
        SetPlayerHealthLevel(0);
        GameManager.OnPlayerHealthChange += SetPlayerHealth;
        GameManager.OnPlayerHealthLevelChange += SetPlayerHealthLevel;
    }

    private void OnDestroy()
    {
        GameManager.OnPlayerHealthChange -= SetPlayerHealth;
        GameManager.OnPlayerHealthLevelChange -= SetPlayerHealthLevel;
    }

    public void SetCamera(Transform camera)
    {
        this.camera = camera;
    }

    private void Update()
    {
        if (camera) transform.forward = camera.forward;
    }

    private void SetPlayerHealthLevel(int level)
    {
        levelNo.text = level.ToString();
    }

    private void SetPlayerHealth(float health, int minHealth, int maxHealth)
    {
        Debug.Log("Health: " + health + "|Min: " + minHealth + "|Max: " + maxHealth);
        healthMeter.minValue = minHealth;
        healthMeter.value = health;
        healthMeter.maxValue = maxHealth;
    }
}