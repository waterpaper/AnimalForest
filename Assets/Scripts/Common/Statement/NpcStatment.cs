using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatment : MonoBehaviour
{
    public enum NPCTYPE
    {
        NPCTYPE_General,
        NPCTYPE_Quest,
        NPCTYPE_WeaponShop,
        NPCTYPE_VarietyShop,
        NPCTYPE_End
    }

    public int ID;
    public NPCTYPE type;
    public string name;
    public int level;
    public float hp;
    public float hpMax;
    public float mp;
    public float mpMax;
    public float atk;
    public float def;
    public int shopKind;
    public List<string> Conversations;
    public List<int> NpcQuestData;

    public void Setting(NpcTable info)
    {
        ID = info.ID;
        type = (NPCTYPE)info.Type;
        name = info.Name;
        level = info.Level;
        hp = info.Hp;
        hpMax = info.HpMax;
        mp = info.Mp;
        mpMax = info.MpMax;
        atk = info.Atk;
        def = info.Def;
        shopKind = info.ShopKind;
        Conversations = new List<string>();
        info.Conversations.ForEach((str) => { Conversations.Add(str); });
        NpcQuestData = new List<int>();
        info.NpcQuests.ForEach((questData) => { NpcQuestData.Add(questData); });
    }

    public void addNpcQuest(int questIndex)
    {
        NpcQuestData.Add(questIndex);
        NpcQuestData.Sort();
    }

    public void deleteNpcQuest(int questIndex)
    {
        for(int i =0;i< NpcQuestData.Count;i++)
        {
            if (NpcQuestData[i] == questIndex)
                NpcQuestData.RemoveAt(i);
        }
    }
}
