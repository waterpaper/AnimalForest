using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystickController : MonoBehaviour
{
    public Transform stick;
    public GameObject pad;

    private PlayerInput _playerInput;

    //움직인 거리입니다.
    Vector3 axis;
    //반지름
    float radius;
    //이동후 돌아오는 센터자리입니다.
    Vector3 defaultCenter;
    
    void Start()
    {
        radius = GetComponent<RectTransform>().sizeDelta.y / 4;
        _playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
    }

    private void OnDisable()
    {
        End();
    }

    public void Move()
    {
        //패드에서 스틱을 움직인 방향을 단위벡터로 검출해 해당 방향으로 스피드 만큼 이동하게 합니다.
        Vector3 touchPos = Vector3.zero;

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            //mobile
            touchPos = myTouch.position;
        }
        else
        {
            //pc용
            touchPos = Input.mousePosition;   
        }

        axis = (touchPos - defaultCenter).normalized;

        float Distance = Vector3.Distance(touchPos, defaultCenter);

        if (Distance > radius)
            stick.position = defaultCenter + axis * radius;
        else
            stick.position = defaultCenter + axis * Distance;

        _playerInput.vertical = axis.y;
        _playerInput.horizontal = axis.x;

        _playerInput.IsJoyStickControll = true;
    }

    public void End()
    {
        //종료시 다시 윈위치로 복구합니다.
        axis = Vector3.zero;
        defaultCenter = pad.transform.position;
        stick.position = defaultCenter;

        _playerInput.vertical = axis.y;
        _playerInput.horizontal = axis.x;
        _playerInput.IsJoyStickControll = true;
    }

    public void RotationStart()
    {
        Vector3 touchPos = Vector3.zero;

        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);
            //mobile
            touchPos = myTouch.position;
        }
        else
        {
            //pc용
            touchPos = Input.mousePosition;
        }

        axis = (touchPos - defaultCenter).normalized;

        float Distance = Vector3.Distance(touchPos, defaultCenter);

        if (Distance > radius)
            stick.position = defaultCenter + axis * radius;
        else
            stick.position = defaultCenter + axis * Distance;

        CameraManager.instance.FixedTick(Time.deltaTime, axis.x, -axis.y);

        _playerInput.IsJoyStickControll = true;
    }

    public void RotationEnd()
    {
        axis = Vector3.zero;
        defaultCenter = pad.transform.position;
        stick.position = defaultCenter;

        _playerInput.IsJoyStickControll = true;
    }

    public void AttackButton()
    {
        _playerInput.normalAttack = true;
        _playerInput.IsJoyStickControll = true;
    }

    public void RollButton()
    {
        _playerInput.roll = true;
        _playerInput.IsJoyStickControll = true;
    }

    public void JumpButton()
    {
        _playerInput.jump = true;
        _playerInput.IsJoyStickControll = true;
    }

    public void ComboAttackButton()
    {
        _playerInput.comboAttack = true;
        _playerInput.IsJoyStickControll = true;
    }

    public void SprintButton()
    {
        _playerInput.sprint = true;
        _playerInput.IsJoyStickControll = true;
    }
}
