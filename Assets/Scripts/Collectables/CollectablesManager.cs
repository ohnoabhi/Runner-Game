using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablesManager :MonoBehaviour
{
    
    int coinCount;
    int gemCount;
    private void OnEnable()
    {
        Coin.OnCoinCollect += CoinCollector;
        Gem.OnGemCollect += GemCollector;
    }
    private void OnDisable()
    {
        Coin.OnCoinCollect -= CoinCollector;
        Gem.OnGemCollect -= GemCollector;
    }
    public void CoinCollector(int amt)
    {
        coinCount = coinCount + amt;
        Debug.Log("Coin =" + coinCount);
    }

    public void GemCollector(int amt)
    {
        gemCount = gemCount + amt;
        Debug.Log("gem =" + gemCount);
    }
}
