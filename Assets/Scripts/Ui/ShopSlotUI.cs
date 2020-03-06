using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ShopSlotUI : MonoBehaviour
{
    private int itemID = -1;
    private int itemMoney = 0;
    public TextMeshProUGUI itemNameUI;
    public TextMeshProUGUI itemTypeUI;
    public TextMeshProUGUI itemExplanationUI;
    public TextMeshProUGUI itemOptionUI;
    public TextMeshProUGUI itemMoneyUI;
    public Image itemImage;


    private void Awake()
    {
        itemImage = transform.GetChild(1).GetChild(2).GetComponent<Image>();
        itemNameUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemTypeUI = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        itemExplanationUI = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        itemOptionUI = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        itemMoneyUI = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
    }

    public void Setting(int number)
    {
        //상점 데이터의 아이템 정보를 가지고 아이템들을 세팅합니다.
        itemID = number;

        ItemTable itemTemp = DataManager.instance.ItemInfo(itemID);

        itemImage.sprite = DataManager.instance.ItemIcon(itemID);
        itemNameUI.text = itemTemp.Name;
        itemTypeUI.text = ItemKindString(itemTemp);
        itemExplanationUI.text = itemTemp.Explanation;
        itemMoneyUI.text = itemTemp.Value.ToString();
        itemOptionUI.text = ItemOptionString(itemTemp);

        itemMoney = itemTemp.Value;
    }

    public void BuyButton()
    {
        //구매버튼 클릿기 아이템을 생성해 인벤토리에 추가합니다.
        int afterMoney = PlayerManager.instance.Money-itemMoney;

        if(afterMoney<0)
        {
            return;
        }

        PlayerManager.instance.Money = afterMoney;
        Item itemTemp = DataManager.instance.ItemSetting(itemID);
  
        InventoryManager.instance.AddItem(itemTemp);
    }

    public string ItemKindString(ItemTable itemTemp)
    {
        //아이템 종류에 따라서 맞는 문구를 반환해 주는 함수입니다.
        switch ((Item.ItemType)itemTemp.ItemKind)
        {
            case Item.ItemType.EquipItem:
                if (itemTemp.DetailKind == (int)EquipItem.EquipItemType.Weapon)
                {
                    return "무기";
                }
                else if (itemTemp.DetailKind == (int)EquipItem.EquipItemType.Armor)
                {
                    return "갑옷";
                }
                else if (itemTemp.DetailKind == (int)EquipItem.EquipItemType.Shield)
                {
                    return "방패";
                }
                break;
            case Item.ItemType.UseItem:
                if (itemTemp.DetailKind == (int)UseItem.UseItemType.HpPotion)
                {
                    return "체력포션";
                }
                else if (itemTemp.DetailKind == (int)UseItem.UseItemType.MpPotion)
                {
                    return "마나포션";
                }
                else if (itemTemp.DetailKind == (int)UseItem.UseItemType.Buff)
                {
                    return "버프";
                }
                break;
            case Item.ItemType.EtcItem:
                if (itemTemp.DetailKind == (int)EtcItem.EtcItemType.Etc)
                {
                    return "기타";
                }
                else if (itemTemp.DetailKind == (int)EtcItem.EtcItemType.Ingredient)
                {
                    return "재료";
                }
                break;
            default:
                break;
        }

        return null;
    }

    public string ItemOptionString(ItemTable itemTemp)
    {
        StringBuilder temp = new StringBuilder();

        if (itemTemp.RecoveryHp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("체력회복 : + ");
            temp.Append(itemTemp.RecoveryHp);
        }
        if (itemTemp.RecoveryMp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("마나회복 : + ");
            temp.Append(itemTemp.RecoveryMp);
        }
        if (itemTemp.AddHp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("체력 : ");
            temp.Append(itemTemp.AddHp);
        }
        if (itemTemp.AddMp > 0.0f)
        {
            temp.AppendLine();
            temp.Append("마나 : ");
            temp.Append(itemTemp.AddMp);
        }
        if (itemTemp.AddAtk > 0.0f)
        {
            temp.AppendLine();
            temp.Append("공격력 : ");
            temp.Append(itemTemp.AddAtk);
        }
        if (itemTemp.AddDef > 0.0f)
        {
            temp.AppendLine();
            temp.Append("방어력 : + ");
            temp.Append(itemTemp.AddDef);
        }

        return temp.ToString();
    }
}
