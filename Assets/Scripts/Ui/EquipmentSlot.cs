using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentSlot : MonoBehaviour
{
    public enum EquipmentSlotType
    {
        EquipmentSlotType_Weapon,
        EquipmentSlotType_Armor,
        EquipmentSlotType_Shield,
        EquipmentSlotType_HpPotion,
        EquipmentSlotType_MpPotion,
        EquipmentSlotType_End
    }

    public EquipmentSlotType type = EquipmentSlotType.EquipmentSlotType_End;

    public void OnDisable()
    {
        this.transform.GetChild(0).GetComponent<Image>().enabled = false;
        this.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        int numTemp = -1;
        Item itemTemp = null;

        if(type == EquipmentSlotType.EquipmentSlotType_Weapon)
        {
            itemTemp = InventoryManager.instance.GetEquipmentItem(EquipItem.EquipItemType.Weapon);
        }
        else if(type == EquipmentSlotType.EquipmentSlotType_Armor)
        {
            itemTemp = InventoryManager.instance.GetEquipmentItem(EquipItem.EquipItemType.Armor);
        }
        else if(type == EquipmentSlotType.EquipmentSlotType_Shield)
        {
            itemTemp = InventoryManager.instance.GetEquipmentItem(EquipItem.EquipItemType.Shield);
        }
        else if(type == EquipmentSlotType.EquipmentSlotType_HpPotion)
        {
            numTemp = InventoryManager.instance.GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType.HpPotion);

            if (numTemp != -1)
                itemTemp = InventoryManager.instance.GetItem(numTemp);
        }
        else if(type == EquipmentSlotType.EquipmentSlotType_MpPotion)
        {
            numTemp = InventoryManager.instance.GetEquipmentPotionItemInventoryNumber(UseItem.UseItemType.MpPotion);

            if (numTemp != -1)
                itemTemp = InventoryManager.instance.GetItem(numTemp);
        }

        if (itemTemp != null)
        {
            //빈칸을 출력하지 않기 위해 꺼두던 아이템이미지 컴포넌트틀 켜줍니다.
            this.transform.GetChild(0).GetComponent<Image>().enabled = true;
            this.transform.GetChild(0).GetComponent<Image>().sprite = DataManager.instance.ItemIcon(itemTemp.itemInfomation.ID);

            if (itemTemp.count == 1)
            {
                this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            else
            {
                this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemTemp.count.ToString();
            }
            this.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            this.transform.GetChild(0).GetComponent<Image>().enabled = false;
            this.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            this.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void ReleaseButton()
    {
        if (type == EquipmentSlotType.EquipmentSlotType_Weapon)
        {
            InventoryManager.instance.ReleaseEquipment(EquipItem.EquipItemType.Weapon);
        }
        else if (type == EquipmentSlotType.EquipmentSlotType_Armor)
        {
            InventoryManager.instance.ReleaseEquipment(EquipItem.EquipItemType.Armor);
        }
        else if (type == EquipmentSlotType.EquipmentSlotType_Shield)
        {
            InventoryManager.instance.ReleaseEquipment(EquipItem.EquipItemType.Shield);
        }
        else if (type == EquipmentSlotType.EquipmentSlotType_HpPotion)
        {
            InventoryManager.instance.ReleaseUsingEquipment(UseItem.UseItemType.HpPotion);
        }
        else if (type == EquipmentSlotType.EquipmentSlotType_MpPotion)
        {
            InventoryManager.instance.ReleaseUsingEquipment(UseItem.UseItemType.MpPotion);
        }
    }
}
