using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// tabledata
/// </summary>
[System.Serializable]
public class TableInfo
{
    public int ID;
    public string Name;
}

[System.Serializable]
public class CharacterTable : TableInfo
{
    public int Level;
    public int Exp;
    public int ExpMax;
    public float Hp;
    public float HpMax;
    public float Mp;
    public float MpMax;
    public float Atk;
    public float Def;
}

[System.Serializable]
public class EnemyTable : TableInfo
{
    public string Explanation;
    public int Level;
    public float Hp;
    public float HpMax;
    public float Mp;
    public float MpMax;
    public float Atk;
    public float Def;
    public int Exp;
}

[System.Serializable]
public class BossTable : TableInfo
{
    public int Level;
    public int Exp;
    public int Hp;
    public int HpMax;
    public int Mp;
    public int MpMax;
    public float Atk;
    public float Def;
    public float TraceDist;
    public float PatrolMoveSpeed;
    public float TraceMoveSpeed;
    public BossSkill BossAttack;
    public BossSkill Skill_1;
    public BossSkill Skill_2;
}

[System.Serializable]
public class ItemTable : TableInfo
{
    public string Explanation;
    public int Value;
    public float AddHp;
    public float AddMp;
    public float AddAtk;
    public float AddDef;
    public float RecoveryHp;
    public float RecoveryMp;
    public int ItemKind;
    public int DetailKind;
}

[System.Serializable]
public class QuestTable : TableInfo
{
    public string OrderNpcConversation;
    public string TargetNpcConversation;
    public int MinLevel;
    public int Kind;
    public int OrderNpc;
    public int TargetNpc;
    public int TargetObject;
    public int TargetCount;
    public int RewardExp;
    public int RewardMoney;
    public List<int> RewardItem;
}

[System.Serializable]
public class MapTable : TableInfo
{
    public int SpawnNum;
    public List<Vector3> SpwanLocation;
    public List<int> SpwanMonster;
    public List<NpcLocationData> NpcLocation;
    public List<BossLocationData> BossLocation;
}

[System.Serializable]
public class ShopTable : TableInfo
{
    public int ShopKind;
    public List<int> ShopItemID;
}

[System.Serializable]
public class EnemyDropItemTable : TableInfo
{
    public int DropMoneyMin;
    public int DropMoneyMax;
    public List<DropItemData> EnemyDropItemDataList;
}

[System.Serializable]
public class NpcTable : TableInfo
{
    public int Type;
    public int Level;
    public float Hp;
    public float HpMax;
    public float Mp;
    public float MpMax;
    public float Atk;
    public float Def;
    public int ShopKind;
    public List<String> Conversations;
    public List<int> NpcQuests;
}

[System.Serializable]
public class SingleConversationTable : TableInfo
{
    public string Text;
    public int Type;
}

[System.Serializable]
public class PlayerLevelTable : TableInfo
{
    public int Level;
    public int ExpMax;
    public float AddHp;
    public float AddMp;
    public float AddAtk;
    public float AddDef;
}

/// <summary>
/// save & load data
/// </summary>
[System.Serializable]
public class PlayerSaveData
{
    public string ID;
    public string Name;
    public int Kind;
    public int Money;
    public int Level;
    public int Exp;
    public float Hp;
    public float HpMax;
    public float Mp;
    public float MpMax;
    public float Atk;
    public float Def;

    public int MapNumber;
    public Vector3 MapPosition;

    public int EquipWeaponItem;
    public int EquipArmorItem;
    public int EquipShieldItem;
    public int EquipHpPotion;
    public int EquipMpPotion;

    public string ClearQuest;
    public string ClearEvent;
    public List<SaveItemData> ItemList;
    public List<SaveQuestData> QuestList;
}