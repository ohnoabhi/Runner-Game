using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHudController : MonoBehaviour
{
    public static TopHudController instance;
    private TopHudItem[] items;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        items = GetComponentsInChildren<TopHudItem>();
    }

    private void Start()
    {
        gameObject.SetActive(true);
    }

    public static void Enable(bool enabled)
    {
        instance.gameObject.SetActive(enabled);
    }

    public Vector3 GetIconPos(CollectableType collectableType)
    {
        foreach (var item in items)
        {
            if (item.GetCollectableType() == collectableType) return item.GetIconPos();
        }

        return Vector3.zero;
    }

    public Vector2 GetIconSizeDelta(CollectableType collectableType)
    {
        foreach (var item in items)
        {
            if (item.GetCollectableType() == collectableType) return item.GetIconSizeDelta();
        }

        return Vector2.zero;
    }
}