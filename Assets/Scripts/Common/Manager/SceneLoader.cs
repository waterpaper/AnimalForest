using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneKind { Null, Title, Custom, Start, Town, Field1, Field1_Boss, End };

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{
    public Vector3 playerLocationTemp;
    private bool startSetting = false;

    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>();

    private SceneKind nowSceneKind = SceneKind.Title;

    public void Awake()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public SceneKind NowSceneKind()
    {
        return nowSceneKind;
    }

    public void SceneLoaderStart(SceneKind scenekind)
    {
        //씬 로드를 시작하는 함수이다
        loadScenes.Clear();

        SceneLoad(scenekind);
    }

    IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        //비동기 방식으로 씬을 로드하고 완료될때까지 대기
        yield return SceneManager.LoadSceneAsync(sceneName, mode);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    void SceneLoad(SceneKind scenekind)
    {
        SceneManager.LoadScene("FadeScene");
        //로드할 씬에 맞게 씬을 로드하고 삭제합니다.

        nowSceneKind = scenekind;

        loadScenes.Add("PlayScene", LoadSceneMode.Additive);

        switch (scenekind)
        {
            case SceneKind.Title:
                loadScenes.Add("TitleScene", LoadSceneMode.Additive);
                break;
            case SceneKind.Custom:
                loadScenes.Add("PlayerCustomScene", LoadSceneMode.Additive);
                break;
            case SceneKind.Start:
                loadScenes.Add("StartGameScene", LoadSceneMode.Additive);
                break;
            case SceneKind.Town:
                loadScenes.Add("TownScene", LoadSceneMode.Additive);
                break;
            case SceneKind.Field1:
                loadScenes.Add("Field1_Scene", LoadSceneMode.Additive);
                break;
            case SceneKind.Field1_Boss:
                loadScenes.Add("Field1_Boss", LoadSceneMode.Additive);
                break;
            default:
                break;
        }

        if (!startSetting)
        {
            startSetting = true;
            StartCoroutine(LoadScene("SettingScene", LoadSceneMode.Additive));
            StartCoroutine(LoadScene("GameUiScene", LoadSceneMode.Additive));
        }
        else if (scenekind == SceneKind.Title)
        {
            //타이틀로 이동시
            SceneManager.UnloadSceneAsync("PlayScene");
            SceneManager.UnloadSceneAsync("GameUiScene");
            startSetting = false;
        }
        else
        {
            PlayerManager.instance.PosistionSetting();
        }

        foreach (var scenes in loadScenes)
        {
            StartCoroutine(LoadScene(scenes.Key, scenes.Value));
        }
    }
}
