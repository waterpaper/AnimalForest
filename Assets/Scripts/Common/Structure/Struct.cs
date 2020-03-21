using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// data
/// </summary>
[System.Serializable]
public struct DropItemData
{
    public int DropItemId;
    public float DropItemPercent;
}
[System.Serializable]
public struct NpcLocationData
{
    public int NpcID;
    public Vector3 Location;
    public Vector3 Rotation;
}
[System.Serializable]
public struct BossLocationData
{
    public int BossID;
    public Vector3 Location;
}


/// <summary>
/// boss
/// </summary>
[System.Serializable]
public struct BossSkill
{
    public string Name;
    public int Mp;
    public float Factor;
    public float Range;
    public float Time;
}

/// <summary>
/// save
/// </summary>
[System.Serializable]
public struct SaveItemData
{
    public int ItemID;
    public int ItemCount;
    public int InventoryNum;
}
[System.Serializable]
public struct SaveQuestData
{
    public int QuestID;
    public int TargetNowCount;
}