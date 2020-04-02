using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillMono : MonoBehaviour
{
    //각 속성을 나타내는 프로퍼티입니다.
    public float Atk { get; protected set; }
    public float Factor { get; protected set; }
    public float Range { get; protected set; }
    public bool IsAttacking { get; protected set; }
    public bool IsGround { get; protected set; }
}

public interface IBossAttack
{
    //보스 공격을 정의한 인터페이스입니다.
    void Setting(float bossAtk, float factor, float range);

    IEnumerator StartAttack();
    IEnumerator DisableAttack();
}

public class BossAttack : MonoBehaviour
{
    //보스의 공격 상태를 검사하고 제어해주는 클래스입니다.
    [Header("AttackObject")]
    //각 공격별 오브젝트입니다.
    public Boss1_Attack Attack;
    public Boss1_Skill_1 Skill_1;
    public Boss1_Skill_2 Skill_2;

    [Header("AttackCoolDown")]
    //공격후 지난 시간을 저장합니다.
    public float Attack_Time = 0.0f;
    public float skill_1_Time = 0.0f;
    public float skill_2_Time = 0.0f;

    //상태 프로퍼티입니다.
    public bool IsSetting { get; private set; }
    public bool IsAttacking { get; private set; }

    private BossStatement _statement;
    private BossMovement _movement;
    
    public void Start()
    {
        _statement = GetComponent<BossStatement>();
        _movement = GetComponent<BossMovement>();

        Attack = transform.GetChild(1).GetChild(0).GetComponent<Boss1_Attack>();
        Skill_1 = transform.GetChild(1).GetChild(1).GetComponent<Boss1_Skill_1>();
        Skill_2 = transform.GetChild(1).GetChild(2).GetComponent<Boss1_Skill_2>();
        
        AttackSetting();
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

    private void AttackSetting()
    {
        //공격, 스킬별 세팅을 처리합니다.
        Attack.Setting(_statement.atk, _statement.bossAttack.Factor, _statement.bossAttack.Range);
        Skill_1.Setting(_statement.atk, _statement.skill_1.Factor, _statement.skill_1.Range);
        Skill_2.Setting(_statement.atk, _statement.skill_2.Factor, _statement.skill_2.Range);
    }

    public bool IsAttackConfirm(BossAction action)
    {
        //공격 중인지 상태를 확인하고 리턴해줍니다.
        if (IsAttacking == false) return false;

        if (action == BossAction.Attack)
            IsAttacking = Attack.IsAttacking;

        else if (action == BossAction.Skill1)
            IsAttacking = Skill_1.IsAttacking;

        else if (action == BossAction.Skill2)
            IsAttacking = Skill_2.IsAttacking;

        return IsAttacking;
    }

    private BossAction ChoiceSkill(float dist)
    {
        //공격 스킬 범위와 쿨타임, 남은 마나에 맞는 공격을 정하고 실행하는 함수입니다.
        if (_statement.skill_2.Range >= dist)
        {
            if (Mathf.Abs(skill_2_Time - Time.time) > _statement.skill_2.Time && _statement.skill_2.Mp < _statement.mp)
            {
                skill_2_Time = Time.time;
                Skill_2.gameObject.SetActive(true);
                return BossAction.Skill2;
            }
        }

        if (_statement.skill_1.Range >= dist)
        {
            if (Mathf.Abs(skill_1_Time - Time.time) > _statement.skill_1.Time && _statement.skill_1.Mp < _statement.mp)
            {
                skill_1_Time = Time.time;
                Skill_1.gameObject.SetActive(true);
                return BossAction.Skill1;
            }
        }

        if (_statement.bossAttack.Range >= dist)
        {
            if (Mathf.Abs(Attack_Time - Time.time) > _statement.bossAttack.Time && _statement.bossAttack.Mp < _statement.mp)
            {
                Attack_Time = Time.time;
                Attack.gameObject.SetActive(true);
                return BossAction.Attack;
            }
        }

        return BossAction.End;
    }
}