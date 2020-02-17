using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    private Transform _bossTransform;
    private Rigidbody _bossRigidbody;

    //naviMeshAgent 컴포넌트를 저장할 변수입니다.
    private NavMeshAgent agent = null;
    //회전할때의 속도를 조절하는 계수입니다.
    private float _damping = 5.0f;
    //다음 순찰 지점을 지정하는 변수입니다.
    private int _nextIdx = 0;
    //추적할 대상을 가지고 있는 변수입니다.
    public GameObject traceTarget;
    //순찰지점들을 저장하기 위한 list타입 변수입니다.
    public List<Transform> wayPoints;

    //현재 초기 생성하는 중인지 판단하는 변수입니다.
    private bool _isSetting = false;

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
            StartCoroutine("IENextMovePositionSetting");
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


    private void Awake()
    {
        _bossTransform = GetComponent<Transform>();
        _bossRigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        //목적지에 가까워질수록 속도를 줄이는 옵션을 비활성화
        agent.autoBraking = false;
        //자동으로 회전하는 기능을 비활성화
        agent.updateRotation = false;
    }

    private void OnEnable()
    {
        if (agent != null && _isSetting == true)
        {
            agent.enabled = true;
            agent.isStopped = true;
        }

        var t = GameObject.FindGameObjectsWithTag("WayPoint");

        for (int i = 0; i < t.Length; i++)
        {
            wayPoints.Add(t[i].transform);
        }
    }

    private void OnDisable()
    {
        wayPoints.Clear();

        if (agent != null)
        {
            Stop();
            agent.enabled = false;
        }

        //초기 생성이 끝낫으면 변수를 다시 false해줍니다.
        if (_isSetting == false)
        {
            _isSetting = true;
        }
    }

    private void FixedUpdate()
    {
        BossRotation();

        agent.speed = _speed;

        if (_patrolling)
        {
            Move();
        }
        else if (_traceing)
        {
            TraceTarget();
        }
    }

    void BossRotation()
    {
        if (agent.enabled == true && agent.isStopped == false)
        {
            //NavMeshAgent가 가야할 방향벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);

            //보간 함수를 사용해 점진적으로 회전시킴
            _bossTransform.rotation = Quaternion.Slerp(_bossTransform.rotation, rot, Time.deltaTime * _damping);
        }
    }

    void TraceTarget()
    {
        if (agent.isPathStale) return;

        agent.destination = traceTarget.transform.position;
        //내비게이션 기능을 활성화 해서 이동을 시작함
        agent.isStopped = false;

    }

    public void Stop()
    {
        _patrolling = false;
        _traceing = false;
        _idle = false;
        Speed = 0.0f;

        if (agent != null && agent.enabled == true && _isSetting ==true && gameObject.activeSelf == true)
            agent.isStopped = true;
    }

    public void Move()
    {
        if (agent.isStopped == true)
        {
            //내비게이션 기능을 활성화 해서 이동을 시작함
            _idle = false;
            agent.isStopped = false;
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
}
