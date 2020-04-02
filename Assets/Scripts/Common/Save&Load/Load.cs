using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load
{
    //정보를 불러오는 로드를 처리하는 클래스입니다.
    public static PlayerSaveData saveDataTemp;
    public ServerDelegate<string> loadDele = new ServerDelegate<string>(LoadData);

    public Load()
    {
        saveDataTemp = new PlayerSaveData();
        saveDataTemp.ItemList = new List<SaveItemData>();
        saveDataTemp.QuestList = new List<SaveQuestData>();
    }

    public void LoadConnection(string id)
    {
        //서버에 로드 연결을 시작합니다.
        ServerManager.instance.Load(id, loadDele);
    }

    public static void LoadData(string id, string data)
    {
        Debug.Log("log : \n"+data);
        //세이브 데이터를 불러와 사용할수 있게 처리합니다.
        saveDataTemp = JsonUtility.FromJson<PlayerSaveData>(data);

        //세이브 데이터를 설정합니다.
        //플레이어 데이터를 설정합니다.
        PlayerManager.instance.LoadPlayerSetting(saveDataTemp);
        
        //인벤토리 및 장비 데이터를 설정합니다.
        InventoryManager.instance.LoadInventoryAndEquipment(saveDataTemp);

        //퀘스트 정보를 설정합니다.
        QuestManager.instance.LoadQuest(saveDataTemp);

        //데이터 로드가 끝나면 씬과 UI를 시작위치로 바꿔줍니다.
        PlayerManager.instance.LoadEnd();
        SceneLoader.instance.SceneLoaderStart(SceneKind.Town);

        CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
        UIManager.instance.UISetting(UiKind.UiKind_NormalUI);
        UIManager.instance.UISetting(UiKind.UiKind_GameUI);
        UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
    }
}
