using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [Header("SoundSetting")]
    public GameObject AudioObjectPrefab;
    public int effectAudioCount = 20;
    public float BGMAudioVolume = 1.0f;
    public float EffectAudioVolume = 1.0f;

    private static SoundManager m_instance;
    private GameObject BGMAudioObject;
    private List<GameObject> EffectAudioObject;


    //싱글톤 접근
    public static SoundManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    void Start()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        BGMAudioObject = Instantiate<GameObject>(AudioObjectPrefab, transform);
        BGMAudioObject.name = "BGMAudioObject";

        EffectAudioObject = new List<GameObject>();

        for (int i = 0; i < effectAudioCount; i++)
        {
            var effectAudioObjectTemp = Instantiate<GameObject>(AudioObjectPrefab, transform);
            effectAudioObjectTemp.name = string.Format("EffectAudioObject {00}", i);

            EffectAudioObject.Add(effectAudioObjectTemp);
        }
    }

    public void BGMPlay(string str, bool loop = false)
    {
        BGMAudioObject.GetComponent<AudioSource>().Stop();
        BGMAudioObject.GetComponent<AudioSource>().clip = DataManager.instance.SoundInfo(str);
        BGMAudioObject.GetComponent<AudioSource>().volume = BGMAudioVolume;
        BGMAudioObject.GetComponent<AudioSource>().Play();
    }

    public void EffectSoundPlay(string str,bool loop = false)
    {
        foreach (var temp in EffectAudioObject)
        {
            if (temp.activeSelf == false)
            {
                temp.GetComponent<AudioSource>().clip = DataManager.instance.SoundInfo(str);
                temp.GetComponent<AudioSource>().volume = EffectAudioVolume;
                temp.GetComponent<AudioSource>().Play();
                temp.SetActive(true);
                break;
            }
        }
    }

    public void BGMStop()
    {
        BGMAudioObject.GetComponent<AudioSource>().Stop();
    }

    public void EffectSoundStop()
    {
        foreach (var temp in EffectAudioObject)
        {
            if (temp.activeSelf == true)
            {
                temp.GetComponent<AudioSource>().Stop();
                temp.SetActive(false);
            }
        }
    }
    
    public void OnDisable()
    {
        Destroy(BGMAudioObject);
        foreach (var temp in EffectAudioObject)
        {
            Destroy(temp);
        }

        EffectAudioObject.Clear();
    }
}
