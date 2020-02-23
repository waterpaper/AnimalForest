using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public Canvas WindowUICanvas;
    public Camera UICamera;

    //싱글톤 접근
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
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
            if(value == true)
                PlayerManager.instance.MoveStop();

            _isPlayerControlPause = value;
        }
    }

    private void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        _isGameover = false;
        _isPlayerControlPause = false;
    }

    private void Start()
    {
        WindowUICanvas = GameObject.Find("WindowUI").GetComponent<Canvas>();
        UICamera = WindowUICanvas.worldCamera;
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ClickParticle());
        }
    }

    IEnumerator ClickParticle()
    {
        //월드 좌표를 스크린 좌표로 변환
        var point = Input.mousePosition;

        //카메라의 뒷쪽 영역일때 좌푯값 보정
        if (point.z < 0.0f)
        {
            point *= -1.0f;
        }

        yield return new WaitForSeconds(2.0f);
    }
}
