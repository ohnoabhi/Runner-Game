using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int levelUpRequirement;

    public float StartHealth;

    public float CurrentHealth;
    public float MaxHealth = 100;

    private Player Player;

    private int HealthLevel => CurrentHealth > 0 ? Mathf.FloorToInt(CurrentHealth / levelUpRequirement) : 0;

    public int MaxLevelHealth => (HealthLevel + 1) * levelUpRequirement;
    public int MinLevelHealth => HealthLevel <= 0 ? 0 : (HealthLevel - 1) * levelUpRequirement;

    private void Awake()
    {
        Player = GetComponent<Player>();
    }

    public void GainHealth(float amount)
    {
        var tempLevel = HealthLevel;
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
        GameManager.OnPlayerHealthChange?.Invoke(CurrentHealth, MinLevelHealth, MaxLevelHealth);
        if (HealthLevel != tempLevel)
        {
            GameManager.OnPlayerHealthLevelChange?.Invoke(HealthLevel);
        }
    }

    public void TakeDamage(float amount)
    {
        var tempLevel = HealthLevel;
        CurrentHealth -= amount;
        if (CurrentHealth < 0) CurrentHealth = 0;
        GameManager.OnPlayerHealthChange?.Invoke(CurrentHealth, MinLevelHealth, MaxLevelHealth);

        if (HealthLevel != tempLevel)
        {
            GameManager.OnPlayerHealthLevelChange?.Invoke(HealthLevel);
        }

        if (CurrentHealth == 0) Player.Die();
    }
}