using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public CollectableType type;

    public void Collect()
    {
        CollectablesManager.Add(type, 1);
        Destroy(gameObject);
    }

}
