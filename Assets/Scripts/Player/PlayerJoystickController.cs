using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoystickController : MonoBehaviour
{
    public Transform Stick;
    public PlayerInput playerInput;
    Vector3 axis;

    float radius;
    Vector3 defaultCenter;
    Touch myTouch;
    
    void Start()
    {
        radius = GetComponent<RectTransform>().sizeDelta.y / 4;
        defaultCenter = Stick.position;
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
    }

    public void Move()
    {
        Vector3 touchPos = Input.mousePosition;
        axis = (touchPos - defaultCenter).normalized;

        float Distance = Vector3.Distance(touchPos, defaultCenter);

        if (Distance > radius)
            Stick.position = defaultCenter + axis * radius;
        else
            Stick.position = defaultCenter + axis * Distance;

        playerInput.vertical = axis.y;
        playerInput.horizontal = axis.x;

        playerInput.IsJoyStickControll = true;
    }

    public void End()
    {
        axis = Vector3.zero;
        Stick.position = defaultCenter;

        playerInput.vertical = axis.y;
        playerInput.horizontal = axis.x;
        playerInput.IsJoyStickControll = true;
    }

    public void RotationStart()
    {
        Vector3 touchPos = Input.mousePosition;
        axis = (touchPos - defaultCenter).normalized;

        float Distance = Vector3.Distance(touchPos, defaultCenter);

        if (Distance > radius)
            Stick.position = defaultCenter + axis * radius;
        else
            Stick.position = defaultCenter + axis * Distance;

        CameraManager.instance.FixedTick(Time.deltaTime, axis.x*2, -axis.y*2);

        playerInput.IsJoyStickControll = true;
    }

    public void RotationEnd()
    {
        axis = Vector3.zero;
        Stick.position = defaultCenter;

        playerInput.IsJoyStickControll = true;
    }

    public void AttackButton()
    {
        playerInput.normalAttack = true;
        playerInput.IsJoyStickControll = true;
    }

    public void RollButton()
    {
        playerInput.roll = true;
        playerInput.IsJoyStickControll = true;
    }

    public void JumpButton()
    {
        playerInput.jump = true;
        playerInput.IsJoyStickControll = true;
    }

    public void ComboAttackButton()
    {
        playerInput.comboAttack = true;
        playerInput.IsJoyStickControll = true;
    }

    public void SprintButton()
    {
        playerInput.sprint = true;
        playerInput.IsJoyStickControll = true;
    }
}
