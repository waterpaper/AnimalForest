using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UiKind { UiKind_NormalUi, UiKind_CustomUi, UiKind_InventoryUi, UIKind_EquipmentUi, UiKind_ShopUi, UiKind_OptionUi, UiKind_GameUi, UIKind_ConversationUi, UIKind_SingleConversationUi, UIKind_SingleConversationPauseUi, UiKind_LoginUI, UiKind_AllUi,UiKind_End };

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance;

    //싱글톤 접근
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    //인벤토리
    public string inventoryButtonName = "Inventory";
    //옵션
    public string optionButtonName = "Option";

    public GameObject normalUi;
    public GameObject customUi;
    public GameObject inventoryUi;
    public GameObject equipmentUi;
    public GameObject shopUi;
    public GameObject optionUi;
    public GameObject conversationUi;
    public GameObject singleConversationUi;
    public GameObject gameUi;
    public GameObject loginUi;

    public BossStatment nowBossStatement;
    public GameObject GetItemContentPrefabs;
    public GameObject GetItemViewerContentLoaction;
    public Stack<int> saveActiveUIStack;

    public bool IsNormalUI { get; private set; }
    public bool IsCustomUI { get; private set; }
    public bool IsInventoryUI { get; private set; }
    public bool IsEquipmentUI { get; private set; }
    public bool IsShopUI { get; private set; }
    public bool IsOptionUI { get; private set; }
    public bool IsConversationUI { get; private set; }
    public bool IsSingleConversationUI { get; private set; }
    public bool IsGameUI { get; private set; }
    public bool IsLoginUI { get; private set; }
    public bool IsBossUI { get; private set; }

    public NpcStatment nowNpcStatment = null;

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        IsNormalUI = true;
        IsCustomUI = true;
        IsInventoryUI = true;
        IsEquipmentUI = true;
        IsShopUI = true;
        IsOptionUI = true;
        IsConversationUI = true;
        IsSingleConversationUI = true;
        IsGameUI = true;
        IsLoginUI = true;
        IsBossUI = false;
    }
    private void Start()
    {
        UISetting(UiKind.UiKind_CustomUi);
        UISetting(UiKind.UiKind_InventoryUi);
        UISetting(UiKind.UIKind_EquipmentUi);
        UISetting(UiKind.UiKind_ShopUi);
        UISetting(UiKind.UiKind_OptionUi);
        UISetting(UiKind.UIKind_ConversationUi);
        UISetting(UiKind.UIKind_SingleConversationUi);
        UISetting(UiKind.UiKind_GameUi);
        UISetting(UiKind.UiKind_NormalUi);
        UISetting(UiKind.UiKind_LoginUI);

        if (SceneLoader.instance.NowSceneKind() == SceneKind.Custom)
        {
            UISetting(UiKind.UiKind_CustomUi);
            UISetting(UiKind.UiKind_LoginUI);
        }
        else
        {
            UISetting(UiKind.UiKind_GameUi);
            UISetting(UiKind.UiKind_NormalUi);
        }

        saveActiveUIStack = new Stack<int>();
    }
    private void FixedUpdate()
    {
        if (Input.GetButtonDown(inventoryButtonName))
            UISetting(UiKind.UiKind_InventoryUi);
        else if (Input.GetButtonDown(optionButtonName))
            UISetting(UiKind.UiKind_OptionUi);
    }

    public void UseUI(UiKind kind)
    {
        switch (kind)
        {
            //UI를 켜주는 함수입니다.
            case UiKind.UiKind_NormalUi:
                normalUi.SetActive(true);
                break;
            case UiKind.UiKind_CustomUi:
                customUi.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_InventoryUi:
                inventoryUi.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_EquipmentUi:
                equipmentUi.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_ShopUi:
                shopUi.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_OptionUi:
                optionUi.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_ConversationUi:
                conversationUi.SetActive(true);
                DisableUI(UiKind.UiKind_NormalUi);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_SingleConversationUi:
                singleConversationUi.SetActive(true);
                break;
            case UiKind.UIKind_SingleConversationPauseUi:
                singleConversationUi.SetActive(true);
                DisableUI(UiKind.UiKind_NormalUi);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_GameUi:
                gameUi.SetActive(true);
                break;
            case UiKind.UiKind_LoginUI:
                loginUi.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void DisableUI(UiKind kind)
    {
        //UI를 꺼주는 함수입니다.
        switch (kind)
        {
            case UiKind.UiKind_NormalUi:
                normalUi.SetActive(false);
                break;
            case UiKind.UiKind_CustomUi:
                customUi.SetActive(false);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_InventoryUi:
                inventoryUi.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_EquipmentUi:
                equipmentUi.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_ShopUi:
                shopUi.SetActive(false);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_OptionUi:
                optionUi.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_ConversationUi:
                conversationUi.SetActive(false);
                UseUI(UiKind.UiKind_NormalUi);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_SingleConversationUi:
                singleConversationUi.SetActive(false);
                break;
            case UiKind.UIKind_SingleConversationPauseUi:
                singleConversationUi.SetActive(false);
                if(CameraManager.instance.playMode)
                    UseUI(UiKind.UiKind_NormalUi);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_GameUi:
                gameUi.SetActive(false);
                break;
            case UiKind.UiKind_LoginUI:
                loginUi.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void UISetting(UiKind kind)
    {
        //UI를 키거나 꺼주는 함수입니다.
        //UI가 켜져있을시 꺼주고 꺼져있을시 켜주게 됩니다.
        if (kind == UiKind.UiKind_NormalUi)
        {
            if (IsNormalUI)
                DisableUI(UiKind.UiKind_NormalUi);
            else
                UseUI(UiKind.UiKind_NormalUi);

            IsNormalUI = !IsNormalUI;
        }
        else if (kind == UiKind.UiKind_CustomUi)
        {
            if (IsCustomUI)
                DisableUI(UiKind.UiKind_CustomUi);
            else
                UseUI(UiKind.UiKind_CustomUi);

            IsCustomUI = !IsCustomUI;
        }
        else if (kind == UiKind.UiKind_InventoryUi)
        {
            if (IsInventoryUI)
                DisableUI(UiKind.UiKind_InventoryUi);
            else
                UseUI(UiKind.UiKind_InventoryUi);

            IsInventoryUI = !IsInventoryUI;
        }
        else if (kind == UiKind.UIKind_EquipmentUi)
        {
            if (IsEquipmentUI)
                DisableUI(UiKind.UIKind_EquipmentUi);
            else
                UseUI(UiKind.UIKind_EquipmentUi);

            IsEquipmentUI = !IsEquipmentUI;
        }
        else if (kind == UiKind.UiKind_ShopUi)
        {
            if (IsShopUI)
                DisableUI(UiKind.UiKind_ShopUi);
            else
                UseUI(UiKind.UiKind_ShopUi);

            IsShopUI = !IsShopUI;
        }
        else if (kind == UiKind.UiKind_OptionUi)
        {
            if (IsOptionUI)
                DisableUI(UiKind.UiKind_OptionUi);
            else
                UseUI(UiKind.UiKind_OptionUi);

            IsOptionUI = !IsOptionUI;
        }
        else if (kind == UiKind.UIKind_ConversationUi)
        {
            if (IsConversationUI)
                DisableUI(UiKind.UIKind_ConversationUi);
            else
                UseUI(UiKind.UIKind_ConversationUi);

            IsConversationUI = !IsConversationUI;
        }
        else if (kind == UiKind.UIKind_SingleConversationUi)
        {
            if (IsSingleConversationUI)
                DisableUI(UiKind.UIKind_SingleConversationUi);
            else
                UseUI(UiKind.UIKind_SingleConversationUi);

            IsSingleConversationUI = !IsSingleConversationUI;
        }
        else if (kind == UiKind.UIKind_SingleConversationPauseUi)
        {
            if (IsSingleConversationUI)
                DisableUI(UiKind.UIKind_SingleConversationPauseUi);
            else
                UseUI(UiKind.UIKind_SingleConversationPauseUi);

            IsSingleConversationUI = !IsSingleConversationUI;
        }
        else if (kind == UiKind.UiKind_GameUi)
        {
            if (IsGameUI)
                DisableUI(UiKind.UiKind_GameUi);
            else
                UseUI(UiKind.UiKind_GameUi);

            IsGameUI = !IsGameUI;
        }
        else if (kind == UiKind.UiKind_LoginUI)
        {
            if (IsLoginUI)
                DisableUI(UiKind.UiKind_LoginUI);
            else
                UseUI(UiKind.UiKind_LoginUI);

            IsLoginUI = !IsLoginUI;
        }
    }

    public bool IsUIActive(UiKind kind)
    {
        //해당 UI종류가 실행되어 있는 상태인지 여부를 판단합니다.

        if (kind==UiKind.UiKind_NormalUi && IsNormalUI) return true;
        if (kind == UiKind.UiKind_CustomUi && IsCustomUI) return true;
        if (kind == UiKind.UiKind_InventoryUi && IsInventoryUI) return true;
        if (kind == UiKind.UIKind_EquipmentUi && IsEquipmentUI) return true;
        if (kind == UiKind.UiKind_ShopUi && IsShopUI) return true;
        if (kind == UiKind.UiKind_OptionUi && IsOptionUI) return true;
        if (kind == UiKind.UIKind_ConversationUi && IsConversationUI) return true;
        if ((kind == UiKind.UIKind_SingleConversationPauseUi|| kind == UiKind.UIKind_SingleConversationPauseUi) && IsSingleConversationUI) return true;
        if (kind == UiKind.UiKind_GameUi && IsGameUI) return true;
        if (kind == UiKind.UiKind_LoginUI && IsLoginUI) return true;

        return false;
    }

    public void InventoryClickButton()
    {
        //인벤토리버튼을 클릭했을때 실행합니다.
        UISetting(UiKind.UiKind_InventoryUi);
        UISetting(UiKind.UIKind_EquipmentUi);
    }

    public void OptionClickButton()
    {
        //옵션메뉴를 클릭했을때 실행합니다.
        UISetting(UiKind.UiKind_OptionUi);
    }


    public void BossUISetting()
    {
        //맵에 보스를 찾습니다.
        var BossObject = GameObject.FindGameObjectWithTag("Boss");

        if (BossObject != null)
        {
            IsBossUI = true;
            nowBossStatement = BossObject.GetComponent<BossStatment>();
        }
        else
        {
            IsBossUI = false;
            nowBossStatement = null;
        }
    }

    public void GetItemUIAdd(string itemText, int count)
    {
        //아이템 추가 UI를 생성해줍니다.
        if (GetItemViewerContentLoaction.activeSelf == true)
        {
            var itemUITemp = Instantiate<GameObject>(GetItemContentPrefabs, GetItemViewerContentLoaction.transform);

            itemUITemp.GetComponent<GetItemContents>().Setting(itemText, count);
        }
    }

    public void SaveToDisable_ActiveUI()
    {
        //실행되어있던 UI를 저장하고 disable시킵니다.
        for (int i = 0; i < (int)UiKind.UiKind_End; i++)
        {
            if (IsUIActive((UiKind)i))
            {
                saveActiveUIStack.Push(i);
                UISetting((UiKind)i);
            }
        }
    }

    public void Load_ActiveUI()
    {
        //저장되어있던 UI를 다시 모두 실행시킵니다.
        while(saveActiveUIStack.Count > 0)
        {
            UISetting((UiKind)saveActiveUIStack.Pop());
        }
    }
}
