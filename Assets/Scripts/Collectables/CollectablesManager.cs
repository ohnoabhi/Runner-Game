using System;
using Collectables;
using UnityEngine;

public enum CollectableType
{
    Cash,
    Gem,
    Key
}

[Serializable]
public class Price
{
    public CollectableType Type;
    public int Amount;

    public bool IsAffordable()
    {
        return CollectablesManager.Get(Type) >= Amount;
    }

    public void Pay()
    {
        CollectablesManager.Remove(Type, Amount);
    }

    public void Gain()
    {
        CollectablesManager.Add(Type, Amount);
    }
}

public static class CollectablesManager
{
    private static Action<int> OnUpdate;

    public static int Get(CollectableType type) => PlayerPrefs.GetInt(type + "Amount", 30);

    public static void Add(CollectableType type, int amount)
    {
        var current = Get(type);
        current += amount;
        if (current < 0) current = 0;
        OnUpdate?.Invoke(current);
        PlayerPrefs.SetInt(type + "Amount", current < 0 ? 0 : current);
    }

    public static void Remove(CollectableType type, int amount)
    {
        Add(type, -amount);
    }

    public static void RegisterForUpdate(Action<int> action)
    {
        OnUpdate += action;
    }

    public static void DeRegisterForUpdate(Action<int> action)
    {
        OnUpdate -= action;
    }

    public static Sprite GetIcon(CollectableType collectabletype)
    {
        var database = CollectableIconDatabase.Get();
        var items = database.Icons;
        if (items == null) return database.Default;

        foreach (var collectableIcon in items)
        {
            if (collectableIcon.Type == collectabletype) return collectableIcon.Icon;
        }

        return database.Default;
    }
}