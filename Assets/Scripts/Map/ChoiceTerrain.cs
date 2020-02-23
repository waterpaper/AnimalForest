using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceTerrain : MonoBehaviour
{
    public int setEvnetID;
    public GameObject terrain_before;
    public GameObject terrain_after;

    private void OnEnable()
    {
        if(PlayerManager.instance.IsClearEventList(setEvnetID) ==true)
        {
            terrain_after.SetActive(true);
            terrain_before.SetActive(false);

            SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_AfterFelid);
        }
        else
        {
            terrain_before.SetActive(true);
            terrain_after.SetActive(false);
        }
    }


    private void LateUpdate()
    {
        gameObject.SetActive(false);
    }
}
