using UnityEngine;
using UnityEngine.UI;

public enum PlayerBarKind
{
    PlayerBarKind_HP,
    PlayerBarKind_MP,
    PlayerBarKind_EXP,
    PlayerBarKind_End
}

public class PlayerBarUI : MonoBehaviour
{
    //수치에 따라 fillAmount속성을 변경할 image
    private Image _barImage;
    private GameObject _textUI;

    [Header("BarKind")]
    public PlayerBarKind kind;

    private void Awake()
    {
        _barImage = transform.GetChild(1).GetComponent<Image>();
        _textUI = transform.GetChild(2).gameObject;
    }

    private void LateUpdate()
    {
        //게이지의 fillamount속성을 변경
        if (kind == PlayerBarKind.PlayerBarKind_HP)
        {
            _barImage.fillAmount = PlayerManager.instance.Hp / PlayerManager.instance.HpMax;
            _textUI.GetComponent<TextUI>().kind = TextUI.TEXTKIND.TEXTKIND_PlayerHp;
        }
        else if (kind == PlayerBarKind.PlayerBarKind_MP)
        {
            _barImage.fillAmount = PlayerManager.instance.Mp / PlayerManager.instance.MpMax;
            _textUI.GetComponent<TextUI>().kind = TextUI.TEXTKIND.TEXTKIND_PlayerMp;
        }
        else if (kind == PlayerBarKind.PlayerBarKind_EXP)
        {
            _barImage.fillAmount = (float)PlayerManager.instance.Exp / PlayerManager.instance.ExpMax;
            _textUI.GetComponent<TextUI>().kind = TextUI.TEXTKIND.TEXTKIND_PlayerExp;
        }
    }

}
