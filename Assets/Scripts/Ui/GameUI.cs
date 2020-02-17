using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject BossBar { get; private set; }

    void Awake()
    {
        BossBar = transform.Find("BossBar").gameObject;
        BossBar.SetActive(false);
    }

    private void LateUpdate()
    {
        if(UIManager.instance.IsBossUI==true)
        {
            ActiveBossBar();
        }
        else
        {
            DisableBossBar();
        }
    }

    public void ActiveBossBar()
    {
        BossBar.SetActive(true);
    }

    public void DisableBossBar()
    {
        BossBar.SetActive(false);
    }
}
