using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    //상점정보를 가지고잇는 클래스입니다.
    public int nowShopItemListCount;
    private List<int> _shopItemList;

    private void Start()
    {
        _shopItemList = new List<int>();
        nowShopItemListCount = 0;
    }

    public void Setting(List<int> itemIDList)
    {
        nowShopItemListCount = 0;
        _shopItemList.Clear();
        itemIDList.ForEach((id) => {
            _shopItemList.Add(id);
            nowShopItemListCount++;
            });
    }

    public int shopIndexItemID(int index)
    {
        if(_shopItemList.Count<=index)
            return -1;

        else
            return _shopItemList[index];
    }
}
