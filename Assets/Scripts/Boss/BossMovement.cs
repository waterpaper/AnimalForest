using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMovement : MonoBehaviour
{
    private Transform _bossTransform;
    private Rigidbody _bossRigidbody;
    private NavMeshAgent _agent;
    private List<Transform> _wayPointList;

    [Header("Movement Attribute")]
    //추적할 대상을 가지고 있는 변수입니다.
    public GameObject traceTarget;
    //회전할때의 속도를 조절하는 계수입니다.
    public float damping = 5.0f;
    //다음 순찰 지점을 지정하는 변수입니다.
    public int nextIdx = 0;

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
            nextIdx = Random.Range(0, _wayPointList.Count);

            //다음 목적지로 이동명령을 수행
            NextMovePositionSetting();
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
        _agent = GetComponent<NavMeshAgent>();

        //옵션을 비활성화합니다.
        _agent.autoBraking = false;
        _agent.updateRotation = false;
    }

    private void OnEnable()
    {
        var t = GameObject.FindGameObjectsWithTag("WayPoint");

        for (int i = 0; i < t.Length; i++)
        {
            _wayPointList.Add(t[i].transform);
        }

        _agent.enabled = true;
        Stop();
        _agent.isStopped = true;
    }

    private void FixedUpdate()
    {
        //보스를 회전합니다.
        BossRotation();

        _agent.speed = _speed;

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
        //NavMeshAgent가 가야할 방향벡터를 쿼터니언 타입의 각도로 변환해 점진적으로 회전합니다.
        if (_agent.enabled == true && _agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(_agent.desiredVelocity);
            _bossTransform.rotation = Quaternion.Slerp(_bossTransform.rotation, rot, Time.deltaTime * damping);
        }
    }

    void TraceTarget()
    {
        //계산중일시 리턴하고 계산이 끝낫으면 목표 위치를 바꾸고 이동을 활성화합니다.
        if (_agent.isPathStale) return;

        _agent.destination = traceTarget.transform.position;
        _agent.isStopped = false;

    }

    public void Stop()
    {
        _patrolling = false;
        _traceing = false;
        _idle = false;
        Speed = 0.0f;

        if (_agent != null && _agent.enabled == true && gameObject.activeSelf == true)
            _agent.isStopped = true;
    }

    public void Move()
    {
        //내비게이션 기능을 활성화 해서 이동을 시작합니다.
        if (_agent.isStopped == true)
        {
            _idle = false;
            _agent.isStopped = false;
            NextMovePositionSetting();
        }

        //navMeshAgent가 이동하고 있고 목적지에 도착햇는지 여부를 계산합니다.
        if (_agent.velocity.sqrMagnitude >= 0.2f * 0.2f && _agent.remainingDistance <= 1.0f)
        {
            //다음 목적지로 이동명령을 수행합니다.
            nextIdx = Random.Range(0, _wayPointList.Count);
            NextMovePositionSetting();
        }
    }

    void NextMovePositionSetting()
    {
        //다음 순찰 위치를 설정해주는 함수입니다.
        if (gameObject.activeSelf == true)
        {
            //최단거리 경로 계산이 끝나지 않았다면 다음을 수행하지 않습니다.
            if (_agent.isPathStale) return;

            //다음 목적지를 wayPoints 배열에서 추출한 위치로 다음 목적지를 지정하고 활성화합니다.
            _agent.destination = _wayPointList[nextIdx].position;
            _agent.isStopped = false;
        }
    }
}
