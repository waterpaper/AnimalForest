using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NormalUI : MonoBehaviour
{
    public Image playerImage;
    public GameObject QuestScrollViewer;
    public GameObject GetItemViewer;

    private void Start()
    {
        QuestScrollViewer = transform.GetChild(0).GetChild(5).gameObject;
        GetItemViewer = transform.GetChild(0).GetChild(6).gameObject;

        QuestScrollViewer.SetActive(false);
    }
    private void FixedUpdate()
    {
        playerImage.sprite = DataManager.instance.GetIconData(IconDataKind.IconDataKind_Character, PlayerManager.instance.Kind);

        if(QuestManager.instance.nowQuestCount > 1)
        {
            QuestScrollViewer.SetActive(true);
        }
        else
        {
            QuestScrollViewer.SetActive(true);
        }
    }
}
