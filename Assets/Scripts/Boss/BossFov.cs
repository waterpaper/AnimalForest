using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFov : MonoBehaviour
{
    //적 캐릭터의 추적 사정 거리의 범위
    public float detectRange;
    
    //적 캐릭터의 시야각
    [Range(0, 360)]
    public float viewAngle = 120f;

    private Transform _bossTr;
    private int _playerLayer;
    private int _layerMask;

    public void Awake()
    {
        //컴포넌트 추출
        _bossTr = GetComponent<Transform>();

        //레이어 마스크 값 계산
        _playerLayer = LayerMask.NameToLayer("PLAYER");
        _layerMask = 1 << _playerLayer;
    }

    //시야각에 플레이어가 들어왓는지 확인하는 함수입니다.
    public GameObject tracePlayer()
    {
        if (_layerMask == 0) return null;

        int index = 0;
        float minDist = detectRange;
        bool isTrace = false;

        //추적 반경 범위 안에서 주인공 캐릭터를 추출
        Collider[] colls = Physics.OverlapSphere(_bossTr.position, viewAngle, 1 << _playerLayer);

        //배열의 개수가 1 이상일때 오브젝트 범위 안에 있다고 판단하고 검사후 가장 가까운 플레이어 오브젝트를 반환합니다.
        for (int i = 0; i < colls.Length; i++)
        {
            //적 캐릭터와 주인공 사이의 방향 벡터를 계산
            Vector3 dir = (colls[i].gameObject.transform.position - _bossTr.position).normalized;

            //적 캐릭터의 시야각에 들어왔는지 판단
            if (Vector3.Angle(_bossTr.forward, dir) < viewAngle * 0.5f)
            {
                //플레이어와 적 캐릭터 간의 거리를 계산
                float dist = Vector3.Distance(colls[i].gameObject.transform.position, _bossTr.position);

                //최소거리의 인덱스를 저장합니다.
                if (minDist > dist)
                {
                    isTrace = true;
                    index = i;
                }
            }
        }

        if (isTrace == true)
            return colls[index].gameObject;
        else
            return null;
    }

    //플레이어를 바라보앗을때 장애물의 여부를 판단하는 함수입니다.
    public bool isViewPlayer(Transform playerTr)
    {
        bool isView = false;
        RaycastHit hit;

        //적 캐릭터와 주인공 사이의 방향 벡터를 계산
        Vector3 dir = (playerTr.position - _bossTr.position).normalized;

        //레이캐스트를 투사해서 장애물이 잇는지 여부를 판단
        if (Physics.Raycast(_bossTr.position, dir, out hit, detectRange, _layerMask))
        {
            isView = (hit.collider.CompareTag("Player"));
        }

        return isView;
    }

    //주어진 각도에 의해 원주 위의 점의 좌푯값을 계산하는 함수
    public Vector3 CirclePoint(float angle)
    {
        //로컬 좌표계를 기준으로 설정하기 위해 적 캐릭터의 y회전값을 더함
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    //보스의 탐지거리를 세팅하는 함수입니다.
    public void Setting(float bossDetectRange)
    {
        detectRange = bossDetectRange;
    }
}
