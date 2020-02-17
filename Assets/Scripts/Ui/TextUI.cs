using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class TextUI : MonoBehaviour
{
    public enum TEXTKIND
    {
        TEXTKIND_PlayerName,
        TEXTKIND_PlayerMoney,
        TEXTKIND_PlayerLevel,
        TEXTKIND_PlayerExp,
        TEXTKIND_PlayerHp,
        TEXTKIND_PlayerAddHp,
        TEXTKIND_PlayerMp,
        TEXTKIND_PlayerAddMp,
        TEXTKIND_PlayerAtk,
        TEXTKIND_PlayerAddAtk,
        TEXTKIND_PlayerDef,
        TEXTKIND_PlayerAddDef,
        TEXTKIND_End
    }

    public TextMeshProUGUI textUI;
    public TEXTKIND kind = TEXTKIND.TEXTKIND_End;

    public void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
    }

    public void LateUpdate()
    {
        StringBuilder temp = new StringBuilder();

        switch (kind)
        {
            case TEXTKIND.TEXTKIND_PlayerName:
                temp.Append(PlayerManager.instance.Name);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerMoney:
                temp.Append(PlayerManager.instance.Money);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerLevel:
                temp.Append("Level : ");
                temp.Append(PlayerManager.instance.Level);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerExp:
                temp.Append("Exp : ");
                temp.Append(PlayerManager.instance.Exp);
                temp.Append(" / ");
                temp.Append(PlayerManager.instance.ExpMax);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerHp:
                temp.Append("Hp : ");
                temp.Append(PlayerManager.instance.Hp);
                temp.Append(" / ");
                temp.Append(PlayerManager.instance.HpMax);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerAddHp:
                temp.Append("( + ");
                temp.Append(PlayerManager.instance.AddHp);
                temp.Append(" )");
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerMp:
                temp.Append("Mp : ");
                temp.Append(PlayerManager.instance.Mp);
                temp.Append(" / ");
                temp.Append(PlayerManager.instance.MpMax);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerAddMp:
                temp.Append("( + ");
                temp.Append(PlayerManager.instance.AddMp);
                temp.Append(" )");
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerAtk:
                temp.Append("공격력 : ");
                temp.Append(PlayerManager.instance.Atk);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerAddAtk:
                temp.Append("( + ");
                temp.Append(PlayerManager.instance.AddAtk);
                temp.Append(" )");
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerDef:
                temp.Append("방어력 : ");
                temp.Append(PlayerManager.instance.Def);
                textUI.text = temp.ToString();
                break;
            case TEXTKIND.TEXTKIND_PlayerAddDef:
                temp.Append("( + ");
                temp.Append(PlayerManager.instance.AddDef);
                temp.Append(" )");
                textUI.text = temp.ToString();
                break;
            default:
                break;
        }
    }
}
