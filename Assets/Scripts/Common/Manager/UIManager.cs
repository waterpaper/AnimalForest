using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UiKind { UiKind_NormalUI, UiKind_CustomUI, UiKind_InventoryUI, UIKind_EquipmentUI, UiKind_ShopUI, UiKind_OptionUI, UiKind_GameUI, UIKind_ConversationUI, UIKind_SingleConversationUI, UIKind_SingleConversationPauseUI, UiKind_LoginUI, UIKind_SignUpUI, UiKind_AllUi,UiKind_End };

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

    [Header("InputButtonString")]
    //인벤토리
    public string inventoryButtonName = "Inventory";
    //옵션
    public string optionButtonName = "Option";

    [Header("UIobject")]
    public GameObject normalUI;
    public GameObject customUI;
    public GameObject inventoryUI;
    public GameObject equipmentUI;
    public GameObject shopUI;
    public GameObject optionUI;
    public GameObject conversationUI;
    public GameObject singleConversationUI;
    public GameObject gameUI;
    public GameObject loginUI;
    public GameObject signUpUI;

    [Header("TempStatment")]
    public BossStatement nowBossStatement;
    public NpcStatment nowNpcStatment = null;

    [Header("GetItem")]
    public GameObject GetItemContentPrefabs;
    public GameObject GetItemViewerContentLoaction;

    [Header("UIStack")]
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
    public bool IsSignUpUI { get; private set; }
    public bool IsBossUI { get; private set; }


    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        FirstSetting();

        saveActiveUIStack = new Stack<int>();

        SceneSetting();
    }

    public void SceneSetting()
    {
        if (SceneLoader.instance.NowSceneKind() == SceneKind.Title)
        {

        }
        else if (SceneLoader.instance.NowSceneKind() == SceneKind.Custom)
        {
            if(!IsLoginUI)
                UISetting(UiKind.UiKind_LoginUI);
        }
        else
        {
            if(!IsGameUI)
                UISetting(UiKind.UiKind_GameUI);
            if(!IsNormalUI)
                UISetting(UiKind.UiKind_NormalUI);
        }

    }

    private void FixedUpdate()
    {
        SceneKind kindTemp = SceneLoader.instance.NowSceneKind();
        if(kindTemp != SceneKind.Title && kindTemp != SceneKind.Custom)
        {
            if (Input.GetButtonDown(inventoryButtonName))
                UISetting(UiKind.UiKind_InventoryUI);
            else if (Input.GetButtonDown(optionButtonName))
                UISetting(UiKind.UiKind_OptionUI);
        }
    }

    public void UseUI(UiKind kind)
    {
        switch (kind)
        {
            //UI를 켜주는 함정수사입니다.
            case UiKind.UiKind_NormalUI:
                normalUI.SetActive(true);
                break;
            case UiKind.UiKind_CustomUI:
                customUI.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_InventoryUI:
                inventoryUI.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_EquipmentUI:
                equipmentUI.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_ShopUI:
                shopUI.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_OptionUI:
                optionUI.SetActive(true);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_ConversationUI:
                conversationUI.SetActive(true);
                DisableUI(UiKind.UiKind_NormalUI);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UIKind_SingleConversationUI:
                singleConversationUI.SetActive(true);
                break;
            case UiKind.UIKind_SingleConversationPauseUI:
                singleConversationUI.SetActive(true);
                DisableUI(UiKind.UiKind_NormalUI);
                GameManager.instance.PlayerControlPause = true;
                break;
            case UiKind.UiKind_GameUI:
                gameUI.SetActive(true);
                break;
            case UiKind.UiKind_LoginUI:
                loginUI.SetActive(true);
                break;
            case UiKind.UIKind_SignUpUI:
                signUpUI.SetActive(true);
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
            case UiKind.UiKind_NormalUI:
                normalUI.SetActive(false);
                break;
            case UiKind.UiKind_CustomUI:
                customUI.SetActive(false);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_InventoryUI:
                inventoryUI.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_EquipmentUI:
                equipmentUI.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_ShopUI:
                shopUI.SetActive(false);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_OptionUI:
                optionUI.SetActive(false);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_ConversationUI:
                conversationUI.SetActive(false);
                UseUI(UiKind.UiKind_NormalUI);
                CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UIKind_SingleConversationUI:
                singleConversationUI.SetActive(false);
                break;
            case UiKind.UIKind_SingleConversationPauseUI:
                singleConversationUI.SetActive(false);
                if(CameraManager.instance.playMode)
                    UseUI(UiKind.UiKind_NormalUI);
                GameManager.instance.PlayerControlPause = false;
                break;
            case UiKind.UiKind_GameUI:
                gameUI.SetActive(false);
                break;
            case UiKind.UiKind_LoginUI:
                loginUI.SetActive(false);
                break;
            case UiKind.UIKind_SignUpUI:
                signUpUI.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void UISetting(UiKind kind)
    {
        //UI를 키거나 꺼주는 함수입니다.
        //UI가 켜져있을시 꺼주고 꺼져있을시 켜주게 됩니다.
        if (kind == UiKind.UiKind_NormalUI)
        {
            if (IsNormalUI)
                DisableUI(UiKind.UiKind_NormalUI);
            else
                UseUI(UiKind.UiKind_NormalUI);

            IsNormalUI = !IsNormalUI;
        }
        else if (kind == UiKind.UiKind_CustomUI)
        {
            if (IsCustomUI)
                DisableUI(UiKind.UiKind_CustomUI);
            else
                UseUI(UiKind.UiKind_CustomUI);

            IsCustomUI = !IsCustomUI;
        }
        else if (kind == UiKind.UiKind_InventoryUI)
        {
            if (IsInventoryUI)
                DisableUI(UiKind.UiKind_InventoryUI);
            else
                UseUI(UiKind.UiKind_InventoryUI);

            IsInventoryUI = !IsInventoryUI;
        }
        else if (kind == UiKind.UIKind_EquipmentUI)
        {
            if (IsEquipmentUI)
                DisableUI(UiKind.UIKind_EquipmentUI);
            else
                UseUI(UiKind.UIKind_EquipmentUI);

            IsEquipmentUI = !IsEquipmentUI;
        }
        else if (kind == UiKind.UiKind_ShopUI)
        {
            if (IsShopUI)
                DisableUI(UiKind.UiKind_ShopUI);
            else
                UseUI(UiKind.UiKind_ShopUI);

            IsShopUI = !IsShopUI;
        }
        else if (kind == UiKind.UiKind_OptionUI)
        {
            if (IsOptionUI)
                DisableUI(UiKind.UiKind_OptionUI);
            else
                UseUI(UiKind.UiKind_OptionUI);

            IsOptionUI = !IsOptionUI;
        }
        else if (kind == UiKind.UIKind_ConversationUI)
        {
            if (IsConversationUI)
                DisableUI(UiKind.UIKind_ConversationUI);
            else
                UseUI(UiKind.UIKind_ConversationUI);

            IsConversationUI = !IsConversationUI;
        }
        else if (kind == UiKind.UIKind_SingleConversationUI)
        {
            if (IsSingleConversationUI)
                DisableUI(UiKind.UIKind_SingleConversationUI);
            else
                UseUI(UiKind.UIKind_SingleConversationUI);

            IsSingleConversationUI = !IsSingleConversationUI;
        }
        else if (kind == UiKind.UIKind_SingleConversationPauseUI)
        {
            if (IsSingleConversationUI)
                DisableUI(UiKind.UIKind_SingleConversationPauseUI);
            else
                UseUI(UiKind.UIKind_SingleConversationPauseUI);

            IsSingleConversationUI = !IsSingleConversationUI;
        }
        else if (kind == UiKind.UiKind_GameUI)
        {
            if (IsGameUI)
                DisableUI(UiKind.UiKind_GameUI);
            else
                UseUI(UiKind.UiKind_GameUI);

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
        else if (kind == UiKind.UIKind_SignUpUI)
        {
            if (IsSignUpUI)
                DisableUI(UiKind.UIKind_SignUpUI);
            else
                UseUI(UiKind.UIKind_SignUpUI);

            IsSignUpUI = !IsSignUpUI;
        }
    }

    public bool IsUIActive(UiKind kind)
    {
        //해당 UI종류가 실행되어 있는 상태인지 여부를 판단합니다.
        if (kind==UiKind.UiKind_NormalUI && IsNormalUI) return true;
        if (kind == UiKind.UiKind_CustomUI && IsCustomUI) return true;
        if (kind == UiKind.UiKind_InventoryUI && IsInventoryUI) return true;
        if (kind == UiKind.UIKind_EquipmentUI && IsEquipmentUI) return true;
        if (kind == UiKind.UiKind_ShopUI && IsShopUI) return true;
        if (kind == UiKind.UiKind_OptionUI && IsOptionUI) return true;
        if (kind == UiKind.UIKind_ConversationUI && IsConversationUI) return true;
        if ((kind == UiKind.UIKind_SingleConversationPauseUI|| kind == UiKind.UIKind_SingleConversationPauseUI) && IsSingleConversationUI) return true;
        if (kind == UiKind.UiKind_GameUI && IsGameUI) return true;
        if (kind == UiKind.UiKind_LoginUI && IsLoginUI) return true;
        if (kind == UiKind.UIKind_SignUpUI && IsSignUpUI) return true;

        return false;
    }

    public void InventoryClickButton()
    {
        //인벤토리버튼을 클릭했을때 실행합니다.
        UISetting(UiKind.UiKind_InventoryUI);
        UISetting(UiKind.UIKind_EquipmentUI);
    }

    public void OptionClickButton()
    {
        //옵션메뉴를 클릭했을때 실행합니다.
        UISetting(UiKind.UiKind_OptionUI);
    }


    public void BossUISetting(BossStatement bossStatementTemp)
    {
        //맵에 보스를 찾습니다.
        if (bossStatementTemp != null)
        {
            nowBossStatement = bossStatementTemp;
            IsBossUI = true;
        }
        else
        {
            nowBossStatement = null;
            IsBossUI = false;
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

    public void FirstSetting()
    {
        IsNormalUI = false;
        IsCustomUI = false;
        IsInventoryUI = false;
        IsEquipmentUI = false;
        IsShopUI = false;
        IsOptionUI = false;
        IsConversationUI = false;
        IsSingleConversationUI = false;
        IsGameUI = false;
        IsLoginUI = false;
        IsSignUpUI = false;
        IsBossUI = false;

        normalUI.SetActive(false);
        customUI.SetActive(false);
        inventoryUI.SetActive(false);
        equipmentUI.SetActive(false);
        shopUI.SetActive(false);
        optionUI.SetActive(false);
        conversationUI.SetActive(false);
        singleConversationUI.SetActive(false);
        loginUI.SetActive(false);
        signUpUI.SetActive(false);
    }
}
