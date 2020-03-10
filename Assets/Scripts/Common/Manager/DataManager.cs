using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    //싱글톤 접근
    public static DataManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DataManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private const string _characterTable_FileName = "Character";
    private const string _enemyTable_FileName = "Enemy";
    private const string _bossTable_FileName = "Boss";
    private const string _ItemTable_FileName = "Item";
    private const string _QuestTable_FileName = "Quest";
    private const string _MapTable_FlieName = "Map";
    private const string _ShopTable_FlieName = "Shop";
    private const string _NpcTable_FileName = "Npc";
    private const string _EnemyDropItemTable_FileName = "EnemyDropItem";
    private const string _SingleConversationTable_FileName = "SingleConversationData";
    private const string _PlayerLevelTable_FileName = "PlayerLevel";
    
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

        AllLoadData();
    }

    void AllLoadData()
    {
        //딕셔너리에 데이터를 저장합니다.
        LoadDictionaryData(CharacterInfoTable, _characterTable_FileName);
        LoadDictionaryData(EnemyInfoTable, _enemyTable_FileName);
        LoadDictionaryData(BossInfoTable, _bossTable_FileName);
        LoadDictionaryData(ItemInfoTable, _ItemTable_FileName);
        LoadDictionaryData(QuestInfoTable, _QuestTable_FileName);
        LoadDictionaryData(MapInfoTable, _MapTable_FlieName);
        LoadDictionaryData(ShopInfoTable, _ShopTable_FlieName);
        LoadDictionaryData(NpcInfoTable, _NpcTable_FileName);
        LoadDictionaryData(EnemyDropItemInfoTable, _EnemyDropItemTable_FileName);
        LoadDictionaryData(SingleConversationInfoTable, _SingleConversationTable_FileName);
        LoadDictionaryData(PlayerLevelInfoTable, _PlayerLevelTable_FileName);

        //이미지를 리스트에 저장합니다.
        CharacterIconData = Resources.LoadAll<Sprite>("CharacterIcon");
        ItemIconData = Resources.LoadAll<Sprite>("ItemIcon");
    }

    //딕셔너리 내부의 데이터를 저장해주는 함수입니다.
    public void LoadDictionaryData<Table>(Dictionary<int, Table> dictionary, string path)
        where Table : TableInfo
    {
        TextAsset strings = Resources.Load<TextAsset>(path);
        Table[] data = JsonHelper.FromJson<Table>(strings.text);

        foreach (var info in data)
        {
            dictionary.Add(info.ID, info);
        }
    }

    public void LoadDictionaryData<Table>(Dictionary<string, Table> dictionary, string path)
        where Table : TableInfo
    {
        TextAsset strings = Resources.Load<TextAsset>(path);
        Table[] data = JsonHelper.FromJson<Table>(strings.text);

        foreach (var info in data)
        {
            dictionary.Add(info.Name, info);
        }
    }

    /*
    public table GetTableData<table>()
    {
        if(table.GetType())
        { }
    }
    */

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
    //아이템을 세팅해서 리턴합니다.
    public Item ItemSetting(int itemID)
    {
        if (itemID < 0) return null;

        ItemTable itemTableTemp = ItemInfo(itemID);

        switch ((Item.ItemType)itemTableTemp.ItemKind)
        {
            case Item.ItemType.EquipItem:
                EquipItem equipItemTemp = new EquipItem(itemID, 1, itemTableTemp.DetailKind);
                return equipItemTemp;
            case Item.ItemType.UseItem:
                UseItem useItemTemp = new UseItem(itemID, 1, itemTableTemp.DetailKind);
                return useItemTemp;
            case Item.ItemType.EtcItem:
                EtcItem EtcitemTemp = new EtcItem(itemID, 1, itemTableTemp.DetailKind);
                return EtcitemTemp;
            default:
                return null;
        }
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