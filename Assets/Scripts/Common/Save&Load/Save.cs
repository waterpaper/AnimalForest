using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Save
{
    public PlayerSaveData saveDataTemp;
    public PlayerState playerStateTemp;
    public Inventory inventoryTemp;
    public Equipment equipmentTemp;
    public List<Quest> nowQuestListTemp;

    public Save()
    {
        saveDataTemp = new PlayerSaveData();
        saveDataTemp.ItemList = new List<SaveItemData>();
        saveDataTemp.QuestList = new List<SaveQuestData>();
    }

    public void SaveData()
    {
        //세이브 데이터를 저장합니다.
        SaveItemData saveItemData;
        SaveQuestData saveQuestData;

        //미리 데이터를 불러옵니다.
        playerStateTemp = PlayerManager.instance.GetPlayerState();
        inventoryTemp = InventoryManager.instance.GetInventory();
        equipmentTemp = InventoryManager.instance.GetEquipment();
        nowQuestListTemp = QuestManager.instance.GetQuest();
        
        saveDataTemp.ItemList.Clear();
        saveDataTemp.QuestList.Clear();

        //세이브 데이터에 대입합니다.
        //플레이어 데이터를 세팅합니다.
        saveDataTemp.ID = playerStateTemp.id;
        saveDataTemp.Name = playerStateTemp.name;
        saveDataTemp.Kind = playerStateTemp.kind;
        saveDataTemp.Money = playerStateTemp.money;
        saveDataTemp.Level = playerStateTemp.level;
        saveDataTemp.Exp = playerStateTemp.exp;
        saveDataTemp.Hp = playerStateTemp.hp;
        saveDataTemp.HpMax = playerStateTemp.hpMax;
        saveDataTemp.Mp = playerStateTemp.mp;
        saveDataTemp.MpMax = playerStateTemp.mpMax;
        saveDataTemp.Atk = playerStateTemp.atk;
        saveDataTemp.Def = playerStateTemp.def;

        //위치정보를 세팅합니다.
        saveDataTemp.MapNumber = (int)SceneLoader.instance.NowSceneKind();
        saveDataTemp.MapPosition = PlayerManager.instance.transform.position;

        //장비정보를 세팅합니다.
        if (equipmentTemp.weapon != null)
            saveDataTemp.EquipWeaponItem = equipmentTemp.weapon.itemInfomation.ID;
        else
            saveDataTemp.EquipWeaponItem = -1;

        if (equipmentTemp.armor != null)
            saveDataTemp.EquipArmorItem = equipmentTemp.armor.itemInfomation.ID;
        else
            saveDataTemp.EquipArmorItem = -1;

        if (equipmentTemp.shield != null)
            saveDataTemp.EquipShieldItem = equipmentTemp.shield.itemInfomation.ID;
        else
            saveDataTemp.EquipShieldItem = -1;

        saveDataTemp.EquipHpPotion = equipmentTemp.HpPotionNum;
        saveDataTemp.EquipMpPotion = equipmentTemp.MpPotionNum;

        //인벤토리 정보를 세팅합니다.
        for (int i = 0; i < inventoryTemp.myInventory.Count; i++)
        {
            if (inventoryTemp.myInventory[i] != null)
            {
                saveItemData.ItemID = inventoryTemp.myInventory[i].itemInfomation.ID;
                saveItemData.ItemCount = inventoryTemp.myInventory[i].count;
                saveItemData.InventoryNum = i;

                saveDataTemp.ItemList.Add(saveItemData);
            }
        }

        //퀘스트 정보를 세팅합니다.
        for (int i = 0; i < nowQuestListTemp.Count; i++)
        {
            if (nowQuestListTemp[i] != null)
            {
                saveQuestData.QuestID = nowQuestListTemp[i].QuestID;
                saveQuestData.TargetNowCount = nowQuestListTemp[i].TargetObject_NowCount;

                saveDataTemp.QuestList.Add(saveQuestData);
            }
        }


        //클리어 정보를 세팅합니다.
        //string으로 각 퀘스트를 0,1로 구분해 데이터를 만들어줍니다.
        saveDataTemp.ClearQuest = "";
        
        for (int i = 0, clearQuestCountTemp = 0; i < 100; i++)
        {
            if (clearQuestCountTemp < playerStateTemp.clearQuestList.Count)
            {
                if (playerStateTemp.clearQuestList[clearQuestCountTemp] == i)
                {
                    clearQuestCountTemp++;
                    saveDataTemp.ClearQuest = string.Format("{00}1",saveDataTemp.ClearQuest);
                    continue;
                }
            }
            saveDataTemp.ClearQuest = string.Format("{00}0", saveDataTemp.ClearQuest);
        }

        //string으로 각 이벤트를 0,1로 구분해 데이터를 만들어줍니다.
        saveDataTemp.ClearEvent = "";

        for (int i = 0, clearEventCountTemp = 0; i < 100; i++)
        {
            if (clearEventCountTemp < playerStateTemp.clearEventList.Count)
            {
                if (playerStateTemp.clearEventList[clearEventCountTemp] == i)
                {
                    clearEventCountTemp++;
                    saveDataTemp.ClearEvent = string.Format("{00}1", saveDataTemp.ClearEvent);
                    continue;
                }
            }
            saveDataTemp.ClearEvent = string.Format("{00}0", saveDataTemp.ClearEvent);
        }


        var data = ObjectToJson(saveDataTemp);
        ServerManager.instance.Save(saveDataTemp.ID, data);
        CreateJsonFile(Application.dataPath, string.Format("Resources/Player_{00}", saveDataTemp.ID), data);
    }

    //json을 저장하는 함수
    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }
}