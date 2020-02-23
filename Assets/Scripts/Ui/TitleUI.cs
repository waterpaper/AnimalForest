using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    private SceneChange sceneChange;
    private bool singleKey = false;

    public void Start()
    {
        SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_Title);
        sceneChange = GetComponent<SceneChange>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && !singleKey)
        {
            singleKey = true;
            StartCoroutine("StartGame");
        }
    }

    IEnumerator StartGame()
    {
        //로그인이 성공했을때 실행됩니다.
        sceneChange.changeScene = SceneKind.Custom;
        sceneChange.StartSceneChange();

        yield return null;
    }
}
