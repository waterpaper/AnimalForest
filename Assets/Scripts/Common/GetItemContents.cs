using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class GetItemContents : MonoBehaviour
{
    private TextMeshProUGUI _textUI;

    private void Awake()
    {
        _textUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void Setting(string itemNameText, int count)
    {
        StringBuilder stringTemp = new StringBuilder(string.Format("< {00} >", itemNameText));

        if(count == 1)
        {
            stringTemp.Append(" 를 획득");
        }
        else
        {
            stringTemp.Append(string.Format("x {01} 를 획득", itemNameText));
        }

        _textUI.text = stringTemp.ToString();

        Destroy(gameObject, 3.0f);
    }
}
