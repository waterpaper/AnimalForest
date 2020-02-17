using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScrollViewer : MonoBehaviour
{
    public int maxQuestUI = 10;
    public GameObject questContentUIPrefab;
    public GameObject contentsLocation;
    public List<GameObject> QuestContentsUIList;
    public bool viewer = false;

    private void Awake()
    {
        for (int i = 0; i < maxQuestUI; i++)
        {
            var newQuestUI = Instantiate<GameObject>(questContentUIPrefab, contentsLocation.transform);
            newQuestUI.GetComponent<QuestContentsUI>().index = i;
            QuestContentsUIList.Add(newQuestUI);
            newQuestUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        viewer = true;

        if (QuestManager.instance.nowQuestCount > 0)
        {
            StartCoroutine(updateQuestUI());
        }
        else if(QuestManager.instance.nowQuestCount == 0)
        {
            viewer = false;
        }
    }
    private void OnDisable()
    {
        viewer = false;
    }

    IEnumerator updateQuestUI()
    {
        while (viewer == true)
        {
            for (int i = 0; i < QuestManager.instance.nowQuestCount; i++)
            {
                QuestContentsUIList[i].SetActive(true);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
