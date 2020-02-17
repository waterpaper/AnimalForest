using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleToGameStart : MonoBehaviour
{
    private SceneChange sceneChange;

    public void Start()
    {
        sceneChange = GetComponent<SceneChange>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine("StartGame");
        }
    }

    IEnumerator NewStartGame()
    {
        SceneLoader.instance.SceneLoaderStart(SceneKind.Start);
        yield return null;
    }

    IEnumerator StartGame()
    {
        sceneChange.changeScene = SceneKind.Custom;
        sceneChange.spwanLocation = new Vector3(0.0f, 5.0f, 0.0f);
        sceneChange.StartSceneChange();

        yield return null;
    }
}
