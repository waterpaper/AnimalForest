using System.Collections.Generic;

public enum EQuestKind
{
    EQuestKind_Hunting,
    EQuestKind_Collection,
    EQuestKind_Relay,
    EQuestKind_BossHunting,
    EQuestKind_End
}

public enum EQuestProgress
{
    EQuestProgress_Proceeding,
    EQuestProgress_Completed,
    EQuestProgress_End
}

abstract public class Quest
{
    public int QuestID { get; protected set; }
    public int MinLevel { get; protected set; }
    public int OrderNpc { get; protected set; }
    public string OrderNpcConversation { get; protected set; }
    public int TargetNpc { get; protected set; }
    public string TargetNpcConversation { get; protected set; }
    public string QuestName { get; protected set; }
    public string QuestExplanation { get; protected set; }
    public string QuestTargetObjectName { get; protected set; }
    public int TargetObject_NowCount { get; protected set; }
    public int TargetObject_TargetCount { get; protected set; }
    public int TargetObject { get; protected set; }
    public EQuestKind QuestKind { get; protected set; }
    public EQuestProgress QuestProgress { get; protected set; }
    public int QuestRewardExp { get; protected set; }
    public int QuestRewardMoney { get; protected set; }
    public List<int> QuestItem { get; protected set; }

    public void Setting(QuestTable QuestInfo, int targetCount = 0)
    {
        QuestID = QuestInfo.ID;
        MinLevel = QuestInfo.MinLevel;
        OrderNpc = QuestInfo.TargetNpc;
        TargetNpc = QuestInfo.TargetNpc;
        QuestName = QuestInfo.Name;
        OrderNpcConversation = QuestInfo.OrderNpcConversation;
        TargetNpcConversation = QuestInfo.TargetNpcConversation;
        QuestKind = (EQuestKind)QuestInfo.Kind;
        TargetObject = QuestInfo.TargetObject;
        TargetObject_NowCount = targetCount;
        TargetObject_TargetCount = QuestInfo.TargetCount;
        QuestRewardExp = QuestInfo.RewardExp;
        QuestRewardMoney = QuestInfo.RewardMoney;

        QuestItem = new List<int>(0);
        QuestInfo.RewardItem.ForEach(itemID => { QuestItem.Add(itemID); });

        if(TargetObject_TargetCount <= TargetObject_NowCount)
            QuestProgress = EQuestProgress.EQuestProgress_Completed;
        else
            QuestProgress = EQuestProgress.EQuestProgress_Proceeding;

        TextSetting();
    }
    abstract public void Updateing(EQuestKind kind, int targetNumber);

    abstract public void TextSetting();
}

public class HuntingQuest : Quest
{
    public override void Updateing(EQuestKind kind, int targetNumber)
    {
        //같은 종류의 쾌스트가 아니거나 완료상태의 쾌스트시 함수를 종료합니다.
        if (kind != QuestKind || QuestProgress == EQuestProgress.EQuestProgress_Completed) return;

        //현재 잡은 몬스터의 아이디와 쾌스트 타겟 아이디와 비교합니다. 아닐시 해당 함수를 종료합니다.
        if (targetNumber != TargetObject) return;

        //현재 타겟몬스터의 잡은 횟수를 증가시키고 목표치와 비교 후 진행상태를 업데이트합니다.
        TargetObject_NowCount++;

        QuestExplanation = QuestTargetObjectName + string.Format(" ( {0} / {1} )", TargetObject_NowCount, TargetObject_TargetCount);

        if (TargetObject_NowCount >= TargetObject_TargetCount)
        {
            QuestProgress = EQuestProgress.EQuestProgress_Completed;
            TextSetting();
        }
    }

    public override void TextSetting()
    {
        if (QuestProgress == EQuestProgress.EQuestProgress_Proceeding)
        {
            QuestTargetObjectName = DataManager.instance.EnemyInfo(TargetObject).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" ( {0} / {1} )", TargetObject_NowCount, TargetObject_TargetCount);
        }
        else if (QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            QuestTargetObjectName = DataManager.instance.NpcInfo(TargetNpc).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" 에게 찾아가기");
        }
    }
}

public class CollectionQuest : Quest
{
    public override void Updateing(EQuestKind kind, int targetNumber)
    {
        if (kind != QuestKind) return;
        //쾌스트 오브젝트 아이디에 해당하는 아이템을 인벤토리에서 검색합니다. 
        Item temp = InventoryManager.instance.GetItem_SearchID(TargetObject);

        //없을시 해당함수를 종료시킵니다.
        if (temp == null)
        {
            TargetObject_NowCount = 0;
            return;
        }

        TargetObject_NowCount = temp.count;

        if (TargetObject_NowCount >= TargetObject_TargetCount)
        {
            if (QuestProgress == EQuestProgress.EQuestProgress_Completed) return;
            
                QuestProgress = EQuestProgress.EQuestProgress_Completed;
                TextSetting();      
        }
        else
        {
            QuestProgress = EQuestProgress.EQuestProgress_Proceeding;
            TextSetting();
        }
    }

    public override void TextSetting()
    {
        if (QuestProgress == EQuestProgress.EQuestProgress_Proceeding)
        {
            QuestTargetObjectName = DataManager.instance.ItemInfo(TargetObject).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" ( {0} / {1} )", TargetObject_NowCount, TargetObject_TargetCount);
        }
        else if (QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            QuestTargetObjectName = DataManager.instance.NpcInfo(TargetNpc).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" 에게 찾아가기");
        }
    }
}

public class RelayQuest : Quest
{
    public override void Updateing(EQuestKind kind, int targetNumber)
    {
        //같은 종류의 쾌스트가 아니거나 완료상태의 쾌스트시 함수를 종료합니다.
        if (kind != QuestKind || QuestProgress == EQuestProgress.EQuestProgress_Completed) return;

        //해당 쾌스트의 목표와 같으면 진행상태를 업데이트합니다.
        if (TargetObject == targetNumber)
        {
            QuestProgress = EQuestProgress.EQuestProgress_Completed;
            TextSetting();
        }
    }

    public override void TextSetting()
    {
        if (QuestProgress == EQuestProgress.EQuestProgress_Proceeding)
        {
            QuestTargetObjectName = DataManager.instance.NpcInfo(TargetNpc).Name;
            QuestExplanation = QuestTargetObjectName + (" 에게 찾아가기");
        }
        else if (QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            QuestTargetObjectName = DataManager.instance.NpcInfo(TargetNpc).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" 에게 찾아가기");
        }
    }
}

public class BossHuntingQuest : Quest
{
    public override void Updateing(EQuestKind kind, int targetNumber)
    {
        //같은 종류의 쾌스트가 아니거나 완료상태의 쾌스트시 함수를 종료합니다.
        if (kind != QuestKind || QuestProgress == EQuestProgress.EQuestProgress_Completed) return;

        //해당 쾌스트의 목표와 같으면 진행상태를 업데이트합니다.
        if (TargetObject == targetNumber)
        {
            QuestProgress = EQuestProgress.EQuestProgress_Completed;
            TextSetting();
        }
    }

    public override void TextSetting()
    {
        if (QuestProgress == EQuestProgress.EQuestProgress_Proceeding)
        {
            QuestTargetObjectName = DataManager.instance.BossInfo(TargetObject).BossName;
            QuestExplanation = QuestTargetObjectName + (" 처치하기");
        }
        else if (QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            QuestTargetObjectName = DataManager.instance.NpcInfo(TargetNpc).Name;
            QuestExplanation = QuestTargetObjectName + string.Format(" 에게 찾아가기");
        }
    }
}