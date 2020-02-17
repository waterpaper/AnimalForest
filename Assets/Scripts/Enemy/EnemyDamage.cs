using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    private int beforDamageNumber = -1;
    //생명 게이지 프리팹을 저장할 변수
    public GameObject hpBarPrefabs;
    //생명 게이지의 위치를 보정할 오프셋
    public Vector3 hpBarOffset = new Vector3(0, 1.5f, 0);
    //부모가 될 canvas 객체
    private Canvas uiCanvas;
    //생명력 바를 가지고 있는 게임 오브젝트입니다.
    private GameObject hpBar = null;
    //생명 수치에 따라 fillAmount속성을 변경할 image
    private Image hpBarImage;
    //생명력을 갖고 있는 에너미 상태 컴포넌트를 가져옵니다.
    private EnemyStatement _enemyStatement;
    //텍스트를 띄우는 ui를 저정합니다.
    public GameObject TextMessageUIPrefab;
    //맞는 이펙트를 출력할때 위치를 보정해주는 오프셋입니다.
    public Vector3 hitEffectOffset = new Vector3(0, 0.5f, 0);


    // Start is called before the first frame update
    void Awake()
    {
        _enemyStatement = GetComponentInParent<EnemyStatement>();
        uiCanvas = null;
        uiCanvas = GameObject.Find("GameUI").GetComponent<Canvas>();
        //생명게이지의 생성 및 초기화
        SetHpBar();
    }

    private void OnEnable()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void OnDisable()
    {
        if (hpBar != null)
            hpBar.SetActive(false);
        hpBarImage.fillAmount = 1.0f;
        beforDamageNumber = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            if (hpBar.activeSelf == false)
            {
                hpBar.SetActive(true);
                hpBar.gameObject.GetComponent<EnemyHpBar>().EnemyInfoSetting(_enemyStatement.name, _enemyStatement.level);
            }

            if (beforDamageNumber == other.gameObject.GetComponent<PlayerAttackRect>().attackNumber)
                return;

            beforDamageNumber = other.gameObject.GetComponent<PlayerAttackRect>().attackNumber;

            //생명 게이지 차감
            _enemyStatement.hp -= other.gameObject.GetComponent<PlayerAttackRect>().Damage;
            //생명 게이지의 fillamount속성을 변경
            hpBarImage.fillAmount = _enemyStatement.hp / _enemyStatement.hpMax;
            if (_enemyStatement.hp <= 0.0f)
            {
                //적 캐릭터의 상태를 DIE로 변경
                GetComponentInParent<EnemyAi>().isDie = true;
                //적 캐릭터가 사망한 이후 생명 게이지를 투명처리
                hpBar.SetActive(false);

                //capsule Collider컴포넌트를 비활성화
                GetComponent<CapsuleCollider>().enabled = false;

                //데미지 ui를 출력합니다
                var ExpUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
                ExpUITemp.name = "Exp";
                ExpUITemp.GetComponent<TextMessageUI>().Setting(string.Format("Exp + {00}", _enemyStatement.exp), new Color(255, 255, 255), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);
            }
            else
            {
                //적 캐릭터의 상태를 Hit로 변경
                GetComponentInParent<EnemyAi>().isHit = true;
            }

            //데미지 ui를 출력합니다
            var damageUITemp = Instantiate<GameObject>(TextMessageUIPrefab, uiCanvas.transform);
            damageUITemp.name = "Damage";
            damageUITemp.GetComponent<TextMessageUI>().Setting(other.gameObject.GetComponent<PlayerAttackRect>().Damage.ToString(), new Color(128, 128, 0), gameObject.transform.position, new Vector3(0.0f, 0.5f, 0.0f), uiCanvas);

            StartCoroutine(ActiveHitParticle());
        }
    }

    void SetHpBar()
    {
        if (uiCanvas != null)
        {
            //uicanvas 하위로 생명 게이지를 생성
            hpBar = Instantiate<GameObject>(hpBarPrefabs, uiCanvas.transform);
            hpBar.name = gameObject.transform.parent.name + "HpBar";
            //fillamount 속성을 변경할 image를 추출
            hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

            //생명게이지가 따라가야 할 대상과 오프셋 값 설정
            var _hpBar = hpBar.GetComponent<EnemyHpBar>();
            _hpBar.targetTr = gameObject.transform;
            _hpBar.offset = hpBarOffset;
        }
    }

    IEnumerator ActiveHitParticle()
    {
        ParticleManager.instance.Play(ParticleName.ParticleName_Battle_EnemyHit, transform, hitEffectOffset);
        yield return new WaitForSeconds(2.0f);
    }
}
