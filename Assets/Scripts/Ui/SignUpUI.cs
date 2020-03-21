using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class SignUpUI : MonoBehaviour
{
    [Header("value")]
    public bool isPasswardEqual = false;

    [Header("SignupInputFelid")]
    public TMP_InputField NewIDFelid;
    public TMP_InputField NewPasswardFelid;
    public TMP_InputField NewPasswardFelidConfirm;

    [Header("AlramText")]
    public TextMeshProUGUI SignUpAlramText;

    private void Awake()
    {
        NewIDFelid = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        NewPasswardFelid = transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<TMP_InputField>();
        NewPasswardFelidConfirm = transform.GetChild(0).GetChild(0).GetChild(6).GetComponent<TMP_InputField>();
        SignUpAlramText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SignUpAlramText.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (string.Equals(NewPasswardFelid.text, NewPasswardFelidConfirm.text) == true)
            isPasswardEqual = true;
        else
            isPasswardEqual = false;
    }

    public void SignUpButton()
    {
        //패스워드가 같을때 회원가입이 가능하게 처리합니다.
        if (isPasswardEqual)
        {
            ServerManager.instance.SignUp(NewIDFelid.text, NewPasswardFelid.text, SignUpAlramText);
        }
        else
            SignUpAlramText.text = "비밀번호가 서로 다릅니다.\n 확인해주세요.";
    }

    public void ExitButton()
    {
        UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
        UIManager.instance.UISetting(UiKind.UIKind_SignUpUI);
    }
 
}
