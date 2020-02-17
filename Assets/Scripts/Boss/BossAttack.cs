using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public bool IsSetting = false;
    public bool IsAttacking = false;
    public BossStatment statement;
    public BossMovement movement;

    public GameObject Skill_1_Object;
    public GameObject Skill_2_Object;
    public GameObject Skill_3_Object;

    public float skill_1_Time = 0.0f;
    public float skill_2_Time = 0.0f;
    public float skill_3_Time = 0.0f;

    public void Awake()
    {
        statement = GetComponent<BossStatment>();
        movement = GetComponent<BossMovement>();

        Skill_1_Object = transform.GetChild(1).GetChild(0).gameObject;
        Skill_2_Object = transform.GetChild(1).GetChild(1).gameObject;
        Skill_3_Object = transform.GetChild(1).GetChild(2).gameObject;
    }

    public void OnEnable()
    {
        if (IsSetting == true)
        {
            Skill_1_Object.SetActive(true);
            Skill_2_Object.SetActive(true);
            Skill_3_Object.SetActive(true);

            IsAttacking = false;
            IsSetting = true;
        }
    }


    private void Start()
    {
        Skill_1_Object.GetComponent<Boss1_Skill_1>().Setting(statement.atk, statement.skill_1.Factor, statement.skill_1.Range);
        Skill_2_Object.GetComponent<Boss1_Skill_2>().Setting(statement.atk, statement.skill_2.Factor, statement.skill_3.Range);
        Skill_3_Object.GetComponent<Boss1_Skill_3>().Setting(statement.atk, statement.skill_2.Factor, statement.skill_3.Range);

    }

    public void OnDisable()
    {
        Skill_1_Object.SetActive(false);
        Skill_2_Object.SetActive(false);
        Skill_3_Object.SetActive(false);
    }

    public bool IsBossAttack(float dist, out BossAI.BossAction action)
    {
        BossAI.BossAction actionTemp = ChoiceSkill(dist);

        //공격을 할수 없을시 실행됩니다.
        if (actionTemp == BossAI.BossAction.End)
        {
            action = BossAI.BossAction.Idle;
            IsAttacking = false;
            return false;
        }

        //선택한 공격으로 AI를 바꿔주고 종료합니다.
        action = actionTemp;
        IsAttacking = true;
        return true;
    }

    public BossAI.BossAction ChoiceSkill(float dist)
    {
        //스킬 범위와 남은 마나에 맞는 공격을 정해 실행시키고 리턴합니다.

        if (statement.skill_3.Range >= dist)
        {
            if (Mathf.Abs(skill_3_Time - Time.time) > statement.skill_3.Time && statement.skill_3.Mp < statement.mp)
            {
                skill_3_Time = Time.time;
                Skill_3_Object.SetActive(true);
                return BossAI.BossAction.Skill2;
            }
        }
        
        if (statement.skill_2.Range >= dist)
        {
            if (Mathf.Abs(skill_2_Time - Time.time) > statement.skill_2.Time && statement.skill_2.Mp < statement.mp)
            {
                skill_2_Time = Time.time;
                Skill_2_Object.SetActive(true);
                return BossAI.BossAction.Skill1;
            }
        }
        
        if (statement.skill_1.Range >= dist)
        {
            if (Mathf.Abs(skill_1_Time - Time.time) > statement.skill_1.Time && statement.skill_1.Mp < statement.mp)
            {
                skill_1_Time = Time.time;
                Skill_1_Object.SetActive(true);
                return BossAI.BossAction.Attack;
            }
        }

        return BossAI.BossAction.End;
    }

    public bool IsAttackConfirm(BossAI.BossAction action)
    {
        if (IsAttacking == false) return false;

        if (action == BossAI.BossAction.Attack)
        {
            IsAttacking = Skill_1_Object.GetComponent<Boss1_Skill_1>().IsAttacking;

            if (IsAttacking == false)
                Skill_1_Object.SetActive(false);
        }
        else if (action == BossAI.BossAction.Skill1)
        {
            IsAttacking = Skill_2_Object.GetComponent<Boss1_Skill_2>().IsAttacking;

            if (IsAttacking == false)
                Skill_2_Object.SetActive(false);
        }
        else if (action == BossAI.BossAction.Skill2)
        {
            IsAttacking = Skill_3_Object.GetComponent<Boss1_Skill_3>().IsAttacking;

            if (IsAttacking == false)
                Skill_3_Object.SetActive(false);
        }

        return IsAttacking;
    }
}