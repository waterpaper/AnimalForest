using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public string id;
    public string name;
    public int kind;
    public float hp;
    public float mp;
    public float atk;
    public float def;
    public int level;
    public int exp;
    public int money;

    public List<int> clearQuestList;
    public List<int> clearEventList;

    public int expMax;
    public float hpMax;
    public float mpMax;

    public float addHp;
    public float addMp;
    public float addAtk;
    public float addDef;

    public void StartPlayerSetting()
    {
        id = "empty";
        name = "empty";
        kind = 1;
        hp = 100;
        mp = 100;
        atk = 10;
        def = 0;
        level = 1;
        exp = 0;
        money = 1000;
        expMax = 100;
        hpMax = 100;
        mpMax = 100;
        addHp = 0;
        addMp = 0;
        addAtk = 0;
        addDef = 0;

        clearEventList = new List<int>();
        clearQuestList = new List<int>();
    }
   
    public void LoadPlayerSetting(PlayerSaveData info)
    {
        id = info.ID;
        name = info.Name;
        kind = info.Kind;
        hp = info.Hp;
        hpMax = info.HpMax;
        mp = info.Mp;
        mpMax = info.MpMax;
        atk = info.Atk;
        def = info.Def;
        level = info.Level;
        exp = info.Exp;
        money = info.Money;
        expMax = DataManager.instance.GetTableData<PlayerLevelTable>(TableDataKind.TableDataKind_PlayerLevel, level).ExpMax;

        for(int i=0;i < info.ClearEvent.Length;i++)
        {
            if(info.ClearEvent[i] == '1')
            {
                clearEventList.Add(i);
            }
        }

        for (int i = 0; i < info.ClearQuest.Length; i++)
        {
            if (info.ClearQuest[i] == '1')
            {
                clearQuestList.Add(i);
            }
        }
    }
}
