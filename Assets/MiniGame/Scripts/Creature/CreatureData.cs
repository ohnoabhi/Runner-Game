using System.Collections;
using System.Collections.Generic;
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
        foreach(var creature in creatureDataItems)
        {
            if(creature.creatureId == Id)
            {
                return creature.creaturePrefab;
            }
        }

        return null;
    }
}

[System.Serializable]
public class CreatureDataItem
{
    public int creatureId;
    public GameObject creaturePrefab;
}
