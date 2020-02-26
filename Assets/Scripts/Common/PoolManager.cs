using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoolManager : MonoBehaviour
{
    private static PoolManager m_instance;

    [Header("Enemy Pool")]
    //적 캐릭터를 저장할 위치
    public GameObject enemyPoolLocation;
    //적 캐릭터가 나타날 위치를 담을 리스트
    public List<Vector3> spwanPointList;
    //적 캐릭터 생성할 주기
    public float createTime = 10.0f;
    //적 한 캐릭터의 최대 생성 개수
    public int maxEnemy = 5;
    //현재 오브젝트 풀에 활성화되있는 몬스터의 id의 리스트입니다.
    public List<int> nowEnemyIDList;
    //오브젝트 풀에 저장할 에너미들을 가지고 있는 리스트입니다.
    public List<GameObject> enemyPrefabList;
    //오브젝트 풀에 저장할 에너미의 프리팹을 저장하는 딕셔너리입니다.
    public Dictionary<int, GameObject> enemyPrefabDictionary;
    //오브젝트 풀에 저장할 에너미 수입니다.
    public Dictionary<int, List<GameObject>> enemyPool;

    [Header("Boss Pool")]
    //적 보스를 저장할 위치입니다.
    public GameObject BossPoolLocation;
    //현재 오브젝트 풀에 활성화되있는 보스의 id의 리스트입니다.
    public List<int> nowBossIDList;
    //오브젝트 풀에 저장할 보스들을 가지고 있는 리스트입니다.
    public List<GameObject> bossPrefabList;
    //오브젝트 풀에 저장할 보스의 프리팹을 저장하는 딕셔너리입니다.
    public Dictionary<int, GameObject> bossPrefabDictionary;
    //오브젝트 풀에 저장되있는 보스풀입니다.
    public Dictionary<int, GameObject> BossDictionary;

    [Header("Npc Pool")]
    //npc를 저장할 위치를 저장합니다.
    public GameObject NpcPoolLoaction;
    //현재 오브젝트 풀에 활성화되있는 npc의 id의 리스트입니다.
    public List<int> NowNpcIDList;
    //오브젝트 풀에 저장할 npc들의 프리팹을 저장하는 리스트입니다.
    public List<GameObject> NpcPrefabList;
    //오브젝트 풀에 저장할 npc들을 가지고 있는 딕셔너리입니다.
    public Dictionary<int, GameObject> NpcDictionary;

    //싱글톤 접근
    public static PoolManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PoolManager>();
                DontDestroyOnLoad(m_instance);
            }
            return m_instance;
        }
    }

    public void Start()
    {
        if (instance != this)
        {
            Destroy(gameObject);
        }

        //적을 생성해 차일드화할 페이런트 게임오브젝트를 생성
        //프리팹을 딕셔너리에 저장후 원하는 적만 생성합니다.
        enemyPrefabDictionary = new Dictionary<int, GameObject>();
        enemyPool = new Dictionary<int, List<GameObject>>();

        enemyPrefabList.ForEach((enemyPrefab) =>
        {
            int idTemp = enemyPrefab.GetComponent<EnemyStatement>().id;
            enemyPrefabDictionary.Add(idTemp, enemyPrefab);
            enemyPool.Add(idTemp, new List<GameObject>());
        });

        bossPrefabDictionary = new Dictionary<int, GameObject>();
        BossDictionary = new Dictionary<int, GameObject>();

        bossPrefabList.ForEach((bossPrefab) =>
        {
            var bossTemp = Instantiate(bossPrefab, BossPoolLocation.transform);
            int bossIDTemp = bossTemp.GetComponent<BossStatment>().ID;

            bossTemp.name = bossTemp.GetComponent<BossStatment>().name;

            BossDictionary.Add(bossIDTemp, bossTemp);
            bossTemp.SetActive(false);
        });


        NowNpcIDList = new List<int>();
        NpcDictionary = new Dictionary<int, GameObject>();

        NpcPrefabList.ForEach((npcPrefab) =>
        {
            var npcTemp = Instantiate(npcPrefab, NpcPoolLoaction.transform);
            int npcIDTemp = npcTemp.GetComponent<NpcStatment>().ID;

            npcTemp.GetComponent<NpcStatment>().Setting(DataManager.instance.NpcInfo(npcIDTemp));
            npcTemp.name = npcTemp.GetComponent<NpcStatment>().name;

            NpcDictionary.Add(npcIDTemp, npcTemp);
            npcTemp.SetActive(false);
        });
    }

    //enemy를 다시 acrive시켜주는 함수입니다.
    public void EnableEnemy(int id)
    {
        StartCoroutine(IEEnableEnemy(id));
    }

    //적 캐릭터를 생성하는 코루틴함수
    IEnumerator IEEnableEnemy(int id, bool isAllActive = false)
    {
        //적 캐릭터의 생성 주기 시간만큼 대기합니다.
        if(!isAllActive)
            yield return new WaitForSeconds(createTime);

        for (int i = 0; i < maxEnemy; i++)
        {
            //비황성화 여부로 사용 가능한 오브젝트인지를 판단
            if (enemyPool[id][i].activeSelf == false)
            {
                //불 규칙적인 위치 산출
                int idx = Random.Range(0, spwanPointList.Count);
                //적 캐릭터를 재 위치시킨다.
                enemyPool[id][i].transform.position = new Vector3(spwanPointList[idx].x, 3.0f, spwanPointList[idx].z);

                enemyPool[id][i].SetActive(true);

                if(!isAllActive)
                    break;
            }
        }
        yield return null;
    }

    //오브젝트 풀에 적을 생성하는 함수
    public void CreateEnemyPooling(EnemyTable enemy)
    {
        //풀링개수만큼 미리 적을 생성합니다.
        for (int i = 0; i < maxEnemy; i++)
        {
            var obj = Instantiate<GameObject>(enemyPrefabDictionary[enemy.ID], enemyPoolLocation.transform);
            obj.name = enemy.Name + i.ToString("00");
            //비활성화
            obj.SetActive(false);

            enemyPool[enemy.ID].Add(obj);
        }
    }

    //맵이 이동시 현재 맵에 맞는 pool의 세팅을 도와주는 함수입니다.
    public void ChangeMap(List<Vector3> spwanListTemp, List<int> monsterListTemp, List<NpcLocationData> npcLocationListTemp, List<BossLocationData> bossLocationListTemp)
    {
        //전 맵의 유지 데이터를 지워줍니다.
        DeleteEnemy();
        EnableNpc();
        EnableBoss();

        //바꿔줄 맵의 유지 데이터를 저장합니다.
        ChangeMap_MonsterSpwanList(spwanListTemp);
        ChangeMap_MonsterKindList(monsterListTemp);
        ChangeMap_NpcActive(npcLocationListTemp);
        ChangeMap_BossActive(bossLocationListTemp);

        //바꿔줄 맵의 유지데이터를 바탕으로 pool의 세팅을 진행합니다.
        SettingPool();

        UIManager.instance.BossUISetting();
    }

    //현재 조건에 맞게 오브젝트 풀을 세팅해줍니다. 
    public void SettingPool()
    {
        nowEnemyIDList.ForEach((monsterID) => { CreateEnemyPooling(DataManager.instance.EnemyInfo(monsterID)); });
        ActivePool();
    }

    public void ActivePool()
    {
        ActiveEnemyPooling();
    }

    public void ActiveEnemyPooling()
    {
        nowEnemyIDList.ForEach((monsterID) => { StartCoroutine(IEEnableEnemy(monsterID, true)); });
    }

    //현재 생성되어 있는 enemy를 모두 제거하기 위한 함수입니다.
    public void DeleteEnemy()
    {
        //맵이동시 사용됩니다.
        //딕셔너리에서 선택된 enemy리스트입니다.
        List<GameObject> selectEnemyList;

        for (int i = 0; i < nowEnemyIDList.Count; i++)
        {
            //딕셔너리에 저장된 현재 존재하는 enemy가 가진 list
            selectEnemyList = enemyPool[nowEnemyIDList[i]];

            //해당 enemy list의 객체를 모두 파괴합니다.
            for (int j = 0; j < selectEnemyList.Count; j++)
            {
                Destroy(selectEnemyList[j]);
            }

            enemyPool[nowEnemyIDList[i]].Clear();
        }

        nowEnemyIDList.Clear();
    }
    //오브젝트에 활성화되있는 npc객체를 맵 체인지시 비활성화 시키기 위한 함수입니다.
    public void EnableNpc()
    {
        NowNpcIDList.ForEach((i) => { NpcDictionary[i].SetActive(false); });

        NowNpcIDList.Clear();
    }

    //오브젝트에 활성화되있는 boss객체를 맵 체인지시 비활성화 시키기 위한 함수입니다.
    public void EnableBoss()
    {
        nowBossIDList.ForEach((i) => { BossDictionary[i].SetActive(false); });

        nowBossIDList.Clear();
    }

    //해당하는 맵에 스폰시킬 몬스터들의 아이디를 저장하는 함수입니다.
    void ChangeMap_MonsterKindList(List<int> monsterIDList)
    {
        nowEnemyIDList.Clear();

        monsterIDList.ForEach((monsterID) => { nowEnemyIDList.Add(monsterID); });
    }

    //해당하는 맵 몬스터 스폰 리스트를 변화시켜주는 함수
    void ChangeMap_MonsterSpwanList(List<Vector3> spwanList)
    {
        spwanPointList.Clear();

        for (int i = 0; i < spwanList.Count; i++)
        {
            spwanPointList.Add(spwanList[i]);
        }
    }

    //해당하는 맵에 npc를 활성화해주는 함수입니다.
    void ChangeMap_NpcActive(List<NpcLocationData> npcLocationListTemp)
    {
        npcLocationListTemp.ForEach((LocationData) =>
        {
            NowNpcIDList.Add(LocationData.NpcID);
            NpcDictionary[LocationData.NpcID].transform.position = LocationData.Location;
            NpcDictionary[LocationData.NpcID].transform.rotation = Quaternion.Euler(LocationData.Rotation);
            NpcDictionary[LocationData.NpcID].SetActive(true);
        });
    }

    //해당하는 맵에 boss를 활성화해주는 함수입니다.
    void ChangeMap_BossActive(List<BossLocationData> bossLocationListTemp)
    {
        bossLocationListTemp.ForEach((LocationData) =>
        {
            nowBossIDList.Add(LocationData.BossID);
            BossDictionary[LocationData.BossID].transform.position = LocationData.Location;
            BossDictionary[LocationData.BossID].SetActive(true);
        });
    }

    //npc에 퀘스트 데이터를 삭제해주는 함수입니다.
    public void NpcQuestDelete(int npcID, int questIndex)
    {
        NpcDictionary[npcID].GetComponent<NpcStatment>().DeleteNpcQuest(questIndex);
    }


}
