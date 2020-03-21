using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss1_Skill_1 : BossSkillMono, IBossAttack
{
    //보스 1의 스킬 1을 처리하는 클래스입니다.

    //위에서 빛을 나타내면서 보스의 범위를 나타내는 프로젝터입니다.
    private GameObject _attackRectProjector;
    private NavMeshAgent _navAgent = null;
    private AttackCollider _attackCollider;

    private void Awake()
    {
        _attackRectProjector = transform.GetChild(1).gameObject;
        _attackCollider = transform.GetChild(0).GetComponent<AttackCollider>();
        _navAgent = gameObject.GetComponentInParent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        StartCoroutine(StartAttack());
    }

    public void Setting(float bossAtk, float factor, float range)
    {
        //데미지와 관련있는 변수를 세팅하며 콜라이더에 데미지를 설정해주는 함수입니다.
        Factor = factor;
        Atk = bossAtk;
        Range = range;

        _attackCollider.Setting(bossAtk * factor);
    }

    private void OnTriggerEnter(Collider other)
    {
        //땅에 접근했을때 공격을 시작하기 위한 함수입니다.
        if (other.tag == "Ground" && IsAttacking)
        {
            StartCoroutine(DisableAttack());
        }
        else if (other.tag == "Player")
        {
            PlayerManager.instance.Knockback(transform.position);

            StartCoroutine(DisableAttack());
        }
    }

    public IEnumerator StartAttack()
    {
        //스킬 시작시 범위 프로젝터를 켜주고 코루틴 함수를 실행합니다.
        _attackRectProjector.SetActive(true);
        _navAgent.enabled = false;

        //점프작용을 입력합니다.
        gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0.0f, 10.0f, 0.0f);

        //공격상태로 바꿔줍니다.
        yield return new WaitForSeconds(0.3f);

        IsGround = false;
        IsAttacking = true;

        //인식을 하지 못해 공격상태가 계속되면 오류를 제어하기 위해 공격을 종료합니다.
        yield return new WaitForSeconds(5.0f);
        if (IsAttacking)
        {
            StartCoroutine(DisableAttack());
        }
    }

    public IEnumerator DisableAttack()
    {
        //공격상태를 종료합니다.
        IsGround = true;

        //navagent를 재실행합니다.
        _navAgent.enabled = true;
        _navAgent.isStopped = true;

        _attackRectProjector.SetActive(false);
        _attackCollider.gameObject.SetActive(true);

        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Skill1_Down);

        yield return new WaitForSeconds(0.5f);

        IsAttacking = false;
        _attackCollider.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
