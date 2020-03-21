using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatment : MonoBehaviour
{
    public int ID = -1;
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

    //npc정보를 세팅합니다.
    public void Awake()
    {
        Setting(DataManager.instance.NpcInfo(ID));
    }

    //키고 끌때 제거되지 않은 완료 퀘스트목록을 지워줍니다.
    public void OnEnable()
    {
        if(ID != -1)
        {
            if (NpcQuestData.Count == 0) return;

            List<int> deleteTemp = new List<int>();

            for (int i=0;i< NpcQuestData.Count; i++)
            {
                if (PlayerManager.instance.IsClearQuestList(NpcQuestData[i]))
                {
                    deleteTemp.Add(i);
                }
            }

            for (int i = deleteTemp.Count-1; i >= 0; i--)
            {
                NpcQuestData.RemoveAt(deleteTemp[i]);
            }
        }
    }

    //정보를 세팅합니다.
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

        gameObject.name = name;
    }


    //퀘스트목록을 추가합니다
    public void AddNpcQuest(int questIndex)
    {
        NpcQuestData.Add(questIndex);
        NpcQuestData.Sort();
    }

    //퀘스트 목록에 인덱스를 제거합니다.
    public void DeleteNpcQuest(int questIndex)
    {
        for(int i =0;i< NpcQuestData.Count;i++)
        {
            if (NpcQuestData[i] == questIndex)
                NpcQuestData.RemoveAt(i);
        }
    }
}
