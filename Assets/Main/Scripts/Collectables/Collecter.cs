using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var collectable = other.GetComponent<CollectableItem>();

        if(collectable != null)
        {
            collectable.Collect();
        }
    }
}
