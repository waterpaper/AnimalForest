using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
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
