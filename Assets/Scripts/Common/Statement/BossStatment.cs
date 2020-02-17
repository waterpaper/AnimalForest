using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossSkill
{
    public string Name;
    public int Mp;
    public float Factor;
    public float Range;
    public float Time;
}


public class BossStatment : MonoBehaviour
{
    public int ID;
    public string bossName;
    public int level;
    public int exp;
    public int hp;
    public int hpMax;
    public int mp;
    public int mpMax;
    public float atk;
    public float def;

    //추적 사정거리
    public float traceDist;
    //순찰 스피드
    public float patrolMoveSpeed;
    //추적 스피드
    public float traceMoveSpeed;
    //기본 공격을 포함한 보스의 스킬 정보
    public BossSkill skill_1;
    public BossSkill skill_2;
    public BossSkill skill_3;


    public void Awake()
    {
        BossTable temp = DataManager.instance.BossInfo(ID);
        Setting(temp);
    }

    private void OnEnable()
    {
        hp = hpMax;
        mp = mpMax;
    }

    void Setting(BossTable temp)
    {
        bossName = temp.BossName;
        level = temp.Level;
        exp = temp.Exp;
        hp =temp.Hp;
        hpMax = temp.HpMax;
        mp = temp.Mp;
        mpMax = temp.MpMax;
        atk = temp.Atk;
        def = temp.Def;
        traceDist = temp.TraceDist;
        patrolMoveSpeed = temp.PatrolMoveSpeed;
        traceMoveSpeed = temp.TraceMoveSpeed;
        skill_1 = temp.Skill_1;
        skill_2 = temp.Skill_2;
        skill_3 = temp.Skill_3;
    }
}
