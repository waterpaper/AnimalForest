using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BossAction
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Skill1,
    Skill2,
    Hit,
    Die,
    End
}

public class BossAI : MonoBehaviour
{
    public BossAction action = BossAction.Patrol;

    //보스 캐릭터의 위치를 저장할 변수
    private Transform _bossTrans;
    //보스 움직임을 저장하는 클래스
    private BossMovement _bossMovement;
    //보스 정보를 저장하는 클래스
    private BossStatement _bossState;
    //보스 캐릭터의 애니메이션 클래스
    private Animator _bossAni;
    //보스 캐릭터의 rigid바디
    private Rigidbody _bossRigidbody;
    //적 캐릭터의 네브
    private NavMeshAgent _enemyNavAgent;

    //시야각 및 추적 반경을 제어하는 enemyFov클래스를 저장할 변수
    private BossFov _bossFov;
    //공격루티을 처리하는 클래스를 저장하는 변수
    private BossAttack _bossAttack;
    //처음 세팅여부를 판단해주는 변수입니다.
    private bool _bossSetting = false;
    //죽엇는지 여부를 판단해주는 변수입니다.
    public bool isDeath = false;

    //타겟 캐릭터
    public GameObject targetPlayer;

    //애니메이션 입력 데이터입니다.
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashSkill_1 = Animator.StringToHash("IsSkill_1");
    private readonly int hashSkill_2 = Animator.StringToHash("IsSkill_2");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDeath = Animator.StringToHash("IsDeath");
    private readonly int hashGround = Animator.StringToHash("IsGround");

    private void Awake()
    {
        //객체 생성시 사용할 컴포넌트를 입력받습니다.
        _bossTrans = GetComponent<Transform>();
        _bossMovement = GetComponent<BossMovement>();
        _bossState = GetComponent<BossStatement>();
        _bossAni = GetComponentInChildren<Animator>();
        _bossFov = GetComponent<BossFov>();
        _bossAttack = GetComponent<BossAttack>();
        _enemyNavAgent = GetComponent<NavMeshAgent>();
        _bossRigidbody = GetComponent<Rigidbody>();

        targetPlayer = null;
    }

    private void OnEnable()
    {
        //보스가 재 생성할때 마다 초기화 해줍니다.
        Initialize();

        if (_bossSetting == true)
        {
            StartCoroutine(IECheckState());

            //보스 UI를 출력합니다.
            UIManager.instance.BossUISetting(_bossState);
        }
        else
            _bossSetting = true;
    }


    private void Initialize()
    {
        //보스 상태를 초기화할때 사용합니다
        action = BossAction.Idle;

        targetPlayer = null;
        isDeath = false;
        _bossFov.Setting(_bossState.traceDist);

        Ani_Initialize();
    }

    private void Ani_Initialize()
    {
        //에니매이터를 초기화합니다.
        _bossAni.SetBool(hashMove, false);
        _bossAni.SetBool(hashAttack, false);
        _bossAni.SetBool(hashSkill_1, false);
        _bossAni.SetBool(hashSkill_2, false);
        _bossAni.SetBool(hashDeath, false);
        _bossAni.SetBool(hashGround, true);
        _bossAni.SetFloat(hashSpeed, 0.0f);
    }

