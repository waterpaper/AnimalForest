using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRect : MonoBehaviour
{
    public EnemyStatement enemyStatement;
    public float damage;

    // Start is called before the first frame update
    void Awake()
    {
        enemyStatement = GetComponentInParent<EnemyStatement>();
    }

    private void OnEnable()
    {
        damage = enemyStatement.atk;
    }
}
