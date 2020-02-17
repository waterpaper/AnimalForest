using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup fadeCg;
    [Range(0.5f, 2.0f)]
    public float fadeDuration = 1.0f;

    void Start()
    {
        //페이드 인 아웃을 시작하는 함수이다.
        fadeCg.alpha = 1.0f;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        //fade in/out 하는 함수
        fadeCg.blocksRaycasts = true;

        //절대값 함수로 백분율을 계산
        float fadeSpeed = Mathf.Abs(fadeCg.alpha - 0.0f) / fadeDuration;

        //알파값을 조정
        while (!Mathf.Approximately(fadeCg.alpha, 0.0f))
        {
            fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, 0.0f, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        fadeCg.blocksRaycasts = false;
        

        //fadeIn이 완료되면 fade씬 제거
        SceneManager.UnloadSceneAsync("FadeScene");
    }
}
