using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill2_Chlid : MonoBehaviour
{
    //내부 컴포넌트입니다.
    private Rigidbody _rigidbody;
    private Animator _ani;
    public AttackCollider _collider;

    //파괴위치 설정을 위한 초기 위치입니다.
    private Vector3 startPosition;
    private float _speed;
    private float _range;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ani = GetComponent<Animator>();
        _collider = GetComponent<AttackCollider>();
    }

    public void Setting(float damage, float range, float speed)
    {
        //데미지와 관련있는 변수를 세팅하며 콜라이더에 데미지를 설정해주는 함수입니다.
        _speed = speed;
        _range = range;
        _collider.Setting(damage, 1.0f);
        startPosition = transform.position;

        _ani.SetFloat("Speed", _speed);
        _ani.SetBool("IsMove", true);

        _rigidbody.AddForce(transform.forward * _speed);
    }

    private void FixedUpdate()
    {
        //일정범위를 지나면 객체가 파괴되게 설정합니다.
        if(Vector3.Distance(startPosition, transform.position) > _range)
        {
            Destroy(this.gameObject);
        }
    }
}
