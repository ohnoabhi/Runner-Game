using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    Coin,
    Gem,
    Health
}
public class CollectablesManager :MonoBehaviour
{
    public Collectable[] collectables;
    public static CollectablesManager instance;

    private void Awake()
    {
        instance = this;
    }
    public static void Add(CollectableType type,int amount)
    {
        if (type != CollectableType.Health)
        {
            foreach (var collectable in instance.collectables)
            {
                if (collectable.Type == type)
                {
                    collectable.Amount += amount;
                    Debug.Log("Amount: " + collectable.Amount);
                    break;
                }
            }
        }

        else if(type == CollectableType.Health)
        {
            PlayerLevelManager.instance.Add(amount);
            PlayerLevelUI.RefreshStats.Invoke();
        }
    }

    public static void Remove(CollectableType type, int amount)
    {
        foreach (var collectable in instance.collectables)
        {
            if (collectable.Type == type)
            {
                collectable.Amount -= amount;
                Debug.Log("Amount: " + collectable.Amount);
                break;
            }
        }
    }

}

[System.Serializable]
public class Collectable
{
    public CollectableType Type;
    public int Amount
    {
        get => PlayerPrefs.GetInt(Type + "Amount", 0);
        set => PlayerPrefs.SetInt(Type + "Amount",value<0?0:value);

    }

    
}
