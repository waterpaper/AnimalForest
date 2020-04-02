using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObject : MonoBehaviour
{
    public GameObject setObject;

    private void OnTriggerEnter(Collider other)
    {
        //플레이어가 부딪치게 되면 셋팅된 오브젝트를 켜줍니다.
        if(other.tag== "Player")
        {
            setObject.SetActive(true);
        }
    }
}
