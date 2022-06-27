using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Database", menuName = "Level/Database", order = 0)]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> Levels;

    public static LevelDatabase Get()
    {
        return Resources.Load<LevelDatabase>("Level/Level Database");
    }

    public LevelData GetLevel(int i)
    {
        if (Levels != null && Levels.Count > i)
            return Levels[i];
        return null;
    }
}