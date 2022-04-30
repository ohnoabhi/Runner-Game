using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLevelManager : MonoBehaviour
{

    public static PlayerLevelManager instance;

    [SerializeField]
    int[] levelUpRequirement;

    private int currentHealth = 0;

    private int currentLevel = 0;

    private void Awake()
    {
        instance = this;
    }

    public void Add(int amt)
    {
        currentHealth += amt;

        if(currentHealth >= levelUpRequirement[currentLevel + 1])
        {
            currentHealth -= levelUpRequirement[currentLevel + 1];
            currentLevel++;
        }
    }

    public void Minus(int amt)
    {
        currentHealth -= amt;

        if(currentHealth <= 0)
        {
            if(currentLevel != 0)
            {
                int value = currentHealth;

                currentHealth = levelUpRequirement[currentLevel - 1];
                currentHealth += value;
                currentLevel--;
            }

            else
            {
                Debug.Log("You died");
            }
        }
    }

    public int GetLevel()
    {
        return currentLevel;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return levelUpRequirement[currentLevel+1];
    }
}
