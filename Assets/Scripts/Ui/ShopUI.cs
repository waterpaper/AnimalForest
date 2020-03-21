using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    private Shop _shop;
    public int createShopSlotCount = 30;
    public GameObject ShopSlotPrefab;
    public GameObject ShopSlotLocation;
    public List<ShopSlotUI> shopSlotList;

    public void Awake()
    {
        _shop = GetComponent<Shop>();

        for (int i = 0; i < createShopSlotCount; i++)
        {
            var slotTemp = Instantiate(ShopSlotPrefab, ShopSlotLocation.transform).GetComponent<ShopSlotUI>();
            slotTemp.gameObject.SetActive(false);
            shopSlotList.Add(slotTemp);
        }
    }

    private void OnEnable()
    {
        NpcStatment nowNpcStatmentTemp = UIManager.instance.nowNpcStatment;

        if (nowNpcStatmentTemp != null)
        {
            ShopTable shopTemp = DataManager.instance.GetTableData<ShopTable>(TableDataKind.TableDataKind_Shop, nowNpcStatmentTemp.shopKind);
            _shop.Setting(shopTemp.ShopItemID);
            ActiveSlot();
        }
    }

    public void OnDisable()
    {
        disableSlot();
    }

    private void ActiveSlot()
    {
        int maxCount = _shop.nowShopItemListCount;

        for (int i = 0; i < maxCount; i++)
        {
            int itemIndexTemp = _shop.shopIndexItemID(i);

            if (itemIndexTemp == -1) break;

            shopSlotList[i].Setting(itemIndexTemp);
            shopSlotList[i].gameObject.SetActive(true);
        }
    }

    private void disableSlot()
    {
        int maxCount = _shop.nowShopItemListCount;

        for (int i = 0; i < maxCount; i++)
        {
            shopSlotList[i].gameObject.SetActive(false);
        }
    }
}