    IEnumerator IEMovementSetting()
    {
        //유한상태기계를 이용해 각 상태에 맞게 행동 클래스를 수정하고 애니메이션을 바꿔줍니다.
        if (this.gameObject.activeSelf == false) yield return null;

        switch (action)
        {
            case BossAction.Idle:
                _bossMovement.Idle = true;
                Ani_Initialize();
                break;
            case BossAction.Patrol:
                _bossMovement.Patrolling = true;
                _bossMovement.Speed = _bossState.patrolMoveSpeed;
                _bossAni.SetBool(hashMove, true);
                break;
            case BossAction.Trace:
                _bossMovement.Traceing = true;
                _bossMovement.Speed = _bossState.traceMoveSpeed;
                _bossAni.SetBool(hashMove, true);
                break;
            case BossAction.Attack:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashAttack, true);
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Attack);
                break;
            case BossAction.Skill1:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashSkill_1, true);
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Skiil1_UP);
                break;
            case BossAction.Skill2:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashSkill_2, true);
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Skill2);
                break;
            case BossAction.Die:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashDeath, true);
                break;
        }

        yield return null;
    }

    IEnumerator IECheckState()
    {
        //유한상태기계를 이용하기 위해 각 상태를 체크, 설정합니다.
        //죽지 않았을시 0.3초 마다 반복합니다.

        while (isDeath != true)
        {
            //애니메이터에 현재 이동속도를 입력합니다.
            _bossAni.SetFloat(hashSpeed, _enemyNavAgent.speed);

            //현재 상태를 저장합니다.
            //상태가 변하지 않은 경우 IEMovementSetting함수를 실행하지 않음으로써 움직임을 변화시키지 않게 합니다. 
            BossAction actionTemp = action;

            //현재 타겟이 없을때입니다.
            if (targetPlayer == null)
            {
                //추적하는 플레이어가 없을시 범위내의 플레이어를 검사하고 순찰모드를 실행합니다.
                targetPlayer = _bossFov.tracePlayer();
                actionTemp = BossAction.Patrol;
            }
            //현재 공격 상태일시 검사입니다.
            else if (actionTemp == BossAction.Attack || actionTemp == BossAction.Skill1 || actionTemp == BossAction.Skill2)
            {
                //현재 공격 도중인지 여부를 검사합니다.
                if (_bossAttack.IsAttackConfirm(actionTemp) == true)
                {
                    actionTemp = action;
                }
                else
                {
                    //공격상태가 끝났을때 대기상태로 돌아갑니다.
                    actionTemp = BossAction.Idle;
                    _bossAni.SetBool("IsAttacking", false);
                }
            }
            else
            {
                //위에서 특수 상황을 제외하고 거리를 통해 상태를 정의하는 곳입니다.
                //플레이어와 적 캐릭터 간의 거리를 계산합니다.
                float dist = Vector3.Distance(targetPlayer.transform.position, transform.position);

                //플레이어가 탐지범위를 벗어낫을시 타겟을 초기화합니다.
                if (dist > _bossState.traceDist)
                {
                    targetPlayer = null;
                    continue;
                }

                //플레이어와 사이에 장애물이 존재시 순찰 상태로 전환합니다.
                if (_bossFov.isViewPlayer(targetPlayer.transform) == false)
                {
                    actionTemp = BossAction.Patrol;
                }
                else
                {
                    //거리를 계산해 공격 가능 상태를 검사하고 실패나 범위에 해당되지 않을 시 추적을 실행합니다.
                    if (_bossAttack.IsBossAttack(dist, out actionTemp) == false)
                    {
                        _bossMovement.traceTarget = targetPlayer;
                        actionTemp = BossAction.Trace;
                    }
                    else
                    {
                        //공격상태가 설정되면 공격중을 나타내는 애니메이션 변수를 실행하고 방향을 바꿔줍니다.
                        _bossAni.SetBool("IsAttacking", true);
                        transform.LookAt(targetPlayer.transform);
                    }
                }
            }

            //행동이 같으면 변화를 시키지 않고 행동이 다르면 변화를 시킵니다.
            if (actionTemp != action || action == BossAction.Die)
            {
                action = actionTemp;
                StartCoroutine(IEMovementSetting());
            }

            yield return new WaitForSeconds(0.3f);
        }

        if (isDeath == true)
        {
            //죽엇을시 상태를 die로 변화후 상태 설정과 죽었을 때 처리하는 함수를 실행합니다.
            action = BossAction.Die;
            StartCoroutine(IEMovementSetting());
            StartCoroutine(IEDeath());
        }
    }
    
    IEnumerator IEDeath()
    {
        // 죽엇을시 실행되는 열거자 함수입니다.
        // 아이템과 경험치같은 보상을 처리하고 죽은 모션을 처리합니다.(일정 시간이 지난후 객체를 없애줍니다.)
        DropItemManager.instance.DropItemSetting(_bossState.ID, _bossTrans);
        PlayerManager.instance.Exp += _bossState.exp;
        QuestManager.instance.UpdateQuest(EQuestKind.EQuestKind_BossHunting, _bossState.ID);
        UIManager.instance.BossUISetting(null);

        _bossMovement.Stop();
        Ani_Initialize();
        _bossAni.SetBool(hashDeath, true);

        GameObject.FindGameObjectWithTag("EventTrigger_Light").GetComponent<EventTrigger>().EventStart();
        yield return new WaitForSeconds(5.0f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            _bossAni.SetBool(hashGround, true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _bossRigidbody.velocity = Vector3.zero;
        }
    }
}
