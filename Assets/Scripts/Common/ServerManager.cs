using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public delegate void ServerDelegate<T> (T id,T data);

enum ServerConnectionKind
{
    ServerConnectionKind_Login,
    ServerConnectionKind_SignUp,
    ServerConnectionKind_End
}

public class ServerManager : MonoBehaviour
{
    [Header("Server")]
    public string url = "http://127.0.0.1:80/";
    public string loginConnectionName = "login";
    public string signupConnectionName = "SignUp";
    public string saveConnectionName = "Save";
    public string loadConnectionName = "Load";
    public TextMeshProUGUI resultTextUI;

    private static ServerManager m_instance;

    //싱글톤 접근
    public static ServerManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ServerManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public static void Function<T>(T id, T data, ServerDelegate<T> dele)
    {
        dele(id, data);
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void Login(string id, string passward, TextMeshProUGUI textUI = null)
    {
        if (textUI != null)
            resultTextUI = textUI;
        else
            resultTextUI = null;

        //로그인을 수행합니다.
        StartCoroutine(PostLoginConnection(id, passward));
    }

    public void SignUp(string id, string passward, TextMeshProUGUI textUI = null)
    {
        if (textUI != null)
            resultTextUI = textUI;
        else
            resultTextUI = null;

        //회원가입을 수행합니다.
        StartCoroutine(PostSignUpConnection(id, passward));
    }

    public void Save(string id, string saveData)
    {
        //세이브를 수행합니다.
        StartCoroutine(PostSaveConnection(id, saveData));
    }

    public void Load(string id, ServerDelegate<string> dele)
    {
        StartCoroutine(PostLoadConnection(id, dele));
        StartCoroutine(SaveCycle());
    }

    IEnumerator SaveCycle()
    {
        while (true)
        {
            if ((int)SceneLoader.instance.NowSceneKind() > (int)SceneKind.Start)
            {
                PlayerManager.instance.Save();
            }

            yield return new WaitForSeconds(3.0f);

            if (GameManager.instance == null)
                break;
        }
    }

    IEnumerator PostLoginConnection(string id, string passward)
    {
        List<IMultipartFormSection> loginForm = new List<IMultipartFormSection>();

        loginForm.Add(new MultipartFormDataSection("ID", id));
        loginForm.Add(new MultipartFormDataSection("Passward", passward));


        UnityWebRequest WebRequest = UnityWebRequest.Post(string.Format("{00}{01}.php", url, loginConnectionName), loginForm);

        yield return WebRequest.SendWebRequest();

        if (WebRequest != null)
        {
            string result = WebRequest.downloadHandler.text;

            if (string.Equals(result, "Success"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "로그인에 성공하였습니다.\n게임을 접속합니다.";
                    resultTextUI.gameObject.SetActive(true);

                    PlayerManager.instance.Id = id;
                    yield return new WaitForSeconds(2.0f);
                    PlayerManager.instance.Load(id);
                }
            }
            else if (string.Equals(result, "ID") || string.Equals(result, "Passward"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "아이디나 비밀번호가 틀렸습니다.\n확인해 주세요";
                    resultTextUI.gameObject.SetActive(true);
                }
            }
        }
    }

    IEnumerator PostSignUpConnection(string id, string passward)
    {
        List<IMultipartFormSection> signForm = new List<IMultipartFormSection>();

        signForm.Add(new MultipartFormDataSection("NewID", id));
        signForm.Add(new MultipartFormDataSection("NewPassward", passward));

        UnityWebRequest WebRequest = UnityWebRequest.Post(string.Format("{00}{01}.php", url, signupConnectionName), signForm);

        yield return WebRequest.SendWebRequest();

        if (WebRequest != null)
        {
            string result = WebRequest.downloadHandler.text;

            if (string.Equals(result, "Create"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "회원가입에 성공했습니다.";
                    resultTextUI.gameObject.SetActive(true);
                    yield return new WaitForSeconds(2.0f);

                    UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
                    UIManager.instance.UISetting(UiKind.UIKind_SignUpUI);
                }
            }
            else if (string.Equals(result, "Exist"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "아이디가 존재합니다.";
                    resultTextUI.gameObject.SetActive(true);
                }
            }
            else if (string.Equals(result, "Error"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "아이디 생성 오류입니다.";
                    resultTextUI.gameObject.SetActive(true);
                }
            }

            Debug.Log(result);
        }

    }

    IEnumerator PostSaveConnection(string id, string saveData)
    {
        List<IMultipartFormSection> saveForm = new List<IMultipartFormSection>();

        saveForm.Add(new MultipartFormDataSection("ID", id));
        saveForm.Add(new MultipartFormDataSection("SaveData", saveData));

        UnityWebRequest WebRequest = UnityWebRequest.Post(string.Format("{00}{01}.php", url, saveConnectionName), saveForm);

        yield return WebRequest.SendWebRequest();

        if (WebRequest != null)
        {
            string result = WebRequest.downloadHandler.text;

            if (string.Equals(result, "Success"))
            {
               
            }
            else if (string.Equals(result, "Error"))
            {
                
            }
            Debug.Log(result);

        }
    }

    IEnumerator PostLoadConnection(string id, ServerDelegate<string> dele)
    {
        List<IMultipartFormSection> loadForm = new List<IMultipartFormSection>();

        loadForm.Add(new MultipartFormDataSection("ID", id));

        UnityWebRequest WebRequest = UnityWebRequest.Post(string.Format("{00}{01}.php", url, loadConnectionName), loadForm);
        yield return WebRequest.SendWebRequest();

        if(WebRequest != null)
        {
            string result =  WebRequest.downloadHandler.text;

            if (string.Equals(result, "Error"))
            {
                if (resultTextUI != null)
                {
                    resultTextUI.text = "서버 오류입니다.";
                    resultTextUI.gameObject.SetActive(true);
                }
            }
            else if(string.Equals(result, "NotExisted"))
            {
                UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
                UIManager.instance.UISetting(UiKind.UiKind_CustomUI);
            }
            else
            {
                Function(id, result, dele);
            }

        }

    }
}
