using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    Coin,
    Gem,
    Health
}

public class CollectablesManager : MonoBehaviour
{
    public Collectable[] collectables;
    public static CollectablesManager instance;
    private static Action<Collectable> OnUpdate;

    private void Awake()
    {
        instance = this;
    }

    public static void Add(CollectableType type, int amount)
    {
        if (type != CollectableType.Health)
        {
            foreach (var collectable in instance.collectables)
            {
                if (collectable.Type == type)
                {
                    collectable.Amount += amount;
                    Debug.Log("Amount: " + collectable.Amount);
                    OnUpdate?.Invoke(collectable);
                    break;
                }
            }
        }
    }

    public static void Remove(CollectableType type, int amount)
    {
        foreach (var collectable in instance.collectables)
        {
            if (collectable.Type == type)
            {
                collectable.Amount -= amount;
                OnUpdate?.Invoke(collectable);
                break;
            }
        }
    }

    public static Collectable GetCollectable(CollectableType type)
    {
        foreach (var collectable in instance.collectables)
        {
            if (collectable.Type == type)
            {
                return collectable;
            }
        }
        return null;
    }

    public void RegisterForUpdate(Action<Collectable> action)
    {
        OnUpdate += action;
    }

    public void DeRegisterForUpdate(Action<Collectable> action)
    {
        OnUpdate -= action;
    }
}

[System.Serializable]
public class Collectable
{
    public CollectableType Type;

    public Sprite collectableImg;

    public int Amount
    {
        get => PlayerPrefs.GetInt(Type + "Amount", 0);
        set => PlayerPrefs.SetInt(Type + "Amount", value < 0 ? 0 : value);
    }
}