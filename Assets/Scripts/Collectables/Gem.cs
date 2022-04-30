using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour,ICollectable
{
    public static event Action<int> OnGemCollect;
    int amt = 5;
    public void Collect()
    {
        Debug.Log("Coin Collected");
        OnGemCollect?.Invoke(amt);
        Destroy(gameObject);
    }
}
