using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : ScriptableObject
{
    public LevelEndType EndType;
    public List<LevelItemData> LevelItems = new List<LevelItemData>();
}

[Serializable]
public struct LevelItemData
{
    public int ReferenceID;
    public Vector3 Position;
}

public enum LevelEndType
{
    BossMatch,
    WallRun,
    TankRun,
    StepFinisher,
    SoldierFinisher,
    Chest
}