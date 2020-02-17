using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObject : MonoBehaviour
{
    public GameObject setObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag== "Player")
        {
            setObject.SetActive(true);
        }
    }
}
