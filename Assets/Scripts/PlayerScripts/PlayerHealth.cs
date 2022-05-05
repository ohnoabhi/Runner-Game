using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int levelUpRequirement;

    private int health
    {
        get => Player.Instance.CurrentHealth;
        set => Player.Instance.CurrentHealth = value;
    }

    private int HealthLevel => health > 0 ? health / levelUpRequirement : 0;


    public void GainHealth(int amount)
    {
        var tempLevel = HealthLevel;
        health += amount;
        if (HealthLevel != tempLevel)
        {
            Player.Instance.OnChangeHealthLevel?.Invoke(HealthLevel);
        }

        InGameUI.RefreshStats?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        var tempLevel = HealthLevel;
        health -= amount;
        if (health < 0) health = 0;

        if (HealthLevel != tempLevel)
        {
            Player.Instance.OnChangeHealthLevel?.Invoke(HealthLevel);
        }

        if (health == 0) Player.Instance.Die();

        InGameUI.RefreshStats?.Invoke();
    }

    public int GetHealthLevel()
    {
        return HealthLevel;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxLevelHealth()
    {
        return HealthLevel * levelUpRequirement;
    }
}