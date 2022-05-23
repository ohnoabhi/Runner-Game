using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Level Objects", menuName = "Level/Objects", order = 0)]
public class LevelObjectDatabase : ScriptableObject
{
    public List<LevelFinisher> LevelFinishers;
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

    public GameFinisher GetFinisher(LevelEndType endType)
    {
        foreach (var levelFinisher in LevelFinishers)
        {
            if (levelFinisher.EndType == endType) return levelFinisher.Finisher;
        }

        return null;
    }
}
[Serializable]
public class LevelFinisher
{
    public LevelEndType EndType;
    public GameFinisher Finisher;
}