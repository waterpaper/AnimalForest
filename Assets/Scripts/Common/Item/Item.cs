using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType { EquipItem, UseItem, EtcItem  };
    public ItemType itemType;
    public ItemTable itemInfomation;
    public int count;
}

public class EquipItem : Item
{
    public enum EquipItemType { Weapon, Armor, Shield }
    public EquipItemType equipType;

    public EquipItem(int id, int num, int equipTypeTemp)
    {
        itemType = ItemType.EquipItem;
        equipType = (EquipItemType)equipTypeTemp;
        itemInfomation = DataManager.instance.ItemInfo(id);
        count = num;
    }
}
public class UseItem : Item
{
    public enum UseItemType { HpPotion, MpPotion, Buff}
    public UseItemType useItemType;

    public UseItem(int id, int num, int useTypeTemp)
    {
        itemType = ItemType.UseItem;
        useItemType = (UseItemType)useTypeTemp;
        itemInfomation = DataManager.instance.ItemInfo(id);
        count = num;
    }
}
public class EtcItem: Item
{
    public enum EtcItemType { Etc, Ingredient }
    public EtcItemType etcItemType;

    public EtcItem(int id, int num, int etcTypeTemp)
    {
        itemType = ItemType.EtcItem;
        etcItemType = (EtcItemType)etcTypeTemp;
        itemInfomation = DataManager.instance.ItemInfo(id);
        count = num;
    }
}