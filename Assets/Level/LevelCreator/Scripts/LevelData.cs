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
    // public int ReferenceID;
    public LevelObject Item;
    public Vector3 Position;
    public int Damage;
    public Multiplier.MultiplierValue MultiplierValue;
    public int CharacterId;
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