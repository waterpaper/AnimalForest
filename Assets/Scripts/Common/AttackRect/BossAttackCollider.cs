using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCollider : MonoBehaviour
{
    public float Damage { get; private set; }
    public BoxCollider collider;
    public GameObject particle = null;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        collider.gameObject.SetActive(true);
    }

    public void Setting(float bossDamage)
    {
        Damage = bossDamage;
    }

    public void Setting(float bossDamage, float range)
    {
        Damage = bossDamage;
        collider.size.Set(collider.size.x, collider.size.y, range);
    }

}
