using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill_2 : BossSkillMono, IBossAttack
{
    //보스 1의 스킬 2을 처리하는 클래스입니다.
    [Header("Skill_Prefab")]
    public GameObject skill_Prefab;

    [Header("Skill_ReadyImage")]
    public GameObject skill_ReadyImage;

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
    }

    public IEnumerator StartAttack()
    {
        //범위를 띄워줍니다.
        skill_ReadyImage.SetActive(true);
        IsAttacking = true;

        yield return new WaitForSeconds(3.0f);

        skill_ReadyImage.SetActive(false);

        //8개의 객체를 만들어 공격을 처리합니다.
        for (int i = 0; i <8; i++)
        {
            var temp = Instantiate<GameObject>(skill_Prefab, GameManager.instance.transform);
            temp.transform.position = transform.position;

            temp.transform.Rotate(transform.up, 45.0f * i);
            temp.GetComponent<Boss1_Skill2_Chlid>().Setting(Atk * Factor, Range, 200.0f);
        }

        StartCoroutine(DisableAttack());
    }

    public IEnumerator DisableAttack()
    {
        //스킬 공격상태를 종료합니다.
        yield return new WaitForSeconds(2.0f);
        IsAttacking = false;
        gameObject.SetActive(false);
    }
}
