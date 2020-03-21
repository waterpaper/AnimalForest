using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill2_Chlid : MonoBehaviour, IBossAttack
{
    //현재 스피드와 범위입니다.
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _range;

    //파괴위치 설정을 위한 초기 위치입니다.
    private Vector3 _startPosition;

    //내부 컴포넌트입니다.
    private Rigidbody _rigidbody;
    private Animator _ani;
    private AttackCollider _collider;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ani = GetComponent<Animator>();
        _collider = GetComponent<AttackCollider>();
    }

    public void Setting(float damage, float range, float speed)
    {
        //데미지와 관련있는 변수를 세팅하며 콜라이더에 데미지를 설정해주는 함수입니다.
        //이동 거리를 판단하기 위해 초기위치를 저장합니다.
        _speed = speed;
        _range = range;

        _collider.Setting(damage, 1.0f);
        StartCoroutine(StartAttack());
    }

    public IEnumerator StartAttack()
    {
        //공격을 정의하고 물리 연산을 업데이트합니다.
        _startPosition = transform.position;
        _rigidbody.AddForce(transform.forward * _speed);

        _ani.SetFloat("Speed", _speed);
        _ani.SetBool("IsMove", true);

        yield return new WaitForSeconds(2.0f);
        //일정 시간이 지난 후 공격 종료 코루틴을 호출합니다.
        StartCoroutine(DisableAttack());
    }

    public IEnumerator DisableAttack()
    {
        while(true)
        {
            //일정범위를 지나면 객체가 파괴되게 설정합니다.
            if (Vector3.Distance(_startPosition, transform.position) > _range)
            {
                Destroy(this.gameObject);
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
