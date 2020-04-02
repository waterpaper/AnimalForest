using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //현재 초기 생성하는 중인지 판단하는 변수입니다.
    private bool _isSetting = false;

    private Transform _enemyTransform;
    private Rigidbody _enemyRigidbody;

    //naviMeshAgent 컴포넌트를 저장할 변수입니다.
    private NavMeshAgent agent;

    //회전할때의 속도를 조절하는 계수입니다.
    private float _damping = 5.0f;

    //다음 순찰 지점을 지정하는 변수입니다.
    public int _nextIdx = 0;

    public GameObject traceTarget;
    public GameObject AttackRange;
    //순찰지점들을 저장하기 위한 list타입 변수입니다.
    public List<Transform> wayPoints;

    //공격 상태를 판단하는 프로퍼티입니다.
    private bool _isAttack = false;
    public bool IsAttack
    {
        get { return _isAttack; }
        set
        {
            Stop();
            _isAttack = value;
        }
    }
    //순찰 상태를 판단하는 프로퍼티입니다.
    private bool _patrolling;
    public bool Patrolling
    {
        get { return _patrolling; }
        set
        {
            Stop();
            _patrolling = value;
        }
    }
    //추적을 판단하는 프로퍼티입니다.
    private bool _traceing;
    public bool Traceing
    {
        get { return _traceing; }
        set
        {
            Stop();
            _traceing = value;

            //다음 목적지의 배열 첨자를 계산
            _nextIdx = Random.Range(0, wayPoints.Count);

            //다음 목적지로 이동명령을 수행
            StartCoroutine(IENextMovePositionSetting());
        }
    }

    //대기상태를 판단하는 프로퍼티입니다.
    private bool _idle;
    public bool Idle
    {
        get { return _idle; }
        set
        {
            Stop();
            _idle = value;
        }
    }

    //움직이는 스피드를 조절하는 프로퍼티입니다.
    private float _speed = 0.0f;
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }

    void Awake()
    {
        _enemyTransform = GetComponent<Transform>();
        _enemyRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;
        //자동으로 회전하는 기능을 비활성화
        agent.updateRotation = false;
    }

    private void OnEnable()
    {
        var t = GameObject.FindGameObjectsWithTag("WayPoint");

        for (int i = 0; i < t.Length; i++)
        {
            wayPoints.Add(t[i].transform);
        }
    }

    private void OnDisable()
    {
        if (agent != null)
        {
            agent.enabled = false;
        }

        wayPoints.Clear();

        //초기 생성이 끝낫으면 변수를 다시 false해줍니다.
        if (_isSetting == false)
        {
            _isSetting = true;
        }

        Stop();
    }

    private void FixedUpdate()
    {
        if (agent.enabled == true)
        {
            EnemyRotation();

            agent.speed = _speed;

            if (_patrolling)
            {
                MoveWayPoint();
            }
            else if (_traceing)
            {
                TraceTarget();
            }
            else if (_isAttack)
            {
                StartCoroutine("Attack");
            }
        }
    }

    void MoveWayPoint()
    {
        Move();
    }

    void TraceTarget()
    {
        if (agent.isPathStale) return;

        agent.destination = traceTarget.transform.position;
        //내비게이션 기능을 활성화 해서 이동을 시작함
        agent.isStopped = false;
    }

    void EnemyRotation()
    {
        if (agent.isStopped == false)
        {
            //NavMeshAgent가 가야할 방향벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);

            //보간 함수를 사용해 점진적으로 회전시킴
            _enemyTransform.rotation = Quaternion.Slerp(_enemyTransform.rotation, rot, Time.deltaTime * _damping);
            _enemyTransform.GetChild(0).transform.localPosition = Vector3.zero;
        }
    }

    public void Stop()
    {
        _patrolling = false;
        _traceing = false;
        _isAttack = false;
        _idle = false;
        Speed = 0.0f;

        if (agent!=null && agent.enabled)
            agent.isStopped = true;
    }

    public void Move()
    {
        if (agent.enabled && agent.isStopped == true)
        {
            //내비게이션 기능을 활성화 해서 이동을 시작함
            _idle = false;
            StartCoroutine("IENextMovePositionSetting");
        }

        //navMeshAgent가 이동하고 있고 목적지에 도착햇는지 여부를 계산합니다.
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 1.0f)
        {
            //다음 목적지의 배열 첨자를 계산
            _nextIdx = Random.Range(0, wayPoints.Count);
            //다음 목적지로 이동명령을 수행
            StartCoroutine("IENextMovePositionSetting");
        }
    }

    IEnumerator IENextMovePositionSetting()
    {
        if (gameObject.activeSelf == true)
        {
            //최단거리 경로 계산이 끝나지 않았다면 다음을 수행하지 않음
            if (agent.isPathStale) yield return null;
            //다음 목적지를 wayPoints 배열에서 추출한 위치로 다음 목적지를 지정
            agent.destination = wayPoints[_nextIdx].position;
            //내비게이션 기능을 활성화 해서 이동을 시작함
            agent.isStopped = false;
        }

        yield return null;
    }

    IEnumerator Attack()
    {
        if (gameObject.activeSelf == true)
        {
            _isAttack = false;
            //enemyAttackRect.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            //enemyAttackRect.SetActive(false);
            _enemyTransform.LookAt(traceTarget.transform, _enemyTransform.up);
            _enemyTransform.GetChild(0).transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _enemyRigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground")
        {
            if (agent != null && _isSetting == true)
                agent.enabled = true;
            _enemyRigidbody.velocity = new Vector3(0.0f, -1.0f, 0.0f);
            agent.isStopped = true;
        }
    }
}
