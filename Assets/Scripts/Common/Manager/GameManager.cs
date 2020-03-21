using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Canvas WindowUICanvas;
    public Camera UICamera;

    //게임오버선택
    private bool _isGameover;
    //플레이어 설정 모드
    private bool _isPlayerControlPause;


    public bool Gameover
    {
        get { return _isGameover; }
        set
        {
            _isGameover = value;
        }
    }
    public bool PlayerControlPause
    {
        get { return _isPlayerControlPause; }
        set
        {
            if (value == true)
                PlayerManager.instance.MoveStop();

            _isPlayerControlPause = value;
        }
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }

        _isGameover = false;
        _isPlayerControlPause = false;
    }

    private void Start()
    {
        WindowUICanvas = GameObject.Find("WindowUI").GetComponent<Canvas>();
        UICamera = WindowUICanvas.worldCamera;
    }
}
