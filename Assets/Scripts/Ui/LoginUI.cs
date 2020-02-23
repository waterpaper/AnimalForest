using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class LoginUI : MonoBehaviour
{
    [Header("InputFelid")]
    public TMP_InputField IDFelid;
    public TMP_InputField PasswardFelid;

    [Header("AlramText")]
    public TextMeshProUGUI LoginAlramText;
    

    void Awake()
    {
        IDFelid = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        PasswardFelid = transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<TMP_InputField>();
        LoginAlramText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        LoginAlramText.gameObject.SetActive(false);
    }

    public void LoginButton()
    {
        ServerManager.instance.Login(IDFelid.text, PasswardFelid.text, LoginAlramText);
    }

    public void SignUpButton()
    {
        UIManager.instance.UISetting(UiKind.UIKind_SignUpUI);
        UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
    }
}
