using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private Inventory _inventory;
    private Equipment _equipment;
    private static InventoryManager m_instance;

    //부모가 될 canvas 객체
    private Canvas uiCanvas;
    //회복시에 띄우는 ui를 저정합니다.
    public GameObject TextMessageUIPrefab;

    //싱글톤 접근
    public static InventoryManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<InventoryManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        _inventory = GetComponent<Inventory>();
        _equipment = GetComponent<Equipment>();
        
    }

    private void Start()
    {
        uiCanvas = UIManager.instance.gameUI.GetComponent<Canvas>();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public Equipment GetEquipment()
    {
        return _equipment;
    }

    public bool AddItem(Item itemTemp)
    {
        if (_inventory.AddInventroyItem(itemTemp))
        {
            QuestManager.instance.updateQuest(EQuestKind.EQuestKind_Collection, itemTemp.itemInfomation.ID);
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
                else if(num == _equipment.MpPotionNum)
                    _equipment.MpPotionNum = -1;
            }
        }

        QuestManager.instance.updateQuest(EQuestKind.EQuestKind_Collection, itemTemp.itemInfomation.ID);

        return true;
    }

    public void UseingItem(int num)
    {
        Item itemInfomationTemp = GetItem(num);

        if (itemInfomationTemp == null) return;

        if ((Item.ItemType)itemInfomationTemp.itemType == Item.ItemType.UseItem)
        {
            if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.HpPotion)
            {
                PlayerManager.instance.Hp +=itemInfomationTemp.itemInfomation.RecoveryHp;

                //회복 수치 ui를 출력합니다
                var ExpUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
                ExpUITemp.name = "RecoveryHp";
                ExpUITemp.GetComponent<TextMessageUI>().Setting(string.Format("+ {00}", itemInfomationTemp.itemInfomation.RecoveryHp), new Color(0, 255, 0), PlayerManager.instance.transform.position, new Vector3(10.0f, 0.5f, 0.0f), uiCanvas);

                ParticleManager.instance.Play(ParticleName.ParticleName_Player_HpPotion, PlayerManager.instance.transform, Vector3.zero);

            }
            else if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.MpPotion)
            {
                PlayerManager.instance.Mp += itemInfomationTemp.itemInfomation.RecoveryMp;

                //회복 수치 ui를 출력합니다
                var ExpUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
                ExpUITemp.name = "RecoveryMp";
                ExpUITemp.GetComponent<TextMessageUI>().Setting(string.Format("+ {00}", itemInfomationTemp.itemInfomation.RecoveryMp), new Color(0, 0, 255), PlayerManager.instance.transform.position, new Vector3(10.0f, 0.5f, 0.0f), uiCanvas);

                ParticleManager.instance.Play(ParticleName.ParticleName_Player_MpPotion, PlayerManager.instance.transform, Vector3.zero);
            }

            DeleteItem(num, 1);
        }

    }

    public Item GetItem(int num)
    {
        return _inventory.GetInventoryItem(num);
    }

    public Item GetItem_SearchID(int id)
    {
        return _inventory.GetInventoryItem_SearchID(id);
    }

    public Item GetEquipmentItem(EquipItem.EquipItemType type)
    {
        if (type == EquipItem.EquipItemType.Weapon)
        {
            return (Item)_equipment.weapon;
        }
        else if(type == EquipItem.EquipItemType.Armor)
        {
            return (Item)_equipment.armor;
        }
        else if(type == EquipItem.EquipItemType.Shield)
        {
            return (Item)_equipment.shield;
        }

        return null;
    }

    public int GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType type)
    {
        if (type == UseItem.UseItemType.HpPotion)
        {
            return _equipment.HpPotionNum;
        }
        else if (type == UseItem.UseItemType.MpPotion)
        {
            return _equipment.MpPotionNum;
        }

        return -1;
    }

    public bool EquipmentingItem(int num)
    {
        Item itemInfomationTemp = GetItem(num);
        Item itemTemp = null;

        if (itemInfomationTemp == null) return false;

        if((Item.ItemType)itemInfomationTemp.itemType==Item.ItemType.EquipItem)
        {
            itemTemp = _equipment.Release((EquipItem.EquipItemType)itemInfomationTemp.itemInfomation.DetailKind);
            _equipment.Equip((EquipItem)GetItem(num));

            if(itemTemp != null)
                AddItem(itemTemp);

            DeleteItem(num, 1);
        }
        else if ((Item.ItemType)itemInfomationTemp.itemType == Item.ItemType.UseItem)
        {
            if((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.HpPotion)
            {
                _equipment.HpPotionNum = num;
                
            }
            else if ((UseItem.UseItemType)itemInfomationTemp.itemInfomation.DetailKind == UseItem.UseItemType.MpPotion)
            {
                _equipment.MpPotionNum = num;
                
            }
        }

        return true;
    }

    public void ReleaseEquipment(EquipItem.EquipItemType type)
    {
        Item itemTemp = _equipment.Release(type);
        
        if(itemTemp != null)
        {
            var setTemp = DropItemManager.instance.ItemSetting(itemTemp.itemInfomation.ID);
            InventoryManager.instance.AddItem(setTemp);
            itemTemp = null;
        }
    }

    public void ReleaseUsingEquipment(UseItem.UseItemType type)
    {
        if (type == UseItem.UseItemType.HpPotion)
            _equipment.HpPotionNum = -1;
        else if (type == UseItem.UseItemType.MpPotion)
            _equipment.MpPotionNum = -1;
    }


    public int NowInventoryItemCount()
    {
        return _inventory.nowInventoryCount;
    }

    public int MaxInventoryItemCount()
    {
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
}
