using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleConversationTrigger : MonoBehaviour
{
    [Header("ConversationSetting")]
    public List<int> conversationList;
    public bool clearList;
    public int questID = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (conversationList.Count > 0)
            {
                UIManager.instance.singleConversationUI.GetComponent<SingleConversationUI>().Setting(conversationList, clearList);

                //싱글 대화차이 열려 있을 경우 종료해준다.
                if(UIManager.instance.IsSingleConversationUI == true)
                    UIManager.instance.UISetting(UiKind.UIKind_SingleConversationPauseUI);


                if (DataManager.instance.SingleConversationInfo(conversationList[0]).Type == 0)
                    UIManager.instance.UISetting(UiKind.UIKind_SingleConversationUI);
                else
                    UIManager.instance.UISetting(UiKind.UIKind_SingleConversationPauseUI);

            }

            if(questID >=1)
            {
                QuestManager.instance.AddQuest(questID);
            }

            gameObject.SetActive(false);
        }
    }
}
