using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackRect;

    private void Start()
    {
        attackRect.SetActive(false);
    }

    public void StartAttack()
    {
        attackRect.SetActive(true);
    }

    public void EndAttack()
    {
        attackRect.SetActive(false);
    }
}
