using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    //한번 공격에 연속 처리를 하지 않기 위해 전 공격의 인덱스값을 저장합니다.
    private int beforDamageNumber = -1;

    //컴포넌트를 가져옵니다.
    private EnemyStatement _enemyStatement;
    private EnemyAi _enemyAI;
    private CapsuleCollider _enemyHitCollider;
    private EnemyHpBar _enemyHpBar;
    
    //생명력 바를 가지고 있는 게임 오브젝트입니다.
    private GameObject enemyhpBarObject = null;

    //생명 게이지 프리팹을 저장할 변수
    public GameObject hpBarPrefabs;
    //생명 게이지의 위치를 보정할 오프셋
    public Vector3 hpBarOffset = new Vector3(0, 1.5f, 0);
    //부모가 될 canvas 객체
    private Canvas uiCanvas;
    //생명 수치에 따라 fillAmount속성을 변경할 image
    private Image hpBarImage;
    //텍스트를 띄우는 ui를 저정합니다.
    public GameObject TextMessageUIPrefab;
    //맞는 이펙트를 출력할때 위치를 보정해주는 오프셋입니다.
    public Vector3 hitEffectOffset = new Vector3(0, 0.5f, 0);

    
    void Awake()
    {
        _enemyStatement = GetComponentInParent<EnemyStatement>();
        _enemyAI = GetComponentInParent<EnemyAi>();
        _enemyHitCollider = GetComponent<CapsuleCollider>();

        uiCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();

        //생명게이지의 생성 및 초기화
        SetHpBar();
    }

    private void OnEnable()
    {
        _enemyHitCollider.enabled = true;
    }

    private void OnDisable()
    {
        if (enemyhpBarObject != null)
            enemyhpBarObject.SetActive(false);
        hpBarImage.fillAmount = 1.0f;
        beforDamageNumber = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            PlayerAttackCollider playerAttackColliderTemp = other.gameObject.GetComponent<PlayerAttackCollider>();
            
            //같은 공격 상태이면 데미지 처리를 하지 않습니다.
            if (beforDamageNumber == playerAttackColliderTemp.attackNumber)
                return;

            beforDamageNumber = playerAttackColliderTemp.attackNumber;

            //hp바를 화면에 출력합니다.
            if (enemyhpBarObject.activeSelf == false)
            {
                enemyhpBarObject.SetActive(true);
                _enemyHpBar.EnemyInfoSetting(_enemyStatement.name, _enemyStatement.level);
            }
      
            //생명 게이지 차감
            _enemyStatement.hp -= playerAttackColliderTemp.Damage;
            //생명 게이지의 fillamount속성을 변경
            hpBarImage.fillAmount = _enemyStatement.hp / _enemyStatement.hpMax;
            if (_enemyStatement.hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                _enemyAI.isDie = true;
                //적 캐릭터가 사망한 이후 생명 게이지를 투명처리
                enemyhpBarObject.SetActive(false);

                //capsule Collider컴포넌트를 비활성화
                _enemyHitCollider.enabled = false;

                //데미지 ui를 출력합니다
                var ExpUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
                ExpUITemp.name = "Exp";
                ExpUITemp.GetComponent<TextMessageUI>().Setting(string.Format("Exp + {00}", _enemyStatement.exp), new Color(255, 255, 255), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);
            }
            else
            {
                //적 캐릭터의 상태를 Hit로 변경
                _enemyAI.isHit = true;
            }

            //데미지 ui를 출력합니다
            var damageUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
            damageUITemp.name = "Damage";
            damageUITemp.GetComponent<TextMessageUI>().Setting(playerAttackColliderTemp.Damage.ToString(), new Color(128, 128, 0), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);

            StartCoroutine(ActiveHitParticle());
        }
    }

    void SetHpBar()
    {
        if (uiCanvas != null)
        {
            //uicanvas 하위로 생명 게이지를 생성
            enemyhpBarObject = Instantiate<GameObject>(hpBarPrefabs, uiCanvas.transform);
            enemyhpBarObject.name = gameObject.transform.parent.name + "HpBar";
            //fillamount 속성을 변경할 image를 추출
            hpBarImage = enemyhpBarObject.GetComponentsInChildren<Image>()[1];

            //생명게이지가 따라가야 할 대상과 오프셋 값 설정
            _enemyHpBar = enemyhpBarObject.GetComponent<EnemyHpBar>();
            _enemyHpBar.targetTr = gameObject.transform;
            _enemyHpBar.offset = hpBarOffset;
        }
    }

    IEnumerator ActiveHitParticle()
    {
        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_EnemyHit);
        ParticleManager.instance.Play(ParticleName.ParticleName_Battle_EnemyHit, transform, hitEffectOffset);
        yield return new WaitForSeconds(2.0f);
    }
}
