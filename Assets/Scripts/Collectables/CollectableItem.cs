using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public CollectableType type;
    public int amount = 1;

    public void Collect()
    {
        CollectablesManager.Add(type, amount);
        Destroy(gameObject);
    }

}
