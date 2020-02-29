using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> myInventory;
    public int maxInventoryCount = 21;
    public int nowInventoryCount = 0;

    private void Awake()
    {
        myInventory = new List<Item>();

        for (int i = 0; i < maxInventoryCount; i++)
        {
            myInventory.Add(null);
        }
    }

    //인벤토리 안에 있는 아이템의 정보를 반환합니다.
    public Item GetInventoryItem(int num)
    {
        if (myInventory[num] == null)
            return null;

        return myInventory[num];
    }

    //인벤토리 안에 해당 아이디의 아이템이 있으면 반환하고 아니면 null을 리턴합니다.
    public Item GetInventoryItem_SearchID(int id)
    {
        for (int i = 0, count = 0; i < maxInventoryCount; i++)
        {
            if (count == nowInventoryCount)
                break;

            if (myInventory[i] == null)
                continue;

            if (myInventory[i].itemInfomation.ID == id)
                return myInventory[i];
        }
        return null;
    }

    //인벤토리에 아이템을 추가합니다.
    public bool AddInventroyItem(Item itemTemp)
    {
        //먼저 현제 인벤토리에 존재하는 아이템과 동일한 아이템이 있는지 확인후 존재하면 갯수를 늘려줍니다.
        for (int i = 0; i < myInventory.Count; i++)
        {
            if (myInventory[i] == null) continue;

            if (myInventory[i].itemType == itemTemp.itemType && myInventory[i].itemInfomation.ID == itemTemp.itemInfomation.ID)
            {
                myInventory[i].count += itemTemp.count;
                return true;
            }
        }

        //겹치는 아이템이 아닐시 아이템을 추가합니다.
        if (maxInventoryCount > nowInventoryCount)
        {
            for (int i = 0, postionTemp = 0; i < myInventory.Count; i++, postionTemp++)
            {
                if (myInventory[i] == null)
                {
                    myInventory[postionTemp] = itemTemp;
                    nowInventoryCount++;
                    break;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    //인벤토리 해당 위치에 아이템을 추가합니다.
    public bool AddInventroyItem(Item itemTemp, int inventoryNumber)
    {
        //해당 인벤토리에 아이템이 존재하면 추가하지 않습니다.
        if (myInventory[inventoryNumber] != null) return false;

        myInventory[inventoryNumber] = itemTemp;


        return true;
    }

    public bool DeleteInventoryItem(int num)
    {
        if (myInventory[num] == null)
            return false;

        myInventory[num] = null;
        nowInventoryCount--;
        return true;
    }

    public bool SettingItemCount(int num, int count)
    {
        if (myInventory[num] == null)
            return false;

        myInventory[num].count = count;

        return true;
    }
}
