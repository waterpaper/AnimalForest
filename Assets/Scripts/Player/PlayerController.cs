﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Initialize")]
    //움직이는 모델
    public GameObject activeModel;
    //공격모션 3단계
    public string[] randomAttacks;
    //이동 속도
    public float moveAmount;
    //메인캐릭터 이동방향
    public Vector3 moveDir;

    [Header("Stats")]
    //움직이는 스피드
    public float moveSpeed = 4.5f;
    //뛰는 스피드
    public float sprintSpeed = 7.0f;
    //점프 파워
    public float jumpForce = 300.0f;
    //캐릭터가 도는 속도
    public float rotateSpeed = 5;

    //애니매이터 컴포턴트
    Animator _anim;
    //리지드바디 컴포넌트
    [HideInInspector]
    public Rigidbody _rigid;
    [HideInInspector]
    public PlayerInput _playerInput;
    [HideInInspector]
    public PlayerAttack _playerAttack;
    public bool _isGround { get; private set; }
    public bool _isMove { get; private set; }
    public bool _isAttack { get; private set; }
    public bool _isRoll { get; private set; }

    //트레일러를 그려주기 위한 무기위치
    public GameObject _weaponTrailer;

    public bool sprintSetting;


    // 각 컴포넌트를 초기화합니다.
    void Start()
    {
        SetAnimator();
        _rigid = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAttack = GetComponent<PlayerAttack>();
        CameraManager.instance.Init(gameObject.transform);
        _isGround = true;
        _isAttack = false;
        _isRoll = false;
        sprintSetting = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 게임오버상태에서 상태값을 초기화한다.
        if (GameManager.instance != null && GameManager.instance.Gameover)
        {
            _isMove = false;
            _isGround = false;
            _isAttack = false;

            return;
        }

        if (GameManager.instance.PlayerControlPause) return;

            UpdateState();

    }

    void SetAnimator()
    {
        //애니메이터를 세팅해준다.
        if (activeModel == null)
        {
            //자식 오브젝트중에서 애니메이터를 찾는다.
            _anim = GetComponentInChildren<Animator>();
            if (_anim == null)
            {
                Debug.Log("No model");
            }
            else
            {
                //애니메이터를 가진 오브젝트를 액션 모델로 재설정해준다.
                activeModel = _anim.gameObject;
            }
        }

        if (_anim == null)
            _anim = activeModel.GetComponent<Animator>();
    }

    //물리움직임을 fixedupdate로 업데이트합니다.
    void FixedUpdate()
    {
        if (GameManager.instance.PlayerControlPause)
        {
            _isAttack = false;
            _anim.SetBool("sprint", false);
            _anim.SetFloat("vertical", 0.0f);
            return;
        }

        if (_isMove && _isGround)
        {
            Move();
        }

        CameraManager.instance.FixedTick(Time.fixedDeltaTime);
    }

    void Move()
    {
        // 일반스피드입니다.
        float targetSpeed = moveSpeed;


        //달리기시 달리기 스피드입니다.
        if (_playerInput.sprint)
        {
            sprintSetting = !sprintSetting;
        }

        if (sprintSetting)
        {
            targetSpeed = sprintSpeed;
        }

        //카메라 중점 좌표로 이동 좌표를 설정해준다(3인칭)
        Vector3 v = _playerInput.vertical * CameraManager.instance.transform.forward;
        Vector3 h = _playerInput.horizontal * CameraManager.instance.transform.right;

        //방향을 설정하고 속도를 곱해준다.
        moveDir = ((v + h).normalized) * (targetSpeed * moveAmount);

        //y값은 점프가 있을수 있으므로 rigid값에서 가져온다.
        moveDir.y = _rigid.velocity.y;

        //나머지
        float m = Mathf.Abs(_playerInput.horizontal) + Mathf.Abs(_playerInput.vertical);
        moveAmount = Mathf.Clamp01(m);

        if (_isMove)
        {
            //땅이고 움직일 수 있을시 앞으로 이동한다.
            _rigid.velocity = moveDir;
        }


        //움직일 수 있을시 캐릭터를 돌려준다.
        if (_isMove)
        {
            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;
        }

        HandleMovementAnimations(); //update character's animations.
    }

    void UpdateState()
    {
        /*
         * 애니메이션 상태를 바꿔줍니다.
         */

        //이동상태를 받아옵니다.
        _isMove = _anim.GetBool("isMove");

        //공격이 끝낫다면 트레일러를 꺼줍니다.
        if (_isAttack)
        {
            if (_anim.GetBool("isNotAttack"))
            {
                _isAttack = false;
                _weaponTrailer.SetActive(false);
                _playerAttack.endAttack();
            }
        }

        //점프할때입니다.
        if (_playerInput.jump)
        {
            if (_isGround && _isMove)//점프를 방금 눌렀을시
            {
                _anim.CrossFade("falling", 0.1f);
                _rigid.AddForce(0, jumpForce, 0);//점프 에니메이션을 작동시킨다.
            }
        }
        //콤보 공격입니다.
        if (_playerInput.comboAttack && !_playerInput.normalAttack)
        {
            if (InventoryManager.instance.NowEquipmentWeaponItem() == false) return;

            //땅에서만 가능합니다.
            if (_isGround)
            {
                _anim.SetTrigger("combo");
                _isAttack = true;
                _weaponTrailer.SetActive(true);
                _playerAttack.startAttack();
            }
        }
        //회피입니다.
        if (_playerInput.roll && _isGround)
        {
            _anim.SetTrigger("roll");
            _isRoll = true;
        }

        //공격입니다
        if (_playerInput.normalAttack && _isMove)
        {
            if (InventoryManager.instance.NowEquipmentWeaponItem() == false) return;

            string targetAnim;

            _isAttack = true;
            _weaponTrailer.SetActive(true);

            //chosing random attack from array.
            int r = Random.Range(0, randomAttacks.Length);
            targetAnim = randomAttacks[r];

            _anim.CrossFade(targetAnim, 0.1f); //play the target animation in 0.1 second.                 

            if (!_isGround)
            {
                _anim.CrossFade("JumpAttack", 0.1f); // When you are air born, you do this jump attack.
            }

            _playerInput.normalAttack = false;
            _playerAttack.startAttack();
        }
    }


    void HandleMovementAnimations()
    {
        //뛰는 상태를 입력받은 대로 처리하고 아니면 false로 입력한다.
        _anim.SetBool("sprint", _playerInput.sprint);
        if (moveAmount == 0)
        {
            _anim.SetBool("sprint", false);
        }

        _anim.SetFloat("vertical", moveAmount, 0.2f, Time.deltaTime);
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = true;
            _anim.SetBool("isGround", true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _isGround = false;
            _anim.SetBool("isGround", false);
        }
    }
}

