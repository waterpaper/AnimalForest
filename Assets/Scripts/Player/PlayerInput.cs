using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{ 
    //앞, 뒤
    public string verticalMoveAxisName = "Vertical";
    //회전
    public string horizontalMoveAxisName = "Horizontal";
    //공격
    public string attackButtonName = "Fire1";
    //연속공격
    public string comboAttackButtonName = "Fire2";
    //달리기
    public string sprintName = "SprintInput";
    //점프
    public string jumpName = "Jump";
    //구르기
    public string rollName = "Roll";


    //[Header("NowCondition")]
    //할당은 내부에서만 가능
    public float vertical { get; set; }
    public float horizontal { get; set; }
    public float rotate { get; set; }
    public bool sprint { get; set; }
    public bool jump { get; set; }
    public bool roll { get; set; }
    public bool normalAttack { get; set; }
    public bool comboAttack { get; set; }

    public bool IsJoyStickControll { get; set; }

    private void Awake()
    {
        IsJoyStickControll = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 게임오버상태에서는 입력을 감지하지 않는다.
        if (GameManager.instance != null && GameManager.instance.Gameover && GameManager.instance.PlayerControlPause)
        {
            vertical = 0;
            horizontal = 0;
            rotate = 0;
            sprint = false;
            jump = false;
            normalAttack = false;
            comboAttack = false;
            roll = false;
            IsJoyStickControll = false;

            return;
        }

        if (IsJoyStickControll == false)
        {
            vertical = Input.GetAxis(verticalMoveAxisName);
            horizontal = Input.GetAxis(horizontalMoveAxisName);
            sprint = Input.GetButton(sprintName);
            jump = Input.GetButtonDown(jumpName);
            normalAttack = Input.GetButtonDown(attackButtonName);
            comboAttack = Input.GetButtonDown(comboAttackButtonName);
            roll = Input.GetButtonDown(rollName);
        }
        else { IsJoyStickControll = false; }

        //플레이어의 클릭입력을 처리한다.
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.PlayerControlPause && CameraManager.instance.playMode)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Physics.Raycast(ray, out hit);
            if (hit.collider != null && hit.collider.tag == "Npc")
            {
                hit.collider.gameObject.GetComponent<SelectedNpc>().Selected();
            }
        }
    }
}
