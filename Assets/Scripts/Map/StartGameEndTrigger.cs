using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameEndTrigger : MonoBehaviour
{
    public SceneChange sceneChange;

    public void Start()
    {
        sceneChange = GetComponent<SceneChange>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(UIManager.instance.IsSingleConversationUI == false && QuestManager.instance.GetQuest(1).QuestProgress == EQuestProgress.EQuestProgress_Completed)
        {
            sceneChange.changeScene = SceneKind.Town;
            sceneChange.StartSceneChange();
            QuestManager.instance.RewardQuest(QuestManager.instance.GetQuest(1).QuestID);
        }
    }
}
