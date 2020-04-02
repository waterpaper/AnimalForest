using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    //접근시 원하는 씬으로 바꾸기 위해 설정하는 클래스입니다.
    public SceneKind changeScene;
    public Vector3 spwanLocation;
    private GameObject _player;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            StartSceneChange();
        }
    }

    public void StartSceneChange()
    {
        SceneLoader.instance.playerLocationTemp = spwanLocation;
        SceneLoader.instance.SceneLoaderStart(changeScene);
        
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player != null&& changeScene != SceneKind.Start && changeScene != SceneKind.Town)
            _player.transform.position = spwanLocation;
    }
}
