using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopHudController : MonoBehaviour
{
    public static TopHudController instance;
    [SerializeField]
    private TopHudItem[] items;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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
        return items[0].gameObject.transform.position;
    }
}
