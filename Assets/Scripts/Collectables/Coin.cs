using UnityEngine;
using System;

public class Coin : MonoBehaviour, ICollectable
{
    public static event Action<int> OnCoinCollect;
    int amt = 1;
    public void Collect()
    {
        Debug.Log("Coin Collected");
        OnCoinCollect?.Invoke(amt);
        Destroy(gameObject);
    }
}
