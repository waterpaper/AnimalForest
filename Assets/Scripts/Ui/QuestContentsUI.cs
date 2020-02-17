using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestContentsUI : MonoBehaviour
{
    public int index=0;
    public TextMeshProUGUI QuestNameUI;
    public TextMeshProUGUI QuestProgressUI;
    public TextMeshProUGUI QuestExplanationUI;
    
    private void Awake()
    {
        QuestNameUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        QuestProgressUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        QuestExplanationUI = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        if (QuestManager.instance.nowQuestCount+1 < index)
        {
            this.gameObject.SetActive(false);
            return;
        }

        Quest quest = QuestManager.instance.GetQuest(index);

        if(quest == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        QuestNameUI.text = quest.QuestName;
        QuestExplanationUI.text = quest.QuestExplanation;

        if(quest.QuestProgress == EQuestProgress.EQuestProgress_Proceeding)
        {
            QuestProgressUI.text = "진행중";
            QuestProgressUI.color = new Color(0, 0, 128);
        }
        else if(quest.QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            QuestProgressUI.text = "완료";
            QuestProgressUI.color = new Color(128, 128, 0);
        }
    }

}
