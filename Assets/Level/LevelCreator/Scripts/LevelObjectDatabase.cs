using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[CreateAssetMenu(fileName = "Level Objects", menuName = "Level/Objects", order = 0)]
public class LevelObjectDatabase : ScriptableObject
{
    public List<LevelFinisher> LevelFinishers;
    public List<LevelObject> LevelObjects = new List<LevelObject>();

    public LevelObject Get(int referenceID)
    {
        return LevelObjects.Where((t, i) => i == referenceID).FirstOrDefault();
    }

    public static LevelObjectDatabase Get()
    {
        try
        {
            return Resources.Load<LevelObjectDatabase>("Level/Level Objects");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public GameFinisher GetFinisher(LevelEndType endType)
    {
        return (from levelFinisher in LevelFinishers
            where levelFinisher.EndType == endType
            select levelFinisher.Finisher).FirstOrDefault();
    }
}

[Serializable]
public class LevelFinisher
{
    public LevelEndType EndType;
    public GameFinisher Finisher;
}