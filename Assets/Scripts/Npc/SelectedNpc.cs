using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedNpc : MonoBehaviour
{
    public bool isPlayerApproach = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isPlayerApproach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerApproach = false;
        }
    }

    public void Selected()
    {
        if(isPlayerApproach == true)
        {
            UIManager.instance.nowNpcStatment = GetComponent<NpcStatment>();

            QuestManager.instance.RelayQuestComplete(GetComponent<NpcStatment>().ID);
            
            UIManager.instance.UISetting(UiKind.UIKind_ConversationUi);

            CameraManager.instance.PauseCamaraChangeON(gameObject);
        }
    }
}
