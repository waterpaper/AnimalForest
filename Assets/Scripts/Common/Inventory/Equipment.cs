using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public EquipItem weapon = null, armor = null, shield = null;
    public int HpPotionNum = -1, MpPotionNum = -1;

    public void Equip(EquipItem item)
    {
        //장비 아이템을 장비합니다.
        EquipItem equipItemTemp = new EquipItem(item.itemInfomation.ID,1, (int)item.itemInfomation.DetailKind); 

        if (item.equipType == EquipItem.EquipItemType.Weapon)
        {
            weapon = equipItemTemp;
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalWeapon, item.itemInfomation.ID);
        }
        else if (item.equipType == EquipItem.EquipItemType.Armor)
        {
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalArmor, item.itemInfomation.ID);
            armor = equipItemTemp;
        }
        else if (item.equipType == EquipItem.EquipItemType.Shield)
        {
            shield = equipItemTemp;
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalShield, item.itemInfomation.ID);
        }

        playerOption(true, (Item)equipItemTemp);
    }

    public Item Release(EquipItem.EquipItemType kind)
    {
        //장비아이템을 해제합니다.
        Item itemTemp =null;

        if (kind == EquipItem.EquipItemType.Weapon)
        {
            if (weapon == null) return null;
            
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalWeapon, -1);
            itemTemp = (Item)weapon;
            weapon = null;
        }
        else if (kind == EquipItem.EquipItemType.Armor)
        {
            if (armor == null) return null;
            
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalArmor, -1);
            itemTemp = (Item)armor;
            armor = null;
        }
        else if (kind == EquipItem.EquipItemType.Shield)
        {
            if (shield == null) return null;
            
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalShield, -1);
            itemTemp = (Item)shield;
            shield = null;
        }

        playerOption(false, itemTemp);

        return itemTemp;
    }

    public void playerOption(bool isEquip, Item item)
    {
        //아이템의 옵션을 적용시킵니다.
        //장비해제시 옵션만큼의 능력치를 없앱니다.
        int value = 1;

        if (!isEquip)
            value = -1;

        if (item.itemInfomation.AddHp > 0.0f)
            PlayerManager.instance.AddHp += (item.itemInfomation.AddHp * value);
        if (item.itemInfomation.AddMp > 0.0f)
            PlayerManager.instance.AddMp += (item.itemInfomation.AddMp * value);
        if (item.itemInfomation.AddAtk > 0.0f)
            PlayerManager.instance.AddAtk += (item.itemInfomation.AddAtk * value);
        if (item.itemInfomation.AddDef > 0.0f)
            PlayerManager.instance.AddDef += (item.itemInfomation.AddDef * value);
    }
}
