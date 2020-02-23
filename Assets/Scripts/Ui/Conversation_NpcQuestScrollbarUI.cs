using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversation_NpcQuestScrollbarUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject slotLocation;
    public ConversationUI_NpcQuestScrollbarUI_Contents[] npcQuestSlot;
    
    private void Awake()
    {
        npcQuestSlot = new ConversationUI_NpcQuestScrollbarUI_Contents[10];

        for (int i=0;i<10;i++)
        {
            npcQuestSlot[i] = Instantiate<GameObject>(slotPrefab, slotLocation.transform).GetComponent<ConversationUI_NpcQuestScrollbarUI_Contents>();
            npcQuestSlot[i].index = i;
            npcQuestSlot[i].gameObject.SetActive(false);
        }
    }

    public void Setting(List<int> npcQuestList)
    {
        for (int i = 0; i < 10; i++)
        {
            if (npcQuestList.Count > i)
            {
                npcQuestSlot[i].questID = npcQuestList[i];
                npcQuestSlot[i].Setting();
                npcQuestSlot[i].gameObject.SetActive(true);
            }
            else
            {
                npcQuestSlot[i].gameObject.SetActive(false);
            }
        }
    }
}
