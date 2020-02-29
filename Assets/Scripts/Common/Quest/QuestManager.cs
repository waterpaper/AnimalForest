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

    public List<Quest> GetQuest()
    {
        return QuestList;
    }

    public Quest GetQuest(int count)
    {
        if (count >= nowQuestCount)
            return null;

        return QuestList[count];
    }

    //퀘스트 아이디를 기준으로 해당 종류에 맞는 퀘스트를 생성해 추가해줍니다.
    public void AddQuest(int questNum, int targetCount = 0)
    {
        Quest newQuest = null;
        QuestTable temp = DataManager.instance.QuestInfo(questNum);

        if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Hunting)
        {
            newQuest = new HuntingQuest();
            newQuest.Setting(temp, targetCount);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Collection)
        {
            newQuest = new CollectionQuest();
            newQuest.Setting(temp, targetCount);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_Relay)
        {
            newQuest = new RelayQuest();
            newQuest.Setting(temp, targetCount);
        }
        else if ((EQuestKind)temp.Kind == EQuestKind.EQuestKind_BossHunting)
        {
            newQuest = new BossHuntingQuest();
            newQuest.Setting(temp, targetCount);
        }

        //퀘스트가 초기화된 새로운 퀘스트만 리스트에 추가합니다.
        if (newQuest != null)
        {
            QuestList.Add(newQuest);
            nowQuestCount++;
        }

        PoolManager.instance.NpcQuestDelete(temp.OrderNpc, questNum);
    }

    public void UpdateQuest(EQuestKind kind, int targetNumber)
    {
        //해당 종류 퀘스트중 targetnumber와 같은 퀘스트를 업데이트해 완료상태인지 아닌지 업데이트 해줍니다.
        StartCoroutine(IEUpdateQuest(kind, targetNumber));
    }

    IEnumerator IEUpdateQuest(EQuestKind kind, int targetNumber)
    {
        QuestList.ForEach((quest) => { quest.Updateing(kind, targetNumber); });
        yield return null;
    }

    public void RelayQuestComplete(int npcID)
    {
        //전달 퀘스트는 다른 퀘스트와는 달리 클릭시 완료해야 하기 때문에 검사후 완료해줍니다.
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

    //npc와 대화를 시작할때 해당 npc의 쾌스트중 완료된 뭬스트가 있는지 여부를 판단하고 존재시 해당 퀘스트 넘버를 반환하고 완료처리를 합니다.
    public int CompleteQuest(int npcID)
    {
        int index = -1;
        
        for(int i=0; i <QuestList.Count ;i++)
        {
            if (QuestList[i].QuestProgress != EQuestProgress.EQuestProgress_Completed) continue;

            //해당 퀘스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].TargetNpc == npcID)
            {
                index = QuestList[i].QuestID;
                break;
            }
        }

        return index;
    }

    //완료 퀘스트를 삭제합니다.
    public void DeleteQuest(int questID)
    {
        for (int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].QuestProgress != EQuestProgress.EQuestProgress_Completed) continue;

            //해당 퀘스트의 완료 npc와 같은지 여부를 판단한다.
            if (QuestList[i].QuestID == questID)
            {
                QuestList.RemoveAt(i);
                nowQuestCount--;
                break;
            }
        }
    }

    //퀘스트 보상을 처리합니다.(추가적으로 보상을 받으며 플레이어의 완료 퀘스트 목록에 추가합니다.)
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

                PlayerManager.instance.AddClearQuestList(questID);
                break;
            }
        }
    }

    //로드시 퀘스트 정보를 설정합니다.
    public void LoadQuest(PlayerSaveData saveData)
    {
        saveData.QuestList.ForEach((questTemp) => {
            AddQuest(questTemp.QuestID, questTemp.TargetNowCount);
        });
    }

}
