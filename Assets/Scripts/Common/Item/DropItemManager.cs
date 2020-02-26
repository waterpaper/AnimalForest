using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemManager : MonoBehaviour
{
    private static DropItemManager m_instance;
    //드랍 아이템박스 프리팹입니다.
    public GameObject dropItemBoxPrefab;
    //드랍 아이템 박스를 저장할 위치
    public GameObject dropItemBoxPoolListLocation;
    //드랍 아이템 박스를 미리 선언해두는 리스트입니다
    public List<GameObject> dropItemBoxPoolList;
    //미리 선언할 드랍 아이템 박스의 갯수입니다.
    public int maxDropItem;

    //싱글톤 접근
    public static DropItemManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<DropItemManager>();
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

        for (int i = 0; i < maxDropItem; i++)
        {
            var obj = Instantiate<GameObject>(dropItemBoxPrefab, dropItemBoxPoolListLocation.transform);
            obj.name = dropItemBoxPrefab.name + i.ToString("00");
            //비활성화
            obj.SetActive(false);

            dropItemBoxPoolList.Add(obj);
        }
    }

    public void DropItemSetting(int enemyID, Transform enemyTransform)
    {
        EnemyDropItemTable dropItemTableTemp;
        List<Item> dropItemListTemp = new List<Item>();
        int dropMoney = 0;

        for (int i = 0; i < maxDropItem; i++)
        {
            if (dropItemBoxPoolList[i].activeSelf == true) continue;

            dropItemTableTemp = DataManager.instance.EnmeyDropItemInfo(enemyID);

            if (dropItemTableTemp != null)
            {
                dropMoney = Random.Range(dropItemTableTemp.dropMoneyMin, dropItemTableTemp.dropMoneyMax);
                float randIdx = Random.Range(0, 100);

                for (int j = 0; j < dropItemTableTemp.EnemyDropItemDataList.Count; j++)
                {
                    
                    if (randIdx < dropItemTableTemp.EnemyDropItemDataList[j].dropItemPercent)
                    {
                        Item itemTemp = ItemSetting(dropItemTableTemp.EnemyDropItemDataList[j].dropItemId);

                        if (itemTemp == null) continue;
                            dropItemListTemp.Add(itemTemp);
                    }
                    
                }

                dropItemBoxPoolList[i].GetComponent<DropItem>().ItemSetting(dropMoney, dropItemListTemp);
                dropItemBoxPoolList[i].transform.position = enemyTransform.position;

                dropItemBoxPoolList[i].SetActive(true);

                break;
            }
        }
    }

    public Item ItemSetting(int itemID)
    {
        if (itemID < 0) return null;

        ItemTable itemTableTemp = DataManager.instance.ItemInfo(itemID);

        switch ((Item.ItemType)itemTableTemp.ItemKind)
        {
            case Item.ItemType.EquipItem:
                EquipItem equipItemTemp= new EquipItem(itemID, 1, itemTableTemp.DetailKind);
                return equipItemTemp;
            case Item.ItemType.UseItem:
                UseItem useItemTemp = new UseItem(itemID, 1, itemTableTemp.DetailKind);
                return useItemTemp;
            case Item.ItemType.EtcItem:
                EtcItem EtcitemTemp = new EtcItem(itemID, 1, itemTableTemp.DetailKind);
                return EtcitemTemp;
            default:
                return null;
        }
    }

}
