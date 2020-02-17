using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill3_Chlid : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _ani;
    public BossAttackCollider _collider;

    private Vector3 startPosition;
    private float _speed;
    private float _range;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ani = GetComponent<Animator>();
        _collider = GetComponent<BossAttackCollider>();
    }

    public void Setting(float damage, float range, float speed)
    {
        _speed = speed;
        _range = range;
        _collider.Setting(damage,1.0f);
        startPosition = transform.position;

        _ani.SetFloat("Speed", _speed);
        _ani.SetBool("IsMove", true);

        _rigidbody.AddForce(transform.forward * _speed);
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(startPosition, transform.position) > _range)
        {
            Destroy(this.gameObject);
        }
    }
}
