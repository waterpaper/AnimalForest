using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1_Skill_1 : MonoBehaviour
{
    //보스 1의 스킬 1을 처리하는 클래스입니다.
    public float Atk { get; private set; }
    public float Factor { get; private set; }
    public float Range { get; private set; }

    public bool IsAttacking { get; private set; }
    public bool isGround { get; set; }

    //위에서 빛을 나타내면서 보스의 범위를 나타내는 프로젝터입니다.
    public GameObject attackRectProjector;
    public NavMeshAgent navAgent = null;

    //공격 콜라이더입니다. 콜라이더의 충돌여부로 플레이어에게 공격여부를 판단합니다.
    public AttackCollider attackCollider;

    private void Awake()
    {
        attackCollider = transform.GetChild(0).GetComponent<AttackCollider>();
        attackRectProjector = transform.GetChild(1).gameObject;
        navAgent = gameObject.GetComponentInParent<NavMeshAgent>();

        if (navAgent != null)
            navAgent.enabled = false;
    }

    private void OnEnable()
    {
        StartCoroutine(StartSkill());
    }

    public void Setting(float bossAtk, float factor, float range)
    {
        //데미지와 관련있는 변수를 세팅하며 콜라이더에 데미지를 설정해주는 함수입니다.
        Factor = factor;
        Atk = bossAtk;
        Range = range;

        attackCollider.Setting(bossAtk * factor);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //땅에 접근했을때 공격을 시작하기 위한 함수입니다.
        if (other.tag == "Ground" && IsAttacking)
        {
            isGround = true;

            if (navAgent != null)
            {
                //navagent를 재실행합니다.
                navAgent.enabled = true;
                navAgent.isStopped = true;
            }

            attackRectProjector.SetActive(false);
            attackCollider.gameObject.SetActive(true);
            StartCoroutine(DisableSkill());
            SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Skill1_Down);
        }
        else if(other.tag == "Player")
        {
            PlayerManager.instance.Knockback(transform.position);

            if (navAgent != null)
            {
                //navagent를 재실행합니다.
                navAgent.enabled = true;
                navAgent.isStopped = true;
            }
        }
    }

    IEnumerator StartSkill()
    {
        //스킬 시작시 범위 프로젝터를 켜주고 코루틴 함수를 실행합니다.
        attackRectProjector.SetActive(true);
        navAgent.enabled = false;

        attackCollider.gameObject.SetActive(false);

        //점프작용을 입력합니다.
        gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0.0f, 10.0f, 0.0f);

        //공격상태로 바꿔줍니다.
        yield return new WaitForSeconds(0.3f);
      
        isGround = false;
        IsAttacking = true;
    }

    IEnumerator DisableSkill()
    {
        //공격상태를 종료합니다.
        yield return new WaitForSeconds(0.5f);
        IsAttacking = false;
        gameObject.SetActive(false);
    }
}
