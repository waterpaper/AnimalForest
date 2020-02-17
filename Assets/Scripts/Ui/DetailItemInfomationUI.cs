using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text;

public class DetailItemInfomationUI : MonoBehaviour, IPointerClickHandler
{
    public int inventoryNumber = 0;
    public GameObject detailItemInfomationUI;
    public GameObject inventoryUI;
    public bool isActive;
    public RectTransform UIRectTransform;

    private void Awake()
    {
        detailItemInfomationUI = GameObject.FindGameObjectWithTag("DetailItemUI");
        UIRectTransform = detailItemInfomationUI.GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        detailItemInfomationUI.SetActive(false);
        isActive = false;    
    }

    public string ItemKindString(Item itemTemp)
    {
        //아이템 종류에 따라서 맞는 문구를 반환해 주는 함수입니다.
        switch((Item.ItemType)itemTemp.itemInfomation.ItemKind)
        {
            case Item.ItemType.EquipItem:
                if(itemTemp.itemInfomation.DetailKind==(int)EquipItem.EquipItemType.Weapon)
                {
                    return "무기";
                }
                else if(itemTemp.itemInfomation.DetailKind == (int)EquipItem.EquipItemType.Armor)
                {
                    return "갑옷";
                }
                else if(itemTemp.itemInfomation.DetailKind == (int)EquipItem.EquipItemType.Shield)
                {
                    return "방패";
                }
                break;
            case Item.ItemType.UseItem:
                if(itemTemp.itemInfomation.DetailKind == (int)UseItem.UseItemType.HpPotion)
                {
                    return "체력포션";
                }
                else if (itemTemp.itemInfomation.DetailKind == (int)UseItem.UseItemType.MpPotion)
                {
                    return "마나포션";
                }
                else if (itemTemp.itemInfomation.DetailKind == (int)UseItem.UseItemType.Buff)
                {
                    return "버프";
                }
                break;
            case Item.ItemType.EtcItem:
                if (itemTemp.itemInfomation.DetailKind == (int)EtcItem.EtcItemType.Etc)
                {
                    return "기타";
                }
                else if (itemTemp.itemInfomation.DetailKind == (int)EtcItem.EtcItemType.Ingredient)
                {
                    return "재료";
                }
                break;
            default:
                break;
        }

        return null;
    }

    public string ItemOptionString(Item itemTemp)
    {
        StringBuilder temp = new StringBuilder();

        if(itemTemp.itemInfomation.RecoveryHp>0.0f)
        {
            temp.AppendLine();
            temp.Append("체력회복 : + ");
            temp.Append(itemTemp.itemInfomation.RecoveryHp);
        }
        if (itemTemp.itemInfomation.RecoveryMp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("마나회복 : + ");
            temp.Append(itemTemp.itemInfomation.RecoveryMp);
        }
        if (itemTemp.itemInfomation.AddHp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("체력 : ");
            temp.Append(itemTemp.itemInfomation.AddHp);
        }
        if (itemTemp.itemInfomation.AddMp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("마나 : ");
            temp.Append(itemTemp.itemInfomation.AddMp);
        }
        if (itemTemp.itemInfomation.AddAtk > 0.0f)
        {
            temp.AppendLine();
            temp.Append("공격력 : ");
            temp.Append(itemTemp.itemInfomation.AddAtk);
        }
        if (itemTemp.itemInfomation.AddDef > 0.0f)
        {
            temp.AppendLine();
            temp.Append("방어력 : + ");
            temp.Append(itemTemp.itemInfomation.AddDef);
        }

        temp.AppendLine();
        temp.Append("판매가격 : ");
        temp.Append(itemTemp.itemInfomation.Value * 0.5f);


        return temp.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Item itemTemp = InventoryManager.instance.GetItem(inventoryNumber);

        if (inventoryUI.GetComponent<InventoryUI>().selectNumber == inventoryNumber)
        {
            detailItemInfomationUI.SetActive(false);
            isActive = false;
            inventoryUI.GetComponent<InventoryUI>().selectNumber = -1;
        }
        else if (itemTemp != null)
        {
            detailItemInfomationUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemTemp.itemInfomation.Name;

            string itemKindStringTemp = ItemKindString(itemTemp);

            if (itemKindStringTemp != null)
                detailItemInfomationUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = itemKindStringTemp;
            else
                detailItemInfomationUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "기타";

            detailItemInfomationUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemTemp.itemInfomation.Explanation;
            detailItemInfomationUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = ItemOptionString(itemTemp);
            detailItemInfomationUI.SetActive(true);

            inventoryUI.GetComponent<InventoryUI>().selectNumber = inventoryNumber;

            isActive = true;
        }
    }
}
