using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
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

    public BossAction action = BossAction.Patrol;

    //보스 캐릭터의 위치를 저장할 변수
    private Transform _bossTrans;
    //보스 움직임을 저장하는 클래스
    private BossMovement _bossMovement;
    //보스 정보를 저장하는 클래스
    private BossStatment _bossState;
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

    public bool isDeath = false;

    //타겟 캐릭터
    public GameObject targetPlayer;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSkill_1 = Animator.StringToHash("IsSkill_1");
    private readonly int hashSkill_2 = Animator.StringToHash("IsSkill_2");
    private readonly int hashSkill_3 = Animator.StringToHash("IsSkill_3");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDeath = Animator.StringToHash("IsDeath");
    private readonly int hashGround = Animator.StringToHash("IsGround");

    private void Awake()
    {
        _bossTrans = GetComponent<Transform>();
        _bossMovement = GetComponent<BossMovement>();
        _bossState = GetComponent<BossStatment>();
        _bossAni = GetComponentInChildren<Animator>();
        _bossFov = GetComponent<BossFov>();
        _bossAttack = GetComponent<BossAttack>();
        _enemyNavAgent = GetComponent<NavMeshAgent>();
        _bossRigidbody = GetComponent<Rigidbody>();

        targetPlayer = null;
    }

    private void OnEnable()
    {
        Initialize();

        if (_bossSetting == true)
        {
            StartCoroutine(IECheckState());
        }
        else
            _bossSetting = true;
    }
    

    private void Initialize()
    {
        //초기화할때 사용합니다
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
        _bossAni.SetBool(hashSkill_1, false);
        _bossAni.SetBool(hashSkill_2, false);
        _bossAni.SetBool(hashSkill_3, false);
        _bossAni.SetBool(hashDeath, false);
        _bossAni.SetBool(hashGround, true);
        _bossAni.SetFloat(hashSpeed, 0.0f);
    }

    IEnumerator IEMovementSetting()
    {
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
                _bossAni.SetBool(hashSkill_1, true);
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Attack);
                break;
            case BossAction.Skill1:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashSkill_2, true);
                SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Skiil1_UP);
                break;
            case BossAction.Skill2:
                _bossMovement.Stop();
                Ani_Initialize();
                _bossAni.SetBool(hashSkill_3, true);
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
        while (isDeath != true)
        {
            _bossAni.SetFloat(hashSpeed, _enemyNavAgent.speed);

            BossAction actionTemp = action;

            if (targetPlayer == null)
            {
                //추적하는 플레이어가 없을시 범위를 검사하고 순찰모드를 실행합니다.
                targetPlayer = _bossFov.tracePlayer();
                actionTemp = BossAction.Patrol;
            }
            else if (actionTemp == BossAction.Attack || actionTemp == BossAction.Skill1 || actionTemp == BossAction.Skill2)
            {
                if (_bossAttack.IsAttackConfirm(actionTemp) == true)
                {
                    actionTemp = action;
                }
                else
                {
                    actionTemp = BossAction.Idle;
                    _bossAni.SetBool("IsAttacking", false);
                }
            }
            else
            {
                //플레이어와 적 캐릭터 간의 거리를 계산합니다.
                float dist = Vector3.Distance(targetPlayer.transform.position, transform.position);

                //플레이어가 탐지범위를 벗어낫을시
                if (dist > _bossState.traceDist)
                {
                    targetPlayer = null;
                    continue;
                }

                //플레이어와 사이에 장애물이 존재시
                if (_bossFov.isViewPlayer(targetPlayer.transform) == false)
                {
                    actionTemp = BossAction.Patrol;
                }
                else
                {
                    //공격상태를 검사하고 실패나 범위를 벗어날 시 추적을 실행합니다.
                    if (_bossAttack.IsBossAttack(dist, out actionTemp) == false)
                    {
                        _bossMovement.traceTarget = targetPlayer;
                        actionTemp = BossAction.Trace;
                    }
                    else
                    {
                        //공격상태가 설정되면 공격중을 나타내는 애니메이션 변수를 on시킨다.
                        _bossAni.SetBool("IsAttacking", true);
                        transform.LookAt(targetPlayer.transform);
                    }
                }
            }
            
            //행동이 같으면 변화를 시키지 않고 행동이 다르면 변화를 시킵니다.
            if(actionTemp != action || action == BossAction.Die)
            {
                action = actionTemp;
                StartCoroutine(IEMovementSetting());
            }

            yield return new WaitForSeconds(0.3f);
        }

        if (isDeath == true)
        {
            action = BossAction.Die;
            StartCoroutine(IEMovementSetting());
            StartCoroutine(IEDeath());
        }
    }

    IEnumerator IEPlayerSearch()
    {
        //추적, 공격시 더 가까운 플레이어를 찾기위한 함수입니다.
        while (action != BossAction.Die)
        {
            if (gameObject.activeSelf == false) break;

            if (targetPlayer != null)
            {
                targetPlayer = _bossFov.tracePlayer();
            }

            yield return new WaitForSeconds(3.0f);
        }
    }
    IEnumerator IEDeath()
    {
        DropItemManager.instance.DropItemSetting(_bossState.ID, _bossTrans);
        PlayerManager.instance.Exp += _bossState.exp;
        _bossMovement.Stop();
        Ani_Initialize();
        _bossAni.SetBool(hashDeath, true);

        QuestManager.instance.UpdateQuest(EQuestKind.EQuestKind_BossHunting, _bossState.ID);
        GameObject.FindGameObjectWithTag("EventTrigger_Light").GetComponent<EventTrigger>().EventStart();
        yield return new WaitForSeconds(5.0f);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            _bossAni.SetBool(hashGround, true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _bossRigidbody.velocity = Vector3.zero;
        }
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ground")
        {
            _bossAni.SetBool(hashGround, false);
        }
    }
    */
}
