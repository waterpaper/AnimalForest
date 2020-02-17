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

            slotTemp.GetComponent<DetailItemInfomationUI>().inventoryNumber = i;
            slotTemp.GetComponent<DetailItemInfomationUI>().inventoryUI = this.gameObject;
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
        //현재 아이템을 확인하고 인벤토리 창에 이미지와 갯수를 띄워줍니다.
        int maxCount = InventoryManager.instance.MaxInventoryItemCount();
        Item itemTemp;

        for (int i = 0; i < maxCount; i++)
        {
            //인벤토리의 아이템 정보를 불러옵니다.
            itemTemp = InventoryManager.instance.GetItem(i);

            if (itemTemp == null)
            {
                slotList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slotList[i].transform.GetChild(1).GetComponent<Image>().enabled = false;
                slotList[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                continue;
            }

            if(selectNumber==i)
                slotList[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
            else
                slotList[i].transform.GetChild(0).GetComponent<Image>().enabled = false;

            //빈칸을 출력하지 않기 위해 꺼두던 아이템이미지 컴포넌트틀 켜줍니다.
            slotList[i].transform.GetChild(1).GetComponent<Image>().enabled = true;
            slotList[i].transform.GetChild(1).GetComponent<Image>().sprite = DataManager.instance.ItemIcon(itemTemp.itemInfomation.ID);

            if(itemTemp.count == 1)
            {
                slotList[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text ="";
            }
            else
            {
                slotList[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemTemp.count.ToString();
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

        int sellMoney = itemTemp.count * itemTemp.itemInfomation.Value;

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
            UIManager.instance.UISetting(UiKind.UiKind_InventoryUi);
            UIManager.instance.UISetting(UiKind.UIKind_EquipmentUi);
        }
        else if(UIManager.instance.IsShopUI ==true)
        {
            UIManager.instance.UISetting(UiKind.UiKind_InventoryUi);
            UIManager.instance.UISetting(UiKind.UiKind_ShopUi);
        }
    }
}
