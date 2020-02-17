using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public int id;
    public string name;
    public int kind;
    public float hp;
    public float mp;
    public float atk;
    public float def;
    public int level;
    public int exp;
    public int money;

    public List<int> clearEventList;

    public int expMax;
    public float hpMax;
    public float mpMax;

    public float addHp;
    public float addMp;
    public float addAtk;
    public float addDef;
   
    public void loadCharacterStatement(PlayerData info)
    {
        id = info.ID;
        name = info.Name;
        kind = info.Kind;
        hp = info.Hp;
        mp = info.Mp;
        atk = info.Atk;
        def = info.Def;
        level = info.Level;
        exp = info.Exp;
        money = info.Money;
        expMax = info.ExpMax;
        hpMax = info.HpMax;
        mpMax = info.MpMax;
        addHp = info.AddHp;
        addMp = info.AddMp;
        addAtk = info.AddAtk;
        addDef = info.AddDef;
        clearEventList = info.ClearEventList;
    }
}
