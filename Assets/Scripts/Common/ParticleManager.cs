using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleKind
{
    ParticleKind_PlayerEffect,
    ParticleKind_BattleEffect,
    ParticleKind_Environment,
    ParticleKind_End
}

public enum ParticleName
{
    ParticleName_Player_HpPotion,
    ParticleName_Player_MpPotion,
    ParticleName_Player_LevelUP,
    ParticleName_Battle_PlayerHit,
    ParticleName_Battle_EnemyHit,
    ParticleName_Battle_Boss1Hit,
    ParticleName_Environment_Boxopen,
    ParticleName_End
}

public class ParticleManager : MonoBehaviour
{
    [Header("ParticleLimitCount")]
    public int playerEffectParticleCount = 2;
    public int battleEffectParticleCount = 10;
    public int environmentParticleCount = 5;

    [Header("Pool Path")]
    public GameObject playerEffectPoolPath;
    public GameObject battleEffectPoolPath;
    public GameObject environmentPoolPath;

    [Header("ParticlePool")]
    public List<GameObject> allParticlePrefab;
    public Dictionary<int, List<GameObject>> ParticleObjectPool;

    private static ParticleManager m_instance;

    //싱글톤 접근
    public static ParticleManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ParticleManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        playerEffectPoolPath = Instantiate<GameObject>(new GameObject(),GameObject.FindGameObjectWithTag("Player").transform);
        playerEffectPoolPath.name = "PlayerEffect";

        battleEffectPoolPath = transform.GetChild(0).gameObject;
        environmentPoolPath = transform.GetChild(1).gameObject;

        ParticleObjectPool = new Dictionary<int, List<GameObject>>();

        Setting();
    }

    public void Setting()
    {
        //실행시 파티클을 미리 세팅, 생성합니다.
        allParticlePrefab.ForEach(
           (particleTemp) =>
           {
               ParticleObjectPool.Add((int)particleTemp.GetComponent<Particle>().Name, new List<GameObject>());

               switch (particleTemp.GetComponent<Particle>().Kind)
               {
                   case ParticleKind.ParticleKind_PlayerEffect:
                       CreatParticlePoolContents(particleTemp, playerEffectParticleCount, playerEffectPoolPath);
                       break;
                   case ParticleKind.ParticleKind_BattleEffect:
                       CreatParticlePoolContents(particleTemp, battleEffectParticleCount, battleEffectPoolPath);
                       break;
                   case ParticleKind.ParticleKind_Environment:
                       CreatParticlePoolContents(particleTemp, environmentParticleCount, environmentPoolPath);
                       break;
                   case ParticleKind.ParticleKind_End:
                       break;
                   default:
                       break;
               }

           });
    }

    public void CreatParticlePoolContents(GameObject particleObject, int count, GameObject path)
    {
        //해당 파티클에 맞는 갯수만큼 파티클을 생성합니다.
        for (int i = 0; i < count; i++)
        {
            var temp = Instantiate(particleObject, path.transform);
            ParticleObjectPool[(int)particleObject.GetComponent<Particle>().Name].Add(temp);
            temp.name = string.Format("{00} {01}", temp.name, i);
            temp.SetActive(false);
        }
    }

    public void Play(ParticleName particleName, Transform particleTransform, Vector3 offset)
    {
        //해당 파티클리스트를 검사해 실행중이지 않은 파티클의 위치를 변경후 실행합니다.
        //만약 모두 실행중인경우 리스트 가장 마지막 파티클을 변경시킵니다.

        for (int i = 0; i < ParticleObjectPool[(int)particleName].Count; i++)
        {
            if (ParticleObjectPool[(int)particleName][i].activeSelf == false)
            {
                PlaySetting(ParticleObjectPool[(int)particleName][i], particleTransform, offset);
                return;
            }
        }

        PlaySetting(ParticleObjectPool[(int)particleName][ParticleObjectPool[(int)particleName].Count - 1], particleTransform, offset);
    }

    private void PlaySetting(GameObject playObject, Transform particleTransform, Vector3 offset)
    {
        playObject.transform.position = new Vector3(particleTransform.position.x + offset.x, particleTransform.position.y + offset.y, particleTransform.position.z + offset.z);
        playObject.transform.rotation = particleTransform.rotation;
        playObject.SetActive(true);
    }

}
