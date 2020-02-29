using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public struct DropItemData
{
    public int dropItemId;
    public float dropItemPercent;
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

[System.Serializable]
public class CharacterTable
{
    public int ID;
    public string Name;
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
public class EnemyTable
{
    public int ID;
    public string Name;
    public string Explanation;
    public int EnemyType;
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
public class BossTable
{
    public int ID;
    public string BossName;
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
    public BossSkill Skill_1;
    public BossSkill Skill_2;
    public BossSkill Skill_3;
}

[System.Serializable]
public class ItemTable
{
    public int ID;
    public string Name;
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
public class QuestTable
{
    public int ID;
    public string Name;
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
public class MapTable
{
    public string MapName;
    public int spawnNum;
    public List<Vector3> spwanLocation;
    public List<int> spwanMonster;
    public List<NpcLocationData> NpcLocation;
    public List<BossLocationData> BossLocation;
}

[System.Serializable]
public class ShopTable
{
    public int ShopKind;
    public List<int> ShopItemID;
}

[System.Serializable]
public class EnemyDropItemTable
{
    public int EnemyID;
    public int dropMoneyMin;
    public int dropMoneyMax;
    public List<DropItemData> EnemyDropItemDataList;
}

[System.Serializable]
public class NpcTable
{
    public int ID;
    public int Type;
    public string Name;
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
public class SingleConversationTable
{
    public int ID;
    public string Text;
    public int Type;
}

[System.Serializable]
public class PlayerLevelTable
{
    public int Level;
    public int ExpMax;
    public float AddHp;
    public float AddMp;
    public float AddAtk;
    public float AddDef;
}

public class DataManager : MonoBehaviour
{
    private static DataManager m_instance;

    //싱글톤 접근
    public static DataManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DataManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public string characterTable_FileName = "Character";
    public string enemyTable_FileName = "Enemy";
    public string bossTable_FileName = "Boss";
    public string ItemTable_FileName = "Item";
    public string QuestTable_FileName = "Quest";
    public string MapTable_FlieName = "Map";
    public string ShopTable_FlieName = "Shop";
    public string NpcTable_FileName = "Npc";
    public string EnemyDropItemTable_FileName = "EnemyDropItem";
    public string SingleConversationTable_FileName = "SingleConversationData";
    public string PlayerLevelTable_FileName = "PlayerLevel";
    
    Dictionary<int, CharacterTable> CharacterInfoTable;
    Dictionary<int, EnemyTable> EnemyInfoTable;
    Dictionary<int, BossTable> BossInfoTable;
    Dictionary<int, ItemTable> ItemInfoTable;
    Dictionary<int, QuestTable> QuestInfoTable;
    Dictionary<string, MapTable> MapInfoTable;
    Dictionary<int, ShopTable> ShopInfoTable;
    Dictionary<int, NpcTable> NpcInfoTable;
    Dictionary<int, EnemyDropItemTable> EnemyDropItemInfoTable;
    Dictionary<int, SingleConversationTable> SingleConversationInfoTable;
    Dictionary<int, PlayerLevelTable> PlayerLevelInfoTable;

    Sprite[] CharacterIconData;
    Sprite[] ItemIconData;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
        CharacterInfoTable = new Dictionary<int, CharacterTable>();
        EnemyInfoTable = new Dictionary<int, EnemyTable>();
        BossInfoTable = new Dictionary<int, BossTable>();
        ItemInfoTable = new Dictionary<int, ItemTable>();
        QuestInfoTable = new Dictionary<int, QuestTable>();
        MapInfoTable = new Dictionary<string, MapTable>();
        ShopInfoTable = new Dictionary<int, ShopTable>();
        NpcInfoTable = new Dictionary<int, NpcTable>();
        EnemyDropItemInfoTable = new Dictionary<int, EnemyDropItemTable>();
        SingleConversationInfoTable = new Dictionary<int, SingleConversationTable>();
        PlayerLevelInfoTable = new Dictionary<int, PlayerLevelTable>();

        LoadData();
    }
    void LoadData()
    {
        TextAsset strings = Resources.Load<TextAsset>(characterTable_FileName);
        CharacterTable[] characterData = JsonHelper.FromJson<CharacterTable>(strings.text);

        foreach (var info in characterData)
        {
            CharacterInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(enemyTable_FileName);
        EnemyTable[] enemyData = JsonHelper.FromJson<EnemyTable>(strings.text);

        foreach (var info in enemyData)
        {
            EnemyInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(bossTable_FileName);
        BossTable[] bossData = JsonHelper.FromJson<BossTable>(strings.text);

        foreach (var info in bossData)
        {
            BossInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(ItemTable_FileName);
        ItemTable[] itemData = JsonHelper.FromJson<ItemTable>(strings.text);

        foreach (var info in itemData)
        {
            ItemInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(QuestTable_FileName);
        QuestTable[] questData = JsonHelper.FromJson<QuestTable>(strings.text);

        foreach (var info in questData)
        {
            QuestInfoTable.Add(info.ID, info);
        }

        CharacterIconData = Resources.LoadAll<Sprite>("CharacterIcon");
        ItemIconData = Resources.LoadAll<Sprite>("ItemIcon");

        strings = Resources.Load<TextAsset>(MapTable_FlieName);
        MapTable[] stageTable = JsonHelper.FromJson<MapTable>(strings.text);

        foreach (var info in stageTable)
        {
            MapInfoTable.Add(info.MapName, info);
        }

        strings = Resources.Load<TextAsset>(ShopTable_FlieName);
        ShopTable[] shopData = JsonHelper.FromJson<ShopTable>(strings.text);

        foreach (var info in shopData)
        {
            ShopInfoTable.Add(info.ShopKind, info);
        }

        strings = Resources.Load<TextAsset>(NpcTable_FileName);
        NpcTable[] npcData = JsonHelper.FromJson<NpcTable>(strings.text);

        foreach (var info in npcData)
        {
            NpcInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(EnemyDropItemTable_FileName);
        EnemyDropItemTable[] enmmyDropItemData = JsonHelper.FromJson<EnemyDropItemTable>(strings.text);

        foreach (var info in enmmyDropItemData)
        {
            EnemyDropItemInfoTable.Add(info.EnemyID, info);
        }

        strings = Resources.Load<TextAsset>(SingleConversationTable_FileName);
        SingleConversationTable[] singleConversationData = JsonHelper.FromJson<SingleConversationTable>(strings.text);

        foreach (var info in singleConversationData)
        {
            SingleConversationInfoTable.Add(info.ID, info);
        }

        strings = Resources.Load<TextAsset>(PlayerLevelTable_FileName);
        PlayerLevelTable[] playerLevelData = JsonHelper.FromJson<PlayerLevelTable>(strings.text);

        foreach (var info in playerLevelData)
        {
            PlayerLevelInfoTable.Add(info.Level, info);
        }
    }

    public CharacterTable CharacterInfo(int id)
    {
        return CharacterInfoTable[id];
    }

    public EnemyTable EnemyInfo(int id)
    {
        return EnemyInfoTable[id];
    }

    public BossTable BossInfo(int id)
    {
        return BossInfoTable[id];
    }

    public ItemTable ItemInfo(int id)
    {
        return ItemInfoTable[id];
    }

    public QuestTable QuestInfo(int id)
    {
        return QuestInfoTable[id];
    }

    public Sprite CharacterIcon(int id)
    {
        return CharacterIconData[id - 1];
    }

    public Sprite ItemIcon(int id)
    {
        return ItemIconData[id - 1];
    }

    public MapTable MapInfo(string mapName)
    {
        return MapInfoTable[mapName];
    }
    public ShopTable ShopInfo(int id)
    {
        return ShopInfoTable[id];
    }
    public NpcTable NpcInfo(int id)
    {
        return NpcInfoTable[id];
    }
    public EnemyDropItemTable EnmeyDropItemInfo(int id)
    {
        return EnemyDropItemInfoTable[id];
    }
    public SingleConversationTable SingleConversationInfo(int id)
    {
        return SingleConversationInfoTable[id];
    }
    public PlayerLevelTable PlayerLevelInfo(int level)
    {
        return PlayerLevelInfoTable[level];
    }
    public int EnemyDictionaryCount()
    {
        return EnemyInfoTable.Count;
    }

}

//json배열을 입력 받기 위해 한번 씌워주는 클래스
public class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.saveInfos;
    }
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.saveInfos = array;
        return JsonUtility.ToJson(wrapper);
    }
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.saveInfos = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [Serializable]
    private class Wrapper<T>
    {
        public T[] saveInfos;
    }
}