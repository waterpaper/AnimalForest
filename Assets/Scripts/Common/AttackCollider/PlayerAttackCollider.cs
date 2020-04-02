using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    //플레이어 공격을 처리하는 클래스입니다.
    //공격당 번호를 매겨 여러번 처리가 이뤄지지 않게 처리합니다.
    public float Damage;
    public int attackNumber = 0;
    
    private void OnEnable()
    {
        attackNumber++;

        if(attackNumber>100000)
        {
            attackNumber = 0;
        }

        if(PlayerManager.instance != null)
        {
            Damage = PlayerManager.instance.Atk;
        }
    }
}
