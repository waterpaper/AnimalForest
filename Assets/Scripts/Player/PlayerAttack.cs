using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackRect;

    private void Start()
    {
        attackRect.SetActive(false);
    }

    public void startAttack()
    {
        attackRect.SetActive(true);
    }

    public void endAttack()
    {
        attackRect.SetActive(false);
    }
}
