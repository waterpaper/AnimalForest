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

    public Save()
    {
        saveDataTemp = new PlayerSaveData();
        saveDataTemp.ClearEventList = new List<int>();
        saveDataTemp.ItemList = new List<SaveItemData>();
    }

    public void SaveData()
    {
        //세이브 데이터를 저장합니다.
        //미리 데이터를 불러옵니다.
        SaveItemData itemTemp;
        playerStateTemp = PlayerManager.instance.GetPlayerState();
        inventoryTemp = InventoryManager.instance.GetInventory();
        equipmentTemp = InventoryManager.instance.GetEquipment();

        saveDataTemp.ClearEventList.Clear();
        saveDataTemp.ItemList.Clear();

        //세이브 데이터에 대입합니다.
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

        saveDataTemp.MapNumber = (int)SceneLoader.instance.NowSceneKind();
        Vector3 positionTemp = PlayerManager.instance.transform.position;
        saveDataTemp.MapPositionX = positionTemp.x;
        saveDataTemp.MapPositionY = positionTemp.y;
        saveDataTemp.MapPositionZ = positionTemp.z;
        

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

        playerStateTemp.clearEventList.ForEach((temp) => { saveDataTemp.ClearEventList.Add(temp); });

        for (int i = 0; i < inventoryTemp.myInventory.Count; i++)
        {
            if (inventoryTemp.myInventory[i] != null)
            {
                itemTemp.ItemID = inventoryTemp.myInventory[i].itemInfomation.ID;
                itemTemp.ItemCount = inventoryTemp.myInventory[i].count;
            }
            else
            {
                itemTemp.ItemID = -1;
                itemTemp.ItemCount = 0;
            }

            saveDataTemp.ItemList.Add(itemTemp);
        }

        var data = ObjectToJson(saveDataTemp);
        ServerManager.instance.Save(saveDataTemp.ID, data);
        CreateJsonFile(Application.dataPath, string.Format("Resources/{00}", saveDataTemp.ID), data);
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