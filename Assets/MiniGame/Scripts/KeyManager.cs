using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;

    private void Awake()
    {
        instance = this;
    }

    public int keys
    {
        get => PlayerPrefs.GetInt("keys", 20);
        set => PlayerPrefs.SetInt("keys", value < 0 ? 0 : value);
    }

    public void Add(int amount)
    {
        keys += amount;
    }

    public void Remove(int amount)
    {
        keys -= amount;
    }

    public int GetKeys()
    {
        return keys;
    }
}