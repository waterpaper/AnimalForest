using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMSoundKind
{
    BGMSoundKind_Title,
    BGMSoundKind_StartGame,
    BGMSoundKind_Town,
    BGMSoundKind_BeforeFelid,
    BGMSoundKind_AfterFelid,
    BGMSoundKind_Boss1,
    BGMSoundKind_End
}

public enum EffectSoundKind
{
    //playerSound
    EffectSoundKind_PlayerAttack = 0,
    EffectSoundKind_PlayerRoll,
    EffectSoundKind_PlayerJumpUP,
    EffectSoundKind_PlayerJumpDowm,
    EffectSoundKind_PlayerFootStep,
    EffectSoundKind_PlayerLevelUP,
    EffectSoundKind_PlayerPotion,

    //enemySound
    EffectSoundKind_EnemyHit = 100,

    //BossSound
    EffectSoundKind_Boss1_Hit = 1000,
    EffectSoundKind_Boss1_Attack,
    EffectSoundKind_Boss1_Skiil1_UP,
    EffectSoundKind_Boss1_Skill1_Down,
    EffectSoundKind_Boss1_Skill2,

    //Environment
    EffectSoundKind_Environment_Boxopen,

    EffectSoundKind_End
}

[System.Serializable]
public struct BgmSoundInfo
{
    public BGMSoundKind Kind;
    public AudioClip BgmSoundClip;
}

[System.Serializable]
public struct EffectSoundInfo
{
    public EffectSoundKind Kind;
    public AudioClip EffectSoundClip;
}

public class BGMSound
{
    public BGMSoundKind Kind;
    public AudioSource source;
}

public class EffectSound
{
    public EffectSoundKind Kind;
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    [Header("SoundSetting")]
    //생성할 사운드 오브젝트입니다.
    public GameObject SoundObjectPrefab;
    //effect 사운드의 총 갯수입니다.
    public int effectSoundCount = 10;
    //bgm의 소리 크기입니다.
    public float BGMSoundVolume = 1.0f;
    //effect의 소리 크기입니다.
    public float EffectSoundVolume = 1.0f;
    //각 bgm, effect 사운드 리스트를 인스펙터 창에서 만들어서 설정해줍니다.
    public List<BgmSoundInfo> BgmSoundInfoList;
    public List<EffectSoundInfo> EffectSoundInforList;

    //각 사운드를 재생시킬 AudioSource를 저장하고 있는 변수입니다.
    private BGMSound NowBGMSound;
    private List<EffectSound> NowEffectSoundList;

    //각 사운드를 가지고 있는 딕셔너리입니다.
    private Dictionary<BGMSoundKind, BgmSoundInfo> BGMSoundDictionary;
    private Dictionary<EffectSoundKind, EffectSoundInfo> EffectSoundDictionary;

    //싱긅톤 객체입니다.
    private static SoundManager m_instance;

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

    void Awake()
    {
        //원하는 초기 데이터를 설정해줍니다.
        //오브젝트가 하나 더 생성되었을시 싱글톤개념에 맞지 않기 때문에 파괴합니다.
        if (instance != this)
        {
            Destroy(gameObject);
        }

        ClipSetting();
        DictionarySetting();
    }

    public void OnDisable()
    {
        //매니저 클래스 종료시 생성햇던 객체들을 지워줍니다.
        Destroy(NowBGMSound.source.gameObject);
        foreach (var temp in NowEffectSoundList)
        {
            Destroy(temp.source.gameObject);
        }

        NowEffectSoundList.Clear();
    }

    public void DictionarySetting()
    {
        //인스펙터창에서 설정해준 sound list를 딕셔너리로 바꿔서 검색속도를 올리기 위해 설정해줍니다.
        BGMSoundDictionary = new Dictionary<BGMSoundKind, BgmSoundInfo>();
        EffectSoundDictionary = new Dictionary<EffectSoundKind, EffectSoundInfo>();

        //리스트의 요소를 딕셔너리에 추가해줍니다.
        BgmSoundInfoList.ForEach((temp) => { BGMSoundDictionary.Add(temp.Kind, temp); });
        EffectSoundInforList.ForEach((temp) => { EffectSoundDictionary.Add(temp.Kind, temp); });
    }

    public void ClipSetting()
    {
        //사운드 재생시 사용할 오브젝트를 갯수에 맞게 만들고 clip을 세팅해줍니다.
        //bgm 부분
        var BGMAudioObject = Instantiate<GameObject>(SoundObjectPrefab, transform);
        BGMAudioObject.name = "BGMAudioObject";
        NowBGMSound = new BGMSound();
        NowBGMSound.source = BGMAudioObject.GetComponent<AudioSource>();

        //effectSound 부분
        NowEffectSoundList = new List<EffectSound>();

        for (int i = 0; i < effectSoundCount; i++)
        {
            var effectAudioObjectTemp = Instantiate<GameObject>(SoundObjectPrefab, transform);
            effectAudioObjectTemp.name = string.Format("EffectAudioObject {00}", i);

            EffectSound soundTemp = new EffectSound();
            soundTemp.source = effectAudioObjectTemp.GetComponent<AudioSource>();

            NowEffectSoundList.Add(soundTemp);
        }
    }

    public void BGMPlay(BGMSoundKind kind, bool loop = true)
    {
        //원하는 bgm을 플레이합니다.
        NowBGMSound.Kind = kind;
        NowBGMSound.source.Stop();
        NowBGMSound.source.clip = BGMSoundDictionary[kind].BgmSoundClip;
        NowBGMSound.source.volume = BGMSoundVolume;
        NowBGMSound.source.loop = loop;
        NowBGMSound.source.Play();
    }

    public void EffectSoundPlay(EffectSoundKind kind, bool loop = false)
    {
        //원하는 effect를 플레이합니다
        //그전에 미리 선언한 오브젝트중 effect sound를 아직 출력하지 않는 object에 할당해 재생시킵니다.
        for (int i = 0; i < NowEffectSoundList.Count; i++)
        {
            if (NowEffectSoundList[i].source.isPlaying == false)
            {
                NowEffectSoundList[i].Kind = kind;
                NowEffectSoundList[i].source.clip = EffectSoundDictionary[kind].EffectSoundClip;
                NowEffectSoundList[i].source.volume = EffectSoundVolume;
                NowEffectSoundList[i].source.loop = loop;
                NowEffectSoundList[i].source.Play();
                break;
            }
        }
    }

    public void SoundStop()
    {
        //전체 사운드를 멈춥니다.
        BGMStop();
        EffectSoundStop();
    }

    public void BGMStop()
    {
        //bgm을 멈춥니다.
        NowBGMSound.source.Stop();
    }

    public void EffectSoundStop()
    {
        //effect sound을 멈춥니다.
        foreach (var temp in NowEffectSoundList)
        {
            if (temp.source.isPlaying == true)
            {
                temp.source.Stop();
            }
        }
    }

    public void EffectSoundStop(EffectSoundKind kind)
    {
        //effect sound을 멈춥니다.
        foreach (var temp in NowEffectSoundList)
        {
            if (temp.source.isPlaying == true && temp.Kind == kind)
            {
                temp.source.Stop();
            }
        }
    }

    public void BGMSoundVolumeLeveling(float level)
    {
        //BGM 볼륨을 조절합니다.
        if (level >= 0.0f && level <= 1.0f)
        {
            BGMSoundVolume = level;
            NowBGMSound.source.volume = BGMSoundVolume;
        }
    }

    public void EffectSoundVolumeLeveling(float level)
    {
        //Effect 볼륨을 조절합니다.
        if (level >= 0.0f && level <= 1.0f)
        {
            EffectSoundVolume = level;
            NowEffectSoundList.ForEach((temp) => { temp.source.volume = EffectSoundVolume; });
        }
    }

    public bool IsBGMSound(BGMSoundKind kind)
    {
        //해당 종류의 bgm이 실행중인지 판단합니다.
        if (NowBGMSound.Kind == kind)
            return true;
        else
            return false;
    }

    public bool IsEffectSound(EffectSoundKind kind)
    {
        //해당 종류의 effect가 실행중인지 판단합니다.
        for (int i = 0; i < NowEffectSoundList.Count; i++)
        {
            if (NowEffectSoundList[i].Kind == kind)
                return true;
        }

        return false;
    }
}
