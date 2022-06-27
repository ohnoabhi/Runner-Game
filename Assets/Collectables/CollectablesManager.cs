using System;
using System.Linq;
using Collectables;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CollectableType
{
    Cash,
    Coin,
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

[Serializable]
public class Collectable
{
    public Sprite Icon;
    public string Audio;
    public CollectableType Type;

    public int Amount
    {
        get => PlayerPrefs.GetInt(Type + "_Amount", Default);
        set => PlayerPrefs.SetInt(Type + "_Amount", value >= 0 ? value : 0);
    }

    public int Default;
}

public class CollectablesManager : MonoBehaviour
{
    private static CollectablesManager instance;
    private static Action<Price> OnUpdate;

    [SerializeField] private Collectable[] Collectables;

    private void Awake()
    {
        instance = this;
    }

    public static int Get(CollectableType type)
    {
        return (from collectable in instance.Collectables where collectable.Type == type select collectable.Amount)
            .FirstOrDefault();
    }

    public static void Add(CollectableType type, int amount)
    {
        foreach (var collectable in instance.Collectables)
        {
            if (collectable.Type == type)
            {
                var final = collectable.Amount + amount;
                if (final < 0) final = 0;
                collectable.Amount = final;

                OnUpdate?.Invoke(new Price() {Type = collectable.Type, Amount = final});
            }
        }
    }

    public static void Remove(CollectableType type, int amount)
    {
        Add(type, -amount);
    }

    public static void RegisterForUpdate(Action<Price> action)
    {
        OnUpdate += action;
    }

    public static void DeRegisterForUpdate(Action<Price> action)
    {
        OnUpdate -= action;
    }

    public static Sprite GetIcon(CollectableType collectabletype)
    {
        return (from collectable in instance.Collectables
            where collectabletype == collectable.Type
            select collectable.Icon).FirstOrDefault();
    }

    public static string GetAudioName(CollectableType type)
    {
        return (from collectable in instance.Collectables
            where type == collectable.Type
            select collectable.Audio).FirstOrDefault();
    }

    [Button]
    private void AddCash()
    {
        Add(CollectableType.Cash, Random.Range(1000, 5000));
    }
}