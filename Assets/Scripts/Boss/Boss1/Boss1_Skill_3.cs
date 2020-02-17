using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Skill_3 : MonoBehaviour
{
    public float Atk { get; private set; }
    public float Factor { get; private set; }
    public float Range { get; private set; }
    public bool IsAttacking { get; private set; }

    public GameObject Skill_Prefab;
    public GameObject Skill_ReadyImage;

    public void Setting(float bossAtk, float factor, float range)
    {
        Factor = factor;
        Atk = bossAtk;
        Range = range;
    }

    private void OnEnable()
    {
        IsAttacking = true;
        StartCoroutine(Skill());
    }

    IEnumerator Skill()
    {
        Skill_ReadyImage.SetActive(true);
        yield return new WaitForSeconds(3.0f);

        Skill_ReadyImage.SetActive(false);

        for (int i = 0; i <8; i++)
        {
            var temp = Instantiate<GameObject>(Skill_Prefab, GameManager.instance.transform);
            temp.transform.position = transform.position;

            temp.transform.Rotate(transform.up, 45.0f * i);
            temp.GetComponent<Boss1_Skill3_Chlid>().Setting(Atk * Factor, Range, 200.0f);
        }

        StartCoroutine(AttackDisable());
    }

    IEnumerator AttackDisable()
    {
        yield return new WaitForSeconds(2.0f);
        IsAttacking = false;
    }
}
