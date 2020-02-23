using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossBarUI : MonoBehaviour
{
    private BossStatment _bossstatement = null;
    private TextMeshProUGUI _bossName;
    private TextMeshProUGUI _bossLevel;
    private Image _bossHpImage;

    private void Awake()
    {
        _bossName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _bossHpImage = transform.GetChild(1).GetChild(1).GetComponent<Image>();
        _bossLevel = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _bossstatement = UIManager.instance.nowBossStatement;

        if (_bossstatement == null) return;

        if (_bossstatement != null)
        {
            _bossName.text = _bossstatement.bossName;
            _bossLevel.text = _bossstatement.level.ToString();
            _bossHpImage.fillAmount = 1.0f;
        }
    }

    private void LateUpdate()
    {
        _bossHpImage.fillAmount = (float)_bossstatement.hp / _bossstatement.hpMax;
    }

    public void BossStatementSetting(BossStatment bossState)
    {
        _bossstatement = bossState;
    }
}
