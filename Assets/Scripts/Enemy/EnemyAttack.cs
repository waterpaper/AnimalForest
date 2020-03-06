using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyStatement enemyStatement;
    public GameObject attackColliderObject;
    public AttackCollider attackCollider;

    private void Start()
    {
        attackColliderObject = transform.GetChild(2).gameObject;
        enemyStatement = GetComponentInParent<EnemyStatement>();
        attackCollider = attackColliderObject.GetComponent<AttackCollider>();

        attackColliderObject.SetActive(false);
        Setting();
    }
    
    public void Setting()
    {
        //데미지와 관련있는 변수를 세팅하며 콜라이더에 데미지를 설정해주는 함수입니다.
        attackCollider.Setting(enemyStatement.atk);
    }

    public void StartAttack()
    {
        attackColliderObject.SetActive(true);
    }

    public void EndAttack()
    {
        attackColliderObject.SetActive(false);
    }
}
