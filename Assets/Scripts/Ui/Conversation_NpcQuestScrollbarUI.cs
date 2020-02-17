using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation_NpcQuestScrollbarUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject slotLocation;
    public GameObject[] npcQuestSlot;
    
    private void Awake()
    {
        npcQuestSlot = new GameObject[10];

        for (int i=0;i<10;i++)
        {
            npcQuestSlot[i] = Instantiate<GameObject>(slotPrefab, slotLocation.transform);
            npcQuestSlot[i].GetComponent<ConversationUI_NpcQuestScrollbarUI_Contents>().index = i;
            npcQuestSlot[i].SetActive(false);
        }
    }

    public void Setting(List<int> npcQuestList)
    {
        for (int i = 0; i < 10; i++)
        {
            if (npcQuestList.Count > i)
            {
                npcQuestSlot[i].GetComponent<ConversationUI_NpcQuestScrollbarUI_Contents>().questID = npcQuestList[i];
                npcQuestSlot[i].GetComponent<ConversationUI_NpcQuestScrollbarUI_Contents>().Setting();
                npcQuestSlot[i].SetActive(true);
            }
            else
            {
                npcQuestSlot[i].SetActive(false);
            }
        }
    }
}
