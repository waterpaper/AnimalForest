using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    [Header("InputFelid")]
    public TMP_InputField IDFelid;
    public TMP_InputField PasswardFelid;

    [Header("SignupInputFelid")]
    public TMP_InputField NewIDFelid;
    public TMP_InputField NewPasswardFelid;

    [Header("Server")]
    public string url = "127.0.0.1:3306";

    void Awake()
    {
        IDFelid = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<TMP_InputField>();
        PasswardFelid = transform.GetChild(0).GetChild(0).GetChild(4).GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoginButton()
    {
        StartCoroutine(LoginCo());
    }

    public void SignupButton()
    {

    }

    IEnumerator LoginCo()
    {
        List<IMultipartFormSection> loginForm = new List<IMultipartFormSection>();

        loginForm.Add(new MultipartFormDataSection("ID", IDFelid.text));
        loginForm.Add(new MultipartFormDataSection("Passward", PasswardFelid.text));

        UnityWebRequest WebRequest = UnityWebRequest.Post(url, loginForm);

        yield return WebRequest.SendWebRequest();

        if(WebRequest != null)
        {
            string result = WebRequest.downloadHandler.text;

            Debug.Log(result);
        }
    }
}
