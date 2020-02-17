using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1_Skill_2 : MonoBehaviour
{
    public float Atk { get; private set; }
    public float Factor { get; private set; }
    public float Range { get; private set; }

    public bool IsAttacking { get; private set; }
    public bool IsAttackStart { get; private set; }
    public bool isGround { get; set; }

    public GameObject attackColliderObject;
    public GameObject attackRectProjector;
    public NavMeshAgent navAgent = null;

    private void Awake()
    {
        attackColliderObject = transform.GetChild(0).gameObject;
        attackRectProjector = transform.GetChild(1).gameObject;
        attackColliderObject.GetComponent<BoxCollider>().enabled = false;
        navAgent = gameObject.GetComponentInParent<NavMeshAgent>();
    }

    public void Setting(float bossAtk, float factor, float range)
    {
        Factor = factor;
        Atk = bossAtk;
        Range = range;

        attackColliderObject.GetComponent<BossAttackCollider>().Setting(bossAtk * factor);
    }

    private void OnEnable()
    {
        if(navAgent != null)
            navAgent.enabled = false;

        attackRectProjector.SetActive(true);
        attackColliderObject.SetActive(true);
        gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0.0f, 10.0f, 0.0f);
        isGround = false;
        IsAttacking = true;
        IsAttackStart = false;

        StartCoroutine(Attack());
    }

    private void OnDisable()
    {
        attackColliderObject.GetComponent<BoxCollider>().enabled = false;
    }

    IEnumerator Attack()
    {
        if(isGround == false)
        {
            yield return new WaitForSeconds(0.3f);
        };

        attackColliderObject.GetComponent<BoxCollider>().enabled = true;
        IsAttackStart = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground" && IsAttackStart == true)
        {
            isGround = true;

            if (navAgent != null)
            {
                navAgent.enabled = true;
                navAgent.isStopped = true;
            }

            attackRectProjector.SetActive(false);
            StartCoroutine(AttackDisable());
        }
        else if(other.tag == "Player")
        {
            PlayerManager.instance.Knockback(transform.position);
        }
    }

    IEnumerator AttackDisable()
    {
        yield return new WaitForSeconds(2.0f);
        IsAttacking = false;
    }
}
