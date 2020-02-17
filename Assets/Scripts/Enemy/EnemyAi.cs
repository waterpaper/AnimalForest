using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public enum EnemyAction
    {
        Idle,
        Patrol,
        Trace,
        Attack,
        Hit,
        Die,
        End
    }

    public EnemyAction action = EnemyAction.Patrol;

    //적 캐릭터의 위치를 저장할 변수
    private Transform _enemyTrans;
    //적 움직임을 저장하는 클래스
    private EnemyMovement _enemyMovement;
    //적 정보를 저장하는 클래스
    private EnemyStatement _enemyState;
    //적 캐릭터의 애니메이션 클래스
    private Animator _enemyAni;
    //적 캐릭터의 네브
    private NavMeshAgent _enemyNavAgent;

    //공격 사정거리
    public float attackDist = 3.0f;
    //추적 사정거리
    public float traceDist = 15.0f;
    [Range(0, 360)]
    //적 캐릭터의 시야각
    public float viewAngle = 120f;
    //적 캐릭터의 히트 여부
    public bool isHit = false;
    //적 캐릭터의 사망 여부
    public bool isDie = false;
    //캐릭터 공격 애니매이션 끝난 여부를 가져옵니다
    public bool isAniAttack;

    //타겟 캐릭터
    public GameObject targetCharacter;
    //캐릭터들을 저장할 변수
    private List<GameObject> Characters;

    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDeath = Animator.StringToHash("IsDeath");
    private readonly int hashHit = Animator.StringToHash("IsHit");

    private void Awake()
    {
        _enemyTrans = GetComponent<Transform>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyAni = GetComponentInChildren<Animator>();
        _enemyState = GetComponent<EnemyStatement>();
        _enemyNavAgent = GetComponent<NavMeshAgent>();

        Characters = new List<GameObject>();

        targetCharacter = null;
    }

    private void OnEnable()
    {
        //초기화할때 사용합니다
        action = EnemyAction.Idle;
        isDie = false;
        isHit = false;

        Characters.Clear();

        var t = GameObject.FindGameObjectWithTag("Player");

        if (t != null)
            Characters.Add(t);

        targetCharacter = null;
        _enemyAni.SetBool(hashMove, false);
        _enemyAni.SetBool(hashAttack, false);

        StartCoroutine("CheckState");
    }

    IEnumerator CheckState()
    {
        //타겟 캐릭터와 현재 상태를 정의해주는 코르틴 함수입니다.

        //사망하기 전까지 무한 루프
        while (!isDie)
        {
            isAniAttack = _enemyAni.GetBool(hashAttack);

            //행동이 적용되지 않고 움직이지 않는 상태를 처리하기 위한 예외처리입니다.
            if (_enemyMovement.Idle)
                action = EnemyAction.Idle;
            //행동이 공격 상태이고 모션이 끝난 상태이면 상태를 대기상태로 전환합니다.
            if (action == EnemyAction.Attack && !isAniAttack)
            {
                action = EnemyAction.Trace;
                ActionSetting();
            }

            //상태값을 저장하기 위한 변수입니다.
            EnemyAction actionTemp = EnemyAction.End;
            float dist;


            if (action == EnemyAction.Die) yield break;

            //가장 가까운 캐릭터를 타겟으로 정합니다.
            dist = SearchMinDistanceCharacter();

            if (targetCharacter != null)
            {
                //공격 사정거리 이내인 경우
                if (dist <= attackDist)
                {
                    //감지 범위 안에 위치할시
                    if (DetectPlayer())
                    {
                        actionTemp = EnemyAction.Attack;
                    }
                    else
                    {
                        actionTemp = EnemyAction.Trace;
                    }
                }
                //추적 사정거리 이내인 경우
                else if (dist <= traceDist)
                {
                    //감지 범위 안에 위치할시
                    if (DetectPlayer())
                    {
                        actionTemp = EnemyAction.Trace;
                    }
                    else if (actionTemp == EnemyAction.Attack || actionTemp == EnemyAction.Trace)
                    {
                        actionTemp = EnemyAction.Trace;
                    }
                    else
                    {
                        actionTemp = EnemyAction.Patrol;
                    }
                }
                else
                {
                    actionTemp = EnemyAction.Patrol;
                }
            }
            else
            {
                actionTemp = EnemyAction.Idle;
            }

            if(isHit==true)
            {
                action = EnemyAction.Hit;
                isHit = false;
                ActionSetting();
            }
            //해당 몬스터의 상태가 변한 경우 상태를 설정해주기 위한 함수를 실행한다.
            else if (actionTemp != action)
            {
                action = actionTemp;
                ActionSetting();
            }

            yield return new WaitForSeconds(0.3f);
        }

        if(isDie)
        {
            action = EnemyAction.Die;
            ActionSetting();
        }
    }

    public void ActionSetting()
    {
        //액션을 정의하는 함수이다.
        switch (action)
        {
            case EnemyAction.Patrol:
                _enemyMovement.Patrolling = true;
                _enemyMovement.Speed = _enemyState.patrolMoveSpeed;
                _enemyAni.SetBool(hashMove, true);
                _enemyAni.SetBool(hashAttack, false);
                _enemyAni.SetBool(hashHit, false);
                _enemyAni.SetFloat(hashSpeed, _enemyState.patrolMoveSpeed);
                break;
            case EnemyAction.Trace:
                _enemyMovement.Traceing = true;
                _enemyMovement.Speed = _enemyState.traceMoveSpeed;
                _enemyMovement.traceTarget = targetCharacter;
                _enemyAni.SetBool(hashMove, true);
                _enemyAni.SetBool(hashAttack, false);
                _enemyAni.SetBool(hashHit, false);
                _enemyAni.SetFloat(hashSpeed, _enemyState.traceMoveSpeed);
                break;
            case EnemyAction.Attack:
                _enemyMovement.IsAttack = true;
                _enemyMovement.traceTarget = targetCharacter;
                _enemyAni.SetBool(hashMove, false);
                _enemyAni.SetBool(hashAttack, true);
                _enemyAni.SetBool(hashHit, false);
                _enemyAni.SetFloat(hashSpeed, 0.0f);
                break;
            case EnemyAction.Idle:
                _enemyMovement.Idle = true;
                _enemyAni.SetBool(hashMove, false);
                _enemyAni.SetBool(hashAttack, false);
                _enemyAni.SetBool(hashHit, false);
                _enemyAni.SetBool(hashDeath, false);
                _enemyAni.SetFloat(hashSpeed, 0.0f);
                break;
            case EnemyAction.Hit:
                _enemyMovement.Stop();
                _enemyAni.SetBool(hashMove, false);
                _enemyAni.SetBool(hashAttack, false);
                _enemyAni.SetBool(hashHit, true);
                _enemyAni.SetBool(hashDeath, false);
                _enemyAni.SetFloat(hashSpeed, 0.0f);
                break;
            case EnemyAction.Die:
                isDie = true;
                _enemyMovement.Stop();
                _enemyAni.SetBool(hashMove, false);
                _enemyAni.SetBool(hashAttack, false);
                _enemyAni.SetBool(hashHit, false);
                _enemyAni.SetBool(hashDeath, true);
                _enemyAni.SetFloat(hashSpeed, 0.0f);

                StartCoroutine(IEDeath());
                break;
        }
    }

    //가장 가까운 거리의 캐릭터를 탐색해 타겟으로 정하고 몬스터와의 거리를 리턴해주는 함수
    float SearchMinDistanceCharacter()
    {
        int num = -1;
        float minDist = 0.0f, dist = 0.0f;

        //가장 가까운 캐릭터를 검색한다
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Characters[i].activeSelf != true) continue;

            dist = Vector3.Distance(Characters[i].transform.position, _enemyTrans.position);

            if (num == -1 || dist < minDist)
            {
                minDist = dist;
                num = i;
            }
        }

        //검색 결과를 저장한다
        if (num == -1)
        {
            targetCharacter = null;
        }
        else
        {
            targetCharacter = Characters[num];
        }

        return dist;
    }

    //주어진 각도에 의해 원주 위의 점의 좌푯값을 계산하는 함수
    public Vector3 CirclePoint(float angle)
    {
        //로컬 좌표계를 기준으로 설정하기 위해 적 캐릭터의 y회전값을 더함
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    //적이 시야각 안에 있는지를 판단하는 함수
    public bool DetectPlayer()
    {
        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (targetCharacter.transform.position - _enemyTrans.position).normalized;

        //적 캐릭터의 시야각에 들어왔는지 판단
        if (Vector3.Angle(_enemyTrans.forward, dir) < viewAngle * 0.5f)
            return true;
        else
            return false;
    }

    IEnumerator IEDeath()
    {
        DropItemManager.instance.DropItemSetting(_enemyState.id, _enemyTrans);
        PlayerManager.instance.Exp+=_enemyState.exp;
        QuestManager.instance.updateQuest(EQuestKind.EQuestKind_Hunting, _enemyState.id);
        yield return new WaitForSeconds(3.0f);
        this.gameObject.SetActive(false);
        _enemyAni.SetBool(hashDeath, false);
        PoolManager.instance.EnableEnemy(_enemyState.id);
    }
}


