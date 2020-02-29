using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ConversationUI : MonoBehaviour
{
    public NpcStatment nowNpcStatment;
    public TextMeshProUGUI npcNameTextUI;
    public TextMeshProUGUI textUI;

    public GameObject questButton;
    public GameObject questOkayButton;
    public GameObject shopButton;
    public GameObject exitButton;
    public GameObject npcQuestScollbar;
    public TextMeshProUGUI questRewardTextUI;

    public int questIndex = 0;
    public bool IsQuestCompletedUI;

    private void Awake()
    {
        npcNameTextUI = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        textUI = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        questButton = transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
        questOkayButton = transform.GetChild(0).GetChild(2).GetChild(1).gameObject;
        shopButton = transform.GetChild(0).GetChild(2).GetChild(2).gameObject;
        exitButton = transform.GetChild(0).GetChild(2).GetChild(3).gameObject;
        questRewardTextUI = transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>();

        npcQuestScollbar = transform.GetChild(0).GetChild(3).gameObject;
    }

    private void OnEnable()
    {
        nowNpcStatment = UIManager.instance.nowNpcStatment;

        questIndex = 0;
        if (nowNpcStatment != null)
        {
            npcNameTextUI.text = nowNpcStatment.name;

            //쾌스트 완료 npc클릭시 함수를 실행하고 종료합니다.
            if (QuestCompleteNPC() == true) return;

            //일상 대화는 0~2번째에 저장되어 있기 때문에 이렇게 사용합니다.
            int rand = Random.Range(0, 2);
            textUI.text = nowNpcStatment.Conversations[rand];

            if (nowNpcStatment.type == NpcStatment.NPCTYPE.NPCTYPE_General)
            {
                questButton.SetActive(false);
                shopButton.SetActive(false);
            }
            else if (nowNpcStatment.type == NpcStatment.NPCTYPE.NPCTYPE_Quest)
            {
                questButton.SetActive(true);
                shopButton.SetActive(false);
            }
            else if (nowNpcStatment.type == NpcStatment.NPCTYPE.NPCTYPE_WeaponShop || nowNpcStatment.type == NpcStatment.NPCTYPE.NPCTYPE_VarietyShop)
            {
                questButton.SetActive(true);
                shopButton.SetActive(true);
            }

            questOkayButton.SetActive(false);


            if (questButton.activeSelf == true)
            {
                if (nowNpcStatment.NpcQuestData.Count <= 0)
                {
                    questButton.SetActive(false);
                }
                else
                {
                    int questMinLevel = DataManager.instance.QuestInfo(nowNpcStatment.NpcQuestData[0]).MinLevel;

                    if (questMinLevel > PlayerManager.instance.Level)
                        questButton.SetActive(false);
                }
            }
        }
    }
    private void OnDisable()
    {
        nowNpcStatment = null;
        npcNameTextUI.text = "";
        textUI.text = "";

        questButton.SetActive(true);
        shopButton.SetActive(true);
        exitButton.SetActive(true);
        npcQuestScollbar.SetActive(false);
        questRewardTextUI.gameObject.SetActive(false);
        textUI.enabled = true;

        IsQuestCompletedUI = false;
    }

    public void QuestButton()
    {
        npcQuestScollbar.GetComponent<Conversation_NpcQuestScrollbarUI>().Setting(nowNpcStatment.NpcQuestData);
        npcQuestScollbar.SetActive(true);
        questButton.SetActive(false);
        shopButton.SetActive(false);
        textUI.enabled = false;
        questRewardTextUI.gameObject.SetActive(false);

    }
    public void ShopButton()
    {
        UIManager.instance.UISetting(UiKind.UIKind_ConversationUI);
        UIManager.instance.UISetting(UiKind.UiKind_ShopUI);
        UIManager.instance.UISetting(UiKind.UiKind_InventoryUI);
    }
    public void ExitButton()
    {
        if (IsQuestCompletedUI == true)
        {
            QuestManager.instance.RewardQuest(questIndex);
            QuestManager.instance.DeleteQuest(questIndex);
        }

        UIManager.instance.nowNpcStatment = null;
        UIManager.instance.UISetting(UiKind.UIKind_ConversationUI);
    }
    public void QuestOkayButton()
    {
        QuestManager.instance.AddQuest(questIndex);

        UIManager.instance.UISetting(UiKind.UIKind_ConversationUI);
        UIManager.instance.nowNpcStatment = null;
    }

    public void QuestViewSetting(int questID, bool completed)
    {
        textUI.enabled = true;
        npcQuestScollbar.SetActive(false);

        StringBuilder stringTemp = new StringBuilder("Reward\n");

        QuestTable questTemp = DataManager.instance.QuestInfo(questID);

        if (completed == false)
        {
            textUI.text = questTemp.OrderNpcConversation;
            questOkayButton.SetActive(true);
        }
        else
        {
            textUI.text = questTemp.TargetNpcConversation;
            questOkayButton.SetActive(false);
            questButton.SetActive(false);
            shopButton.SetActive(false);
        }
        stringTemp.AppendFormat("Exp : {0}  Money : {1}", questTemp.RewardExp, questTemp.RewardMoney);
        
        for (int i = 0; i < questTemp.RewardItem.Count; i++)
        {
            if(i==0)
                stringTemp.AppendFormat("\nItem : ");

            stringTemp.AppendFormat("\t{0}\n\t", DataManager.instance.ItemInfo(questTemp.RewardItem[i]).Name);
        }

        questRewardTextUI.text = stringTemp.ToString();
        questRewardTextUI.gameObject.SetActive(true);

        questIndex = questID;
    }

    public bool QuestCompleteNPC()
    {
        questIndex = QuestManager.instance.CompleteQuest(nowNpcStatment.ID);

        if (questIndex == -1) return false;

        QuestViewSetting(questIndex, true);
        IsQuestCompletedUI = true;

        return true;
    }

}
