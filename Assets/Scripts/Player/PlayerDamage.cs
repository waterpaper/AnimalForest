using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public Canvas GameUICanvas;
    public GameObject DamageUIPrefab;

    //데미지 수치가 나타나는 위치를 보정해주는 오프셋입니다.
    public Vector3 damageOffset = new Vector3(0.0f, 0.5f, 0.0f);
    //맞는 이펙트를 출력할때 위치를 보정해주는 오프셋입니다.
    public Vector3 hitEffectOffset = new Vector3(0, 0.5f, 0);

    private void Start()
    {
        GameUICanvas = UIManager.instance.gameUI.GetComponent<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttack" || other.gameObject.tag == "BossAttack")
        { 
            var attackCollider = other.gameObject.GetComponent<AttackCollider>();

            if (attackCollider == null)
                attackCollider = other.gameObject.GetComponentInParent<AttackCollider>();

            //데미지를 계산, 처리합니다.
            float temp = (attackCollider.Damage - PlayerManager.instance.Def);
            if (temp < 0.0f) temp = 0;
            PlayerManager.instance.Hp -= temp;

            //데미지 ui를 출력합니다
            var damageUITemp = Instantiate<GameObject>(DamageUIPrefab, GameUICanvas.transform).GetComponent<TextMessageUI>();
            damageUITemp.gameObject.name = "Damage";
            damageUITemp.Setting(temp.ToString(), new Color(255,0,0), gameObject.transform.position, damageOffset, GameUICanvas);


            other.gameObject.SetActive(false);
            StartCoroutine(ActiveHitParticle());
        }
    }

    IEnumerator ActiveHitParticle()
    {
        ParticleManager.instance.Play(ParticleName.ParticleName_Battle_PlayerHit, transform, hitEffectOffset);
        yield return new WaitForSeconds(2.0f);
    }
}
