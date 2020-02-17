using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> QuestList;

    public int nowQuestCount = 0;

    private static QuestManager m_instance;

    //싱글톤 접근
    public static QuestManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<QuestManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        QuestList = new List<Quest>();
    }

    public Quest GetQuest(int count)
    {
        if (count >= nowQuestCount)
            return null;

        return QuestList[count];
    }

    public void AddQuest(int questNum)
    {
        Quest newQuest = null;
        QuestTable temp = DataManager.instance.QuestInfo(questNum);

        if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Hunting)
        {
            newQuest = new HuntingQuest();
            newQuest.Setting(temp);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Collection)
        {
            newQuest = new CollectionQuest();
            newQuest.Setting(temp);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Relay)
        {
            newQuest = new RelayQuest();
            newQuest.Setting(temp);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_BossHunting)
        {
            newQuest = new BossHuntingQuest();
            newQuest.Setting(temp);
        }

        //쾌스트가 초기화된 새로운 쾌스트만 리스트에 추가합니다.
        if (newQuest != null)
        {
            QuestList.Add(newQuest);
            nowQuestCount++;
        }
    }

    public void updateQuest(EQuestKind kind, int targetNumber)
    {
        StartCoroutine(IEUpdateQuest(kind, targetNumber));
    }

    IEnumerator IEUpdateQuest(EQuestKind kind, int targetNumber)
    {
        QuestList.ForEach((quest) => { quest.Updateing(kind, targetNumber); });
        yield return null;
    }

    public void RelayQuestComplete(int npcID)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].QuestProgress == EQuestProgress.EQuestProgress_Completed) continue;

            //해당 쾌스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].TargetNpc == npcID)
            {
                QuestList[i].Updateing(EQuestKind.EQuestKind_Relay, npcID);
                break;
            }
        }
    }

    public int CompleteQuest(int npcID)
    {
        //npc와 대화를 시작할때 해당 npc의 쾌스트중 완료된 쾌스트가 있는지 여부를 판단하고 존재시 해당 쾌스트 넘버를 반환하고 완료처리를 한다.
        int index = -1;
        
        for(int i=0; i <QuestList.Count ;i++)
        {
            if (QuestList[i].QuestProgress != EQuestProgress.EQuestProgress_Completed) continue;

            //해당 쾌스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].TargetNpc == npcID)
            {
                index = QuestList[i].QuestID;
                break;
            }
        }

        return index;
    }

    public void DeleteQuest(int questID)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].QuestProgress != EQuestProgress.EQuestProgress_Completed) continue;

            //해당 쾌스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].QuestID == questID)
            {
                QuestList.RemoveAt(i);
                nowQuestCount--;
                break;
            }
        }
    }

    public void RewardQuest(int questID)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            //해당 쾌스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].QuestID == questID)
            {
                PlayerManager.instance.Exp += QuestList[i].QuestRewardExp;
                PlayerManager.instance.Money += QuestList[i].QuestRewardMoney;
                QuestList[i].QuestItem.ForEach((rewardItemTemp) => {  InventoryManager.instance.AddItem(DropItemManager.instance.ItemSetting(rewardItemTemp)); });

                break;
            }
        }
    }

}
