using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureData : MonoBehaviour
{
    public static CreatureData instance;
    public CreatureDataItem[] creatureDataItems;

    private void Awake()
    {
        instance = this;
    }

    public GameObject ReturnCreature(int Id)
    {
        return (from creature in creatureDataItems where creature.creatureId == Id select creature.creaturePrefab).FirstOrDefault();
    }
}

[System.Serializable]
public class CreatureDataItem
{
    public int creatureId;
    public GameObject creaturePrefab;
}