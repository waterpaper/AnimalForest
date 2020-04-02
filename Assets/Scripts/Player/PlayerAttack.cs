using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //플레이어 공격 클래스입니다.
    public GameObject attackRect;

    private void Start()
    {
        attackRect.SetActive(false);
    }

    public void startAttack()
    {
        attackRect.SetActive(true);
        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_PlayerAttack);
    }

    public void endAttack()
    {
        attackRect.SetActive(false);
    }
}
