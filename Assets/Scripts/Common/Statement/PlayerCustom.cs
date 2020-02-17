using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustom : MonoBehaviour
{
    public int kindCount;
    public int weaponCount;
    public int armorCount;
    public int shieldCount;

    public int nowKindNum = 0;
    public int nowWeaponNum = 0;
    public int nowArmorNum = 0;
    public int nowShieldNum = 0;

    public List<GameObject> PlayerKindObject;
    public List<GameObject> PlayerArmorObject;
    public List<GameObject> PlayerShieldObject;
    public List<Mesh> PlayerWeaponMesh;
    public GameObject PlayerWeaponObject;

    public GameObject PlayerKindObjectLocation;
    public GameObject PlayerArmorObjectLocation;
    public GameObject PlayerShieldObjectLocation;

    public void Awake()
    {
        for (int i = 0; i < kindCount; i++)
        {
            PlayerKindObject.Add(PlayerKindObjectLocation.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < weaponCount; i++)
        {
            GameObject tempObject = Resources.Load("Animal_Mesh/Sword" + string.Format("{0:D2}", i + 1)) as GameObject;
          
            PlayerWeaponMesh.Add(tempObject.GetComponent<MeshFilter>().sharedMesh);
        }

        for (int i = 0; i < armorCount; i++)
        {
            PlayerArmorObject.Add(PlayerArmorObjectLocation.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < shieldCount; i++)
        {
            PlayerShieldObject.Add(PlayerShieldObjectLocation.transform.GetChild(i).gameObject);
        }
    }

    public bool KindChange(int temp)
    {
        temp -= 1;

        if (PlayerKindObject.Count <= temp || temp <0) return false;

        PlayerKindObject[nowKindNum].SetActive(false);
        PlayerKindObject[temp].SetActive(true);
        nowKindNum = temp;

        return true;
    }

    public bool WeaponChange(int temp)
    {
        temp -= 1;

        if (PlayerWeaponMesh.Count <= temp) return false;

        if(temp < 0)
            PlayerWeaponObject.GetComponent<MeshFilter>().mesh = null;
        else 
            PlayerWeaponObject.GetComponent<MeshFilter>().mesh = PlayerWeaponMesh[temp];

        nowWeaponNum = temp;

        return true;
    }

    public bool ArmorChange(int temp)
    {
        temp -= 1;

        if (PlayerArmorObject.Count <= temp) return false;

        if (nowArmorNum > 0)
            PlayerArmorObject[nowArmorNum].SetActive(false);
        else if(nowArmorNum <= 0)
            PlayerArmorObject[0].SetActive(false);


        if (temp > 0)
            PlayerArmorObject[temp].SetActive(true);
        else if (temp <= 0)
            PlayerArmorObject[0].SetActive(true);

        nowArmorNum = temp;

        return true;
    }

    public bool ShieldChange(int temp)
    {
        temp -= 1;

        if (PlayerKindObject.Count <= temp) return false;

        if (nowShieldNum > -1)
            PlayerShieldObject[nowShieldNum].SetActive(false);
        if (temp > -1)
            PlayerShieldObject[temp].SetActive(true);
        
        nowShieldNum = temp;

        return true;
    }
}
