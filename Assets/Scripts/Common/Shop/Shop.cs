using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private List<int> ShopItemList;
    public int nowShopItemListCount;

    private void Start()
    {
        ShopItemList = new List<int>();
        nowShopItemListCount = 0;
    }

    public void Setting(List<int> itemIDList)
    {
        nowShopItemListCount = 0;
        ShopItemList.Clear();
        itemIDList.ForEach((id) => {
            ShopItemList.Add(id);
            nowShopItemListCount++;
            });
    }

    public int shopIndexItemID(int index)
    {
        if(ShopItemList.Count<=index)
        {
            return -1;
        }
        else
        {
            return ShopItemList[index];
        }
    }
}
