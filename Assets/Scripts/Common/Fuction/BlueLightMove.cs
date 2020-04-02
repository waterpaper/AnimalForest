using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueLightMove : MonoBehaviour
{
    [Header("Bluelight CameraAttribute")]
    //메인 카메라와 솔로 카메라입니다.
    public GameObject mainCamera;
    public GameObject soloCamera;

    [Header("Bluelight PositionAttribute")]
    //현재 이동 포지션
    public int nowPosition;
    //이동해야할 좌표를 저장해둔 list
    public List<Transform> movePositionList;
    //혼자 이동해야할 위치를 지정해주는 List입니다.
    public List<int> soloMovePositionList;
    //불빛이 움직이는 스피드
    public float lightSpeed = 10.0f;
    //근처를 판단하는 거리
    public float lightDistance = 20.0f;

    [Header("Bluelight PlayerAttribute")]
    //플레이어 좌표를 저장
    public Transform playerTransform;
    //빛과의 근접거리 판단 거리
    public float lightToPlayerDistance = 5.0f;
    
    //불빛의 움직임 여부입니다.
    private bool _isMove;
    private bool _isSoloMove;

    //불빛의 위치정보
    private Transform _transForm;
    //현재 목표 포지션
    private Vector3 _nowMovePosition;

    void Start()
    {
        _transForm = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").transform;

        mainCamera = playerTransform.GetComponentInChildren<Camera>().gameObject;
        soloCamera = _transForm.Find("Camera").gameObject;

        nowPosition = 0;
        _nowMovePosition = Vector3.zero;
        _isMove = false;
        _isSoloMove = false;
    }


    private void FixedUpdate()
    {
        //불빛을 이동시킨다.
        if (_isMove)
        {
            LightMove();
        }
        else
        {
            //다음 위치로 목표를 이동시키는지 여부를 검사합니다.
            NextLocation();
        }
    }

    //불빛을 이동시킨다.
    void LightMove()
    {
        //빛의 이동을 처리합니다.
        //빛과 목표위치 근접유무 판단하고 근접했을시 이동을 종료시킵니다.
        if (LocationPositionCheck())
        {
            _isMove = false;
            return;
        }

        //빛을 이동시킵니다.
        _transForm.Translate(Vector3.forward * Time.fixedDeltaTime * lightSpeed);
        //회전합니다
        Vector3 targetDir = movePositionList[nowPosition].position - transform.position;
        Quaternion tr = Quaternion.LookRotation(targetDir.normalized);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.fixedDeltaTime * 3.0f);
        transform.rotation = targetRotation;

    }

    void NextLocation()
    {
        //이동이 종료되었을때 다음 위치로 설정합니다
        if (nowPosition + 1 >= movePositionList.Count)
        {
            //혼자 이동하는 빛이 세팅되어 있는 경우 원 상태로 돌려줍니다.
            if (_isSoloMove == true)
                SoloMoveSetting(false);

            return;
        }

        //플레이어가 근접한 경우 다음위치로 설정해줍니다.
        if(PlayerPositionCheck() == true)
        {
            nowPosition++;
            _isMove = true;

            //빛이 혼자이동하는 부분일 경우
            if (IsSoloMove() == true)
            {
                //혼자 이동하는 세팅이 되어있지 않은 경우 실행합니다.
                if (_isSoloMove == false)
                    SoloMoveSetting(true);
            }
            else
            {
                if (_isSoloMove == true)
                    //혼자 이동하는 빛이 세팅되어 있는 경우 원 상태로 돌려줍니다.
                    SoloMoveSetting(false);
            }
        }
        else
        {
            if (IsSoloMove() == true)
            {
                //플레이어가 근접하지는 않앗으나 계속 이동하는 부분인 경우
                nowPosition++;
                _isMove = true;

                //혼자 이동하는 세팅이 되어있지 않은 경우 실행합니다.
                if (_isSoloMove == false)
                    SoloMoveSetting(true);
            }
            else
            {
                if (_isSoloMove == true)
                    //혼자 이동하는 빛이 세팅되어 있는 경우 원 상태로 돌려줍니다.
                    SoloMoveSetting(false);
            }
        }
    }


    //불빛과 플레이이사이의 거리가 얼만지 판단하고 최소 요건과 비교해 리턴한다
    bool LightToPlayerDistance()
    {
        Vector3 distance = playerTransform.position - gameObject.transform.position;

        float temp = distance.sqrMagnitude;

        if (temp < lightDistance * lightDistance)
            return true;
        else
            return false;
    }

    //불빛과 목표 사이의 거리를 판단하고 최소 요건과 비교해 
    bool LightToDestinationDistance()
    {
        Vector3 distance = _nowMovePosition - gameObject.transform.position;

        float temp = distance.sqrMagnitude;

        if (temp < 1.0f * 1.0f)
        {
            _isMove = false;
            return true;
        }
        else
            return false;
    }

    //빛과 원하는 위치 거리를 체크해 근접하면 다음으로 아니면 이동x
    bool LocationPositionCheck()
    {
        Vector3 offset = movePositionList[nowPosition].position - _transForm.position;

        if (Vector3.SqrMagnitude(offset) < 1.0f)
        {
            _isMove = false;
            return true;
        }
        else
            return false;
    }

    //빛과 플레이어의 거리를 체크해 근접하면 다음으로 아니면 이동x
    bool PlayerPositionCheck()
    {
        Vector3 offset = movePositionList[nowPosition].position - playerTransform.position;

        if (Vector3.SqrMagnitude(offset) < lightToPlayerDistance * lightToPlayerDistance)
            return true;
        else
            return false;
    }

    //다음 위치가 빛 혼자 이동하는 부분인지 판단합니다.
    bool IsSoloMove()
    {
        for (int i = 0; i < soloMovePositionList.Count; i++)
        {
            if (nowPosition == soloMovePositionList[i])
                return true;
        }

        return false;
    }

    //혼자 이동하기위한 변수를 세팅해줍니다.
    void SoloMoveSetting(bool soloMoveTrue)
    {
        if (soloMoveTrue)
        {
            //카메라와 UI를 따로 설정해 세팅해줍니다. 
            mainCamera.SetActive(false);
            soloCamera.SetActive(true);

            _isSoloMove = true;
            lightSpeed += 5.0f;

            GameManager.instance.PlayerControlPause = true;
            CameraManager.instance.playMode = false;
            UIManager.instance.SaveToDisable_ActiveUI();
        }
        else
        {
            //카메라와 UI를 원래대로 돌려줍니다.
            soloCamera.SetActive(false);
            mainCamera.SetActive(true);

            lightSpeed -= 5.0f;
            _isSoloMove = false;

            GameManager.instance.PlayerControlPause = false;
            CameraManager.instance.playMode = true;
            UIManager.instance.Load_ActiveUI();
        }
    }
}
