using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //보스의 공격 상태를 검사하고 제어해주는 클래스입니다.
    public bool IsSetting = false;
    public bool IsAttacking = false;

    public BossStatement statement;
    public BossMovement movement;

    public Boss1_Attack Attack;
    public Boss1_Skill_1 Skill_1;
    public Boss1_Skill_2 Skill_2;

    public float Attack_Time = 0.0f;
    public float skill_1_Time = 0.0f;
    public float skill_2_Time = 0.0f;

    public void Awake()
    {
        statement = GetComponent<BossStatement>();
        movement = GetComponent<BossMovement>();

        Attack = transform.GetChild(1).GetChild(0).GetComponent<Boss1_Attack>();
        Skill_1 = transform.GetChild(1).GetChild(1).GetComponent<Boss1_Skill_1>();
        Skill_2 = transform.GetChild(1).GetChild(2).GetComponent<Boss1_Skill_2>();
    }

    private void Start()
    {
        //공격, 스킬별 세팅을 처리합니다.
        Attack.Setting(statement.atk, statement.bossAttack.Factor, statement.bossAttack.Range);
        Skill_1.Setting(statement.atk, statement.skill_1.Factor, statement.skill_1.Range);
        Skill_2.Setting(statement.atk, statement.skill_2.Factor, statement.skill_2.Range);

        IsAttacking = false;
    }

    public void OnDisable()
    {
        //off시 공격 오브젝트들을 재설정해줍니다.
        Attack.gameObject.SetActive(false);
        Skill_1.gameObject.SetActive(false);
        Skill_2.gameObject.SetActive(false);

        IsAttacking = false;
    }

    public bool IsBossAttack(float dist, out BossAction action)
    {
        //보스가 공격이 가능한지 여부를 판단해 공격을 실행해주는 함수입니다.
        //보스가 공격이 가능하면 상태를 바꿔주고 아니면 대기상태로 리턴합니다.
        BossAction actionTemp = ChoiceSkill(dist);
        
        if (actionTemp == BossAction.End)
        {
            //공격이 설정되지 않을시 실행합니다.
            action = BossAction.Idle;
            IsAttacking = false;
            return false;
        }

        //선택한 공격으로 AI를 바꿔주고 종료합니다.
        action = actionTemp;
        IsAttacking = true;
        return true;
    }

    public BossAction ChoiceSkill(float dist)
    {
        //공격 스킬 범위와 쿨타임, 남은 마나에 맞는 공격을 정하고 실행하는 함수입니다.
        if (statement.skill_2.Range >= dist)
        {
            if (Mathf.Abs(skill_2_Time - Time.time) > statement.skill_2.Time && statement.skill_2.Mp < statement.mp)
            {
                skill_2_Time = Time.time;
                Skill_2.gameObject.SetActive(true);
                return BossAction.Skill2;
            }
        }
        
        if (statement.skill_1.Range >= dist)
        {
            if (Mathf.Abs(skill_1_Time - Time.time) > statement.skill_1.Time && statement.skill_1.Mp < statement.mp)
            {
                skill_1_Time = Time.time;
                Skill_1.gameObject.SetActive(true);
                return BossAction.Skill1;
            }
        }
        
        if (statement.bossAttack.Range >= dist)
        {
            if (Mathf.Abs(Attack_Time - Time.time) > statement.bossAttack.Time && statement.bossAttack.Mp < statement.mp)
            {
                Attack_Time = Time.time;
                Attack.gameObject.SetActive(true);
                return BossAction.Attack;
            }
        }

        return BossAction.End;
    }

    public bool IsAttackConfirm(BossAction action)
    {
        //공격 중인지 상태를 확인하고 리턴해줍니다.

        if (IsAttacking == false) return false;

        if (action == BossAction.Attack)
        {
            IsAttacking = Attack.IsAttacking;
        }
        else if (action == BossAction.Skill1)
        {
            IsAttacking = Skill_1.IsAttacking;
        }
        else if (action == BossAction.Skill2)
        {
            IsAttacking = Skill_2.IsAttacking;
        }

        return IsAttacking;
    }
}