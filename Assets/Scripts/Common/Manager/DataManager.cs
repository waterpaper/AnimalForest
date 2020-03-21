using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
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
    //public Dictionary<string, MapTable> MapInfos { get { return MapInfoTable; } }
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
            Destroy(this.gameObject);
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
    //원하는 데이터를 반환해주는 함수입니다.
    public T GetTableData<T>(TableDataKind kind, int id = 0, string name = null)
    {
        if(T.GetType())

        switch (kind)
        {
            case TableDataKind.TableDataKind_Character:
                return CharacterInfoTable[id];

            case TableDataKind.TableDataKind_Enemy:
                return EnemyInfoTable[id];

            case TableDataKind.TableDataKind_Boss:
                return BossInfoTable[id];

            case TableDataKind.TableDataKind_Item:
                return ItemInfoTable[id];

            case TableDataKind.TableDataKind_Quest:
                return QuestInfoTable[id];

            case TableDataKind.TableDataKind_Map:
                return MapInfoTable[name];

            case TableDataKind.TableDataKind_Shop:
                return ShopInfoTable[id];

            case TableDataKind.TableDataKind_Npc:
                return NpcInfoTable[id];

            case TableDataKind.TableDataKind_EnemyDropItem:
                return EnemyDropItemInfoTable[id];

            case TableDataKind.TableDataKind_SingleConversation:
                return SingleConversationInfoTable[id];

            case TableDataKind.TableDataKind_PlayerLevel:
                return PlayerLevelInfoTable[id];

            default:
                return null;
        }
    }
    */

    //원하는 아이콘을 가져온다
    //이때 id에 맞춰 list에 저장되어 있기 때문에 id에서 -1한 값을 인덱스로 접근해 가져온다. 
    public Sprite GetIconData(IconDataKind kind, int id)
    {
        int index = id - 1;

        switch (kind)
        {
            case IconDataKind.IconDataKind_Character:
                return CharacterIconData[index];

            case IconDataKind.IconDataKind_Item:
                return ItemIconData[index];

            default:
                return null;
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