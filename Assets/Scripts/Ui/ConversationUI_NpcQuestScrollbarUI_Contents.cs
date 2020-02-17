﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversationUI_NpcQuestScrollbarUI_Contents : MonoBehaviour
{
    public int index = 0;
    public int questID = 0;
    public TextMeshProUGUI QuestName;

    private void Awake()
    {
        QuestName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Setting()
    {
        QuestName.text = DataManager.instance.QuestInfo(questID).Name;
    }

    public void Click()
    {
        UIManager.instance.conversationUi.GetComponent<ConversationUI>().QuestViewSetting(questID, false);
    }
}