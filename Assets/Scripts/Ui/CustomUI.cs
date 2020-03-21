using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomUI : MonoBehaviour
{
    public bool _IsSetting = false;
    public string nameTemp;
    public int kindIndex = 1;
    public TMP_InputField nameTextUI;
    public TextMeshProUGUI kindTextUI;

    private void Start()
    {
        nameTextUI = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        kindTextUI = transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>();

        kindTextUI.text = kindIndex.ToString();

        _IsSetting = true;
    }

    private void OnEnable()
    {
        if (PlayerManager.instance != null && _IsSetting == true)
        {
            CameraManager.instance.PauseCamaraChangeON(PlayerManager.instance.transform.gameObject);

            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalKind, PlayerManager.instance.Kind);
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalWeapon, 0);
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalArmor, 0);
            PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalShield, 0);
        }
    }

    public void BeforeButton()
    {
        if (PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalKind, kindIndex - 1) == true)
        {
            kindIndex -= 1;
            kindTextUI.text = kindIndex.ToString();
        }
    }

    public void NextButton()
    {
        if (PlayerManager.instance.CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalKind, kindIndex + 1) == true)
        {
            kindIndex += 1;
            kindTextUI.text = kindIndex.ToString();
        }
    }

    public void OkayButton()
    {
        PlayerManager.instance.Name = nameTextUI.text;
        PlayerManager.instance.Kind = kindIndex;
        PlayerManager.instance.LoadEnd();
        SceneLoader.instance.playerLocationTemp = new Vector3(88.0f, 2.0f, 42.0f);
        SceneLoader.instance.SceneLoaderStart(SceneKind.Start);

        CameraManager.instance.PauseCamaraChangeOFF(PlayerManager.instance.transform.gameObject);
        UIManager.instance.UISetting(UiKind.UiKind_NormalUI);
        UIManager.instance.UISetting(UiKind.UiKind_GameUI);
        UIManager.instance.UISetting(UiKind.UiKind_CustomUI);
    }
}
