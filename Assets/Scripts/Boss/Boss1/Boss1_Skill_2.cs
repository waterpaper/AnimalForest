using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill_2 : MonoBehaviour
{
    public float Atk { get; private set; }
    public float Factor { get; private set; }
    public float Range { get; private set; }
    public bool IsAttacking { get; private set; }

    public GameObject Skill_Prefab;
    public GameObject Skill_ReadyImage;

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
    }

    IEnumerator StartSkill()
    {
        //범위를 띄워줍니다.
        Skill_ReadyImage.SetActive(true);
        IsAttacking = true;

        yield return new WaitForSeconds(3.0f);

        Skill_ReadyImage.SetActive(false);

        //8개의 객체를 만들어 공격을 처리합니다.
        for (int i = 0; i <8; i++)
        {
            var temp = Instantiate<GameObject>(Skill_Prefab, GameManager.instance.transform);
            temp.transform.position = transform.position;

            temp.transform.Rotate(transform.up, 45.0f * i);
            temp.GetComponent<Boss1_Skill2_Chlid>().Setting(Atk * Factor, Range, 200.0f);
        }

        StartCoroutine(DisableSkill());
    }

    IEnumerator DisableSkill()
    {
        //스킬 공격상태를 종료합니다.
        yield return new WaitForSeconds(2.0f);
        IsAttacking = false;
        gameObject.SetActive(false);
    }
}
