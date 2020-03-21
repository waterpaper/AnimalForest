using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableDataDictionary<T, Table>
     where Table : TableInfo
{
    Dictionary<T, Table> _dictionary;

    public TableDataDictionary()
    {
        _dictionary = new Dictionary<T, Table>();
    }

    public void SetTableData(T key, Table value)
    {
        _dictionary.Add(key, value);
    }

    public Table GetTableData(T search)
    {
        return _dictionary[search];
    }

    public int Count()
    {
        return _dictionary.Count;
    }
}

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

    TableDataDictionary<int, CharacterTable> CharacterInfoTable;
    TableDataDictionary<int, EnemyTable> EnemyInfoTable;
    TableDataDictionary<int, BossTable> BossInfoTable;
    TableDataDictionary<int, ItemTable> ItemInfoTable;
    TableDataDictionary<int, QuestTable> QuestInfoTable;
    TableDataDictionary<string, MapTable> MapInfoTable;
    TableDataDictionary<int, ShopTable> ShopInfoTable;
    TableDataDictionary<int, NpcTable> NpcInfoTable;
    TableDataDictionary<int, EnemyDropItemTable> EnemyDropItemInfoTable;
    TableDataDictionary<int, SingleConversationTable> SingleConversationInfoTable;
    TableDataDictionary<int, PlayerLevelTable> PlayerLevelInfoTable;

    Sprite[] CharacterIconData;
    Sprite[] ItemIconData;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }

        CharacterInfoTable = new TableDataDictionary<int, CharacterTable>();
        EnemyInfoTable = new TableDataDictionary<int, EnemyTable>();
        BossInfoTable = new TableDataDictionary<int, BossTable>();
        ItemInfoTable = new TableDataDictionary<int, ItemTable>();
        QuestInfoTable = new TableDataDictionary<int, QuestTable>();
        MapInfoTable = new TableDataDictionary<string, MapTable>();
        ShopInfoTable = new TableDataDictionary<int, ShopTable>();
        NpcInfoTable = new TableDataDictionary<int, NpcTable>();
        EnemyDropItemInfoTable = new TableDataDictionary<int, EnemyDropItemTable>();
        SingleConversationInfoTable = new TableDataDictionary<int, SingleConversationTable>();
        PlayerLevelInfoTable = new TableDataDictionary<int, PlayerLevelTable>();

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
    public void LoadDictionaryData<Table>(TableDataDictionary<int, Table> dictionary, string path)
        where Table : TableInfo
    {
        TextAsset strings = Resources.Load<TextAsset>(path);
        Table[] data = JsonHelper.FromJson<Table>(strings.text);

        foreach (var info in data)
        {
            dictionary.SetTableData(info.ID, info);
        }
    }

    public void LoadDictionaryData<Table>(TableDataDictionary<string, Table> dictionary, string path)
        where Table : TableInfo
    {
        TextAsset strings = Resources.Load<TextAsset>(path);
        Table[] data = JsonHelper.FromJson<Table>(strings.text);

        foreach (var info in data)
        {
            dictionary.SetTableData(info.Name, info);
        }
    }

    //원하는 데이터를 가져온다 
    public T GetTableData<T>(TableDataKind kind, int key)
        where T : TableInfo
    {
        switch (kind)
        {
            case TableDataKind.TableDataKind_Character:
                return CharacterInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Enemy:
                return EnemyInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Boss:
                return BossInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Item:
                return ItemInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Quest:
                return QuestInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Shop:
                return ShopInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_Npc:
                return NpcInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_EnemyDropItem:
                return EnemyDropItemInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_SingleConversation:
                return SingleConversationInfoTable.GetTableData(key) as T;

            case TableDataKind.TableDataKind_PlayerLevel:
                return PlayerLevelInfoTable.GetTableData(key) as T;

            default:
                return null;
        }
    }

    //원하는 데이터를 가져온다 
    public T GetTableData<T>(TableDataKind kind, string key)
         where T : TableInfo
    {
        switch (kind)
        {
            case TableDataKind.TableDataKind_Map:
                return MapInfoTable.GetTableData(key) as T; ;

            default:
                return null;
        }
    }

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

    public int EnemyDictionaryCount()
    {
        return EnemyInfoTable.Count();
    }

    //아이템을 세팅해서 리턴합니다.
    public Item ItemSetting(int itemID)
    {
        ItemTable itemTableTemp = ItemInfoTable.GetTableData(itemID);

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