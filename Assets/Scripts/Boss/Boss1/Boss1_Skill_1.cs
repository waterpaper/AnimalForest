using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill_1 : MonoBehaviour
{
    public float Atk { get; private set; }
    public float Factor { get; private set; }
    public float Range { get; private set; }
    public bool IsAttacking { get; private set; }

    public GameObject attackColliderObject;

    private void Awake()
    {
        attackColliderObject = transform.GetChild(0).gameObject;
    }
    private void OnEnable()
    {
        IsAttacking = true;
        attackColliderObject.SetActive(true);
        StartCoroutine(AttackDisable());
    }

    public void Setting(float bossAtk, float factor, float range)
    {
        Factor = factor;
        Atk = bossAtk;
        Range = range;

        attackColliderObject.GetComponent<BossAttackCollider>().Setting(bossAtk * factor, range);
    }

    IEnumerator AttackDisable()
    {
        yield return new WaitForSeconds(1.5f);
        IsAttacking = false;
    }
}
