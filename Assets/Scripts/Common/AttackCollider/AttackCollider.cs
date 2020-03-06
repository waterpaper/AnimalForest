using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    //공격 콜라이더를 제어하는 클래스입니다.
    public float Damage { get; private set; }
    public BoxCollider attackCollider;

    private void Awake()
    {
        attackCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        attackCollider.enabled = true;
    }

    //콜라이더와 접근시 데미지를 세팅하는 함수입니다.
    public void Setting(float damageTemp)
    {
        Damage = damageTemp;
    }

    public void Setting(float damageTemp, float range)
    {
        Damage = damageTemp;
        attackCollider.size.Set(attackCollider.size.x, attackCollider.size.y, range);
    }

}
