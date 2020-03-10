using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GoogleLogin : MonoBehaviour
{
    public TextMeshProUGUI resultTextUI;

    // Start is called before the first frame update
    void Start()
    {
        //구글게임 서비스 활성화
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        resultTextUI = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void LoginButton()
    {
        //로그인을 수행합니다.
        StartCoroutine(LoginConnection());
    }

    IEnumerator LoginConnection()
    {
        //구글 로그인이 되어있다면 실행하는 로그인구문입니다.
        if (Social.localUser.authenticated)
        {
            if (resultTextUI != null)
            {
                resultTextUI.text = "로그인에 성공하였습니다.\n게임을 접속합니다.";
                resultTextUI.gameObject.SetActive(true);

                yield return new WaitForSeconds(2.0f);

                UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
                UIManager.instance.UISetting(UiKind.UiKind_CustomUI);
            }
        }
        else
        {
            //로그인 인증 처리함수입니다.
            Social.localUser.Authenticate((bool success) => {
                if (success)
                {
                    if (resultTextUI != null)
                    {
                        resultTextUI.text = "로그인에 성공하였습니다.\n게임을 접속합니다.";
                        resultTextUI.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (resultTextUI != null)
                    {
                        resultTextUI.text = "구글 로그인 에러.\nname :" + Social.localUser.userName;
                        resultTextUI.gameObject.SetActive(true);
                    }
                }
            });

            yield return new WaitForSeconds(5.0f);


            Debug.Log("name :" + Social.localUser.userName);
            if (Social.localUser.authenticated)
            {
                UIManager.instance.UISetting(UiKind.UiKind_LoginUI);
                UIManager.instance.UISetting(UiKind.UiKind_CustomUI);
            }
        }
    }
}
