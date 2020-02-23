using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossDamage : MonoBehaviour
{
    private int beforDamageNumber = -1;
    //생명 게이지 프리팹을 저장할 변수
    public BossStatment _bossStatement;

    //부모가 될 canvas 객체
    private Canvas uiCanvas;
    //데미지를 입엇을시 띄우는 ui를 저정합니다.
    public GameObject TextMessageUIPrefab;
    //맞는 이펙트를 출력할때 위치를 보정해주는 오프셋입니다.
    public Vector3 hitEffectOffset = new Vector3(0, 0.5f, 0);

    private void Awake()
    {
        _bossStatement = GetComponentInParent<BossStatment>();
    }

    private void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        uiCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        beforDamageNumber = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            if (beforDamageNumber == other.gameObject.GetComponent<PlayerAttackRect>().attackNumber)
                return;

            beforDamageNumber = other.gameObject.GetComponent<PlayerAttackRect>().attackNumber;

            //생명 게이지 차감
            _bossStatement.hp -= (int)other.gameObject.GetComponent<PlayerAttackRect>().Damage;
          
            if (_bossStatement.hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponentInParent<BossAI>().isDeath = true;
                GetComponentInParent<BossAI>().action = BossAI.BossAction.Die;

                //capsule Collider컴포넌트를 비활성화
                GetComponent<CapsuleCollider>().enabled = false;

                //데미지 ui를 출력합니다
                var ExpUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
                ExpUITemp.name = "Exp";
                ExpUITemp.GetComponent<TextMessageUI>().Setting(string.Format("Exp + {00}", _bossStatement.exp), new Color(255, 255, 255), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);
            }

            //데미지 ui를 출력합니다
            var damageUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
            damageUITemp.name = "Damage";
            damageUITemp.GetComponent<TextMessageUI>().Setting(other.gameObject.GetComponent<PlayerAttackRect>().Damage.ToString(), new Color(128, 128, 0), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);
            
            StartCoroutine(IEHitParticle());
            SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_Boss1_Hit);
        }
    }

    IEnumerator IEHitParticle()
    {
        ParticleManager.instance.Play(ParticleName.ParticleName_Battle_Boss1Hit, transform, hitEffectOffset);
        yield return new WaitForSeconds(2.0f);
    }
}
