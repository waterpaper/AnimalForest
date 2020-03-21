using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public int selectNumber;
    public GameObject slotPrefab;
    public GameObject slotCreateLocation;
    public List<GameObject> slotList;
    
    public GameObject equipButton;
    public GameObject sellButton;
    public GameObject usingBUtton;
    
    void Awake()
    {
        equipButton = gameObject.transform.GetChild(0).GetChild(1).gameObject;
        sellButton = gameObject.transform.GetChild(0).GetChild(2).gameObject;
        usingBUtton = gameObject.transform.GetChild(0).GetChild(3).gameObject;

        slotList = new List<GameObject>();

        for (int i = 0; i < 21; i++)
        {
            var slotTemp = Instantiate<GameObject>(slotPrefab, slotCreateLocation.transform);

            slotTemp.GetComponent<DetailItemInfomationUI>().FirstSetting(this, i);
            slotTemp.name = slotTemp.name + i.ToString("00");
            slotList.Add(slotTemp);
        }
    }

    private void OnEnable()
    {
        if(UIManager.instance.IsShopUI == true)
        {
            equipButton.SetActive(false);
            sellButton.SetActive(true);
            usingBUtton.SetActive(false);
        }
        else
        {
            equipButton.SetActive(true);
            sellButton.SetActive(false);
            usingBUtton.SetActive(true);
        }

        selectNumber = -1;
    }

    private void OnDisable()
    {
        //인벤토리를 닫을시 아이템이미지 컴포넌트를 종료해 빈 칸이 나중에 뜨지 않도록 합니다.
        int maxCount = InventoryManager.instance.NowInventoryItemCount();

        for (int i = 0; i < maxCount; i++)
        {
            slotList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            slotList[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        Image selectItemImage;
        Image itemImage;
        TextMeshProUGUI itemNumText; 

        //현재 아이템을 확인하고 인벤토리 창에 이미지와 갯수를 띄워줍니다.
        int maxCount = InventoryManager.instance.MaxInventoryItemCount();
        Item itemTemp;

        for (int i = 0; i < maxCount; i++)
        {
            //컴포넌트를 연결합니다.
            selectItemImage = slotList[i].transform.GetChild(0).GetComponent<Image>();
            itemImage = slotList[i].transform.GetChild(1).GetComponent<Image>();
            itemNumText = slotList[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            
            //인벤토리의 아이템 정보를 불러옵니다.
            itemTemp = InventoryManager.instance.GetItem(i);

            if (itemTemp == null)
            {
                selectItemImage.enabled = false;
                itemImage.enabled = false;
                itemNumText.text = "";
                continue;
            }

            if(selectNumber==i)
                selectItemImage.enabled = true;
            else
                selectItemImage.enabled = false;

            //빈칸을 출력하지 않기 위해 꺼두던 아이템이미지 컴포넌트틀 켜줍니다.
            itemImage.enabled = true;
            itemImage.sprite = DataManager.instance.GetIconData(IconDataKind.IconDataKind_Item ,itemTemp.itemInfomation.ID);

            if(itemTemp.count == 1)
            {
                itemNumText.text ="";
            }
            else
            {
                itemNumText.text = itemTemp.count.ToString();
            }
        }
    }

    public void EquipButton()
    {
        if (selectNumber == -1) return;

        InventoryManager.instance.EquipmentingItem(selectNumber);
    }

    public void SellBUtton()
    {
        if (selectNumber == -1) return;

        Item itemTemp = InventoryManager.instance.GetItem(selectNumber);

        int sellMoney =itemTemp.count * (int)(itemTemp.itemInfomation.Value * 0.5f);

        InventoryManager.instance.DeleteItem(selectNumber, itemTemp.count);

        PlayerManager.instance.Money += sellMoney;
    }

    public void UsingButton()
    {
        if (selectNumber == -1) return;

        InventoryManager.instance.UseingItem(selectNumber);
    }


    public void ExitButton()
    {
        if (UIManager.instance.IsEquipmentUI == true)
        {
            UIManager.instance.UISetting(UiKind.UiKind_InventoryUI);
            UIManager.instance.UISetting(UiKind.UIKind_EquipmentUI);
        }
        else if(UIManager.instance.IsShopUI ==true)
        {
            UIManager.instance.UISetting(UiKind.UiKind_InventoryUI);
            UIManager.instance.UISetting(UiKind.UiKind_ShopUI);
        }
    }
}
