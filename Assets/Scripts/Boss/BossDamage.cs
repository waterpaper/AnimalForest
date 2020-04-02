using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossDamage : MonoBehaviour
{
    //컴포넌트를 지정합니다.
    private BossStatement _bossStatement;
    private BossAI _bossAI;
    private CapsuleCollider _bossHitCollider;

    //직전 플레이어 공격 번호입니다.
    private int _beforDamageNumber;

    //맞는 이펙트를 출력할때 위치를 보정해주는 오프셋입니다.
    public Vector3 hitEffectOffset = new Vector3(0, 0.5f, 0);

    private void Awake()
    {
        _bossStatement = GetComponentInParent<BossStatement>();
        _bossAI = GetComponentInParent<BossAI>();
        _bossHitCollider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        //변수들을 초기화합니다.
        _bossHitCollider.enabled = true;
        _beforDamageNumber = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            Hit(other.gameObject.GetComponent<PlayerAttackCollider>());
        }
    }

    private void Hit(PlayerAttackCollider playerAttackColliderTemp)
    {
        //같은 공격 상태이면 데미지 처리를 하지 않습니다.
        if (_beforDamageNumber == playerAttackColliderTemp.attackNumber) return;
        else
            _beforDamageNumber = playerAttackColliderTemp.attackNumber;

        //공격력에 맞는 데미지를 감소시킵니다.
        _bossStatement.hp -= (int)playerAttackColliderTemp.Damage;

        if (_bossStatement.hp <= 0.0f)
            Death();

        //데미지 ui를 출력합니다
        UIManager.instance.PrintGameText(playerAttackColliderTemp.Damage.ToString(), gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f), new Color(128, 128, 0));

        StartCoroutine(IEHitEffect());
    }

    private void Death()
    {
        //boss 사망을 처리합니다.
        _bossAI.IsDeath = true;
        _bossHitCollider.enabled = false;

        //경험치 ui를 출력합니다
        UIManager.instance.PrintGameText(string.Format("Exp + {00}", _bossStatement.exp), gameObject.transform, new Vector3(0.0f, 0.5f, 0.0f), new Color(255, 255, 255));
    }

    IEnumerator IEHitEffect()
    {
        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Hit);
        ParticleManager.instance.Play(ParticleName.ParticleName_Battle_Boss1Hit, transform, hitEffectOffset);
        yield return new WaitForSeconds(2.0f);
    }
}
