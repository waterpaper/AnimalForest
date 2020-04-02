using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{
    private Inventory _inventory;
    private Equipment _equipment;

    public Inventory GetInventory()
    {
        //인벤토리 클래스를 반환합니다.
        return _inventory;
    }

    public Equipment GetEquipment()
    {
        //장비하고있는 아이템을 나타내는 클래스를 반환합니다.
        return _equipment;
    }

    public int NowInventoryItemCount()
    {
        //현재 인벤토리 숫자입니다
        return _inventory.nowInventoryCount;
    }

    public int MaxInventoryItemCount()
    {
        //최대 인벤토리 아이템 숫자를 반환합니다.
        return _inventory.maxInventoryCount;
    }

    public bool NowEquipmentWeaponItem()
    {
        //현재 무기를 장착하고 잇는지 여부를 판단하는 함수입니다.
        if (_equipment.weapon == null)
            return false;
        else
            return true;
    }

    public bool NowEquipmentHpPotionItem()
    {
        //현재 hp 포션을 장착하고 잇는지 여부를 판단해주는 함수입니다.
        if (_equipment.HpPotionNum == -1)
            return false;
        else
            return true;
    }

    public bool NowEquipmentMpPotionItem()
    {
        //현재 mp 포션을 장착하고 잇는지 여부를 판단해주는 함수입니다.
        if (_equipment.MpPotionNum == -1)
            return false;
        else
            return true;
    }

    public Item GetItem(int num)
    {
        //인벤토리의 아이템을 반환합니다.
        return _inventory.GetInventoryItem(num);
    }

    public Item GetItem_SearchID(int id)
    {
        //인벤토리 아이템중 해당 id의 아이템을 반환합니다.
        return _inventory.GetInventoryItem_SearchID(id);
    }

    public Item GetEquipmentItem(EquipItem.EquipItemType type)
    {
        //현재 장비하고 있는 장비를 반환합니다.
        if (type == EquipItem.EquipItemType.Weapon)
            return (Item)_equipment.weapon;

        else if (type == EquipItem.EquipItemType.Armor)
            return (Item)_equipment.armor;

        else if (type == EquipItem.EquipItemType.Shield)
            return (Item)_equipment.shield;

        return null;
    }

    public int GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType type)
    {
        //현재 장비하고 있는 포션의 인벤토리 넘버를 반환합니다.
        if (type == UseItem.UseItemType.HpPotion)
            return _equipment.HpPotionNum;

        else if (type == UseItem.UseItemType.MpPotion)
            return _equipment.MpPotionNum;

        return -1;
    }


    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        _inventory = GetComponent<Inventory>();
        _equipment = GetComponent<Equipment>();

    }

    public bool AddItem(Item itemTemp, int inventoryNumber = 0, bool load = false)
    {
        //아이템을 추가합니다.
        if (load == true)
        {
            //로드 상태일시 인벤토리 해당 위치에 추가합니다.
            if (_inventory.AddInventroyItem(itemTemp, inventoryNumber))
            {
                QuestManager.instance.UpdateQuest(EQuestKind.EQuestKind_Collection, itemTemp.itemInfomation.ID);
                return true;
            }

            return false;
        }

        if (_inventory.AddInventroyItem(itemTemp))
        {
            //아이템을 추가하면서 퀘스트를 업데이트해줍니다.
            QuestManager.instance.UpdateQuest(EQuestKind.EQuestKind_Collection, itemTemp.itemInfomation.ID);
            return true;
        }
        else
        {
            //아이템 추가에 실패했을때
            return false;
        }
    }

    public bool DeleteItem(int num, int count)
    {
        //아이템을 제거합니다.(사용, 판매시)
        Item itemTemp = _inventory.GetInventoryItem(num);

        if (itemTemp == null) return false;

        itemTemp.count -= count;

        if (itemTemp.count > 0)
        {
            if (!_inventory.SettingItemCount(num, itemTemp.count)) return false;
        }
        else
        {
            if (!_inventory.DeleteInventoryItem(num)) return false;
            else
            {
                if (num == _equipment.HpPotionNum)
                    _equipment.HpPotionNum = -1;
                else if (num == _equipment.MpPotionNum)
                    _equipment.MpPotionNum = -1;
            }
        }

        QuestManager.instance.UpdateQuest(EQuestKind.EQuestKind_Collection, itemTemp.itemInfomation.ID);

        return true;
    }

    public void UseingItem(int num)
    {
        //아이템을 사용합니다.
        Item itemInfomationTemp = GetItem(num);

        if (itemInfomationTemp == null) return;

        if ((Item.ItemType)itemInfomationTemp.itemType == Item.ItemType.UseItem)
        {
            if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.HpPotion)
            {
                PlayerManager.instance.Hp += itemInfomationTemp.itemInfomation.RecoveryHp;

                //회복 수치 ui를 출력합니다
                UIManager.instance.PrintGameText(string.Format("+ {00}", itemInfomationTemp.itemInfomation.RecoveryHp), PlayerManager.instance.transform,
                    new Vector3(10.0f, 0.5f, 0.0f), new Color(0, 255, 0));
                ParticleManager.instance.Play(ParticleName.ParticleName_Player_HpPotion, PlayerManager.instance.transform, Vector3.zero);

            }
            else if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.MpPotion)
            {
                PlayerManager.instance.Mp += itemInfomationTemp.itemInfomation.RecoveryMp;

                //회복 수치 ui를 출력합니다
                UIManager.instance.PrintGameText(string.Format("+ {00}", itemInfomationTemp.itemInfomation.RecoveryMp), PlayerManager.instance.transform,
                    new Vector3(10.0f, 0.5f, 0.0f), new Color(0, 0, 255));
                ParticleManager.instance.Play(ParticleName.ParticleName_Player_MpPotion, PlayerManager.instance.transform, Vector3.zero);
            }

            //효과를 실행후 아이템을 1개 제거합니다.
            DeleteItem(num, 1);
        }

    }

    //해당 인벤토리에 존재하는 아이템을 장비합니다.
    public bool EquipmentingItem(int num)
    {
        if (num < 0) return false;

        Item itemInfomationTemp = GetItem(num);
        Item itemTemp = null;

        if (itemInfomationTemp == null) return false;

        if ((Item.ItemType)itemInfomationTemp.itemType == Item.ItemType.EquipItem)
        {
            itemTemp = _equipment.Release((EquipItem.EquipItemType)itemInfomationTemp.itemInfomation.DetailKind);
            _equipment.Equip((EquipItem)GetItem(num));

            //원래 장비하고 있던 장비를 인벤토리에 다시 추가합니다.
            if (itemTemp != null)
                AddItem(itemTemp);

            DeleteItem(num, 1);
        }
        else if ((Item.ItemType)itemInfomationTemp.itemType == Item.ItemType.UseItem)
        {
            if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.HpPotion)
                _equipment.HpPotionNum = num;
            
            else if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.MpPotion)
                _equipment.MpPotionNum = num;
        }

        return true;
    }

    //해당 아이템을 장비합니다.(사용 아이템 x)
    public bool EquipmentingItem(Item.ItemType type, Item item)
    {
        Item itemTemp = null;

        if (item == null) return false;

        if ((Item.ItemType)item.itemType == Item.ItemType.EquipItem)
        {
            itemTemp = _equipment.Release((EquipItem.EquipItemType)item.itemInfomation.DetailKind);
            _equipment.Equip((EquipItem)item);

            if (itemTemp != null)
                AddItem(itemTemp);
        }

        return true;
    }

    public void ReleaseEquipment(EquipItem.EquipItemType type)
    {
        //해당 장비를 해제합니다.
        Item itemTemp = _equipment.Release(type);

        if (itemTemp != null)
        {
            var setTemp = DataManager.instance.ItemSetting(itemTemp.itemInfomation.ID);
            InventoryManager.instance.AddItem(setTemp);
            itemTemp = null;
        }
    }

    public void ReleaseUsingEquipment(UseItem.UseItemType type)
    {
        //해당 사용 아이템 장비를 해제합니다.
        if (type == UseItem.UseItemType.HpPotion)
            _equipment.HpPotionNum = -1;
        else if (type == UseItem.UseItemType.MpPotion)
            _equipment.MpPotionNum = -1;
    }

    public void LoadInventoryAndEquipment(PlayerSaveData loadData)
    {
        //로드한 데이터를 기준으로 인벤토리와 장비를 세팅해줍니다.
        //장비를 세팅합니다.
        EquipmentingItem(Item.ItemType.EquipItem, DataManager.instance.ItemSetting(loadData.EquipWeaponItem));
        EquipmentingItem(Item.ItemType.EquipItem, DataManager.instance.ItemSetting(loadData.EquipArmorItem));
        EquipmentingItem(Item.ItemType.EquipItem, DataManager.instance.ItemSetting(loadData.EquipShieldItem));

        //인벤토리를 세팅합니다
        loadData.ItemList.ForEach((saveItemTemp) =>
        {
            Item itemTemp = DataManager.instance.ItemSetting(saveItemTemp.ItemID);
            itemTemp.count = saveItemTemp.ItemCount;
            AddItem(itemTemp, saveItemTemp.InventoryNum, true);
        });

        //퀵슬롯 포션을 세팅합니다.
        EquipmentingItem(loadData.EquipHpPotion);
        EquipmentingItem(loadData.EquipMpPotion);

    }
}
