using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Objects", menuName = "Level/Objects", order = 0)]
public class LevelObjectDatabase : ScriptableObject
{

        public List<LevelObject> LevelObjects = new List<LevelObject>();

    public LevelObject Get(int referenceID)
    {
        for (var i = 0; i < LevelObjects.Count; i++)
        {
            if (i == referenceID) return LevelObjects[i];
        }

        return null;
    }

    public static LevelObjectDatabase Get()
    {
        return Resources.Load<LevelObjectDatabase>("Level/Level Objects");
    }
}