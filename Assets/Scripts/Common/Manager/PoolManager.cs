using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoolManager : SingletonMonoBehaviour<PoolManager>
{
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
    public GameObject bossPoolLocation;
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
    public GameObject npcPoolLocation;
    //현재 오브젝트 풀에 활성화되있는 npc의 id의 리스트입니다.
    public List<int> nowNpcIDList;
    //오브젝트 풀에 저장할 npc들의 프리팹을 저장하는 리스트입니다.
    public List<GameObject> npcPrefabList;
    //오브젝트 풀에 저장할 npc의 프리팹을 저장하는 딕셔너리입니다.
    public Dictionary<int, GameObject> npcPrefabDictionary;
    //오브젝트 풀에 저장할 npc들을 가지고 있는 딕셔너리입니다.
    public Dictionary<int, GameObject> npcDictionary;

    public void Awake()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        //오브젝트 풀을 구현하기 위해 리스트에 입력받은 프리팹들을 딕셔너리로 바꿔 저장해 줍니다.
        nowEnemyIDList = new List<int>();
        nowBossIDList = new List<int>();
        nowNpcIDList = new List<int>();

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
            int bossIDTemp = bossPrefab.GetComponent<BossStatement>().ID;
            bossPrefabDictionary.Add(bossIDTemp, bossPrefab);
            BossDictionary.Add(bossIDTemp, null);
        });

        npcPrefabDictionary = new Dictionary<int, GameObject>();
        npcDictionary = new Dictionary<int, GameObject>();

        npcPrefabList.ForEach((npcPrefab) =>
        {
            int npcIDTemp = npcPrefab.GetComponent<NpcStatment>().ID;
            npcPrefabDictionary.Add(npcIDTemp, npcPrefab);
            npcDictionary.Add(npcIDTemp, null);
        });

        enemyPrefabList.Clear();
        bossPrefabList.Clear();
        npcPrefabList.Clear();
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

    //enemy를 다시 active시켜주는 함수입니다.
    public void EnableEnemy(int id)
    {
        StartCoroutine(IEEnableEnemy(id));
    }

    //적 캐릭터를 생성하는 코루틴함수
    IEnumerator IEEnableEnemy(int id, bool isAllActive = false)
    {
        //적 캐릭터의 생성 주기 시간만큼 대기합니다.
        if (!isAllActive)
            yield return new WaitForSeconds(createTime);

        for (int i = 0; i < maxEnemy; i++)
        {
            //적이 현재 맵에 있는 몬스터면 리스폰을 합니다.
            if (nowEnemyIDList[i] == id)
                break;

            //만약 적이 현재 맵에 없는 몬스터면 리스폰 하지 않고 넘어갑니다.
            if (nowEnemyIDList.Count - 1 == i)
                yield return null;
        }

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

                if (!isAllActive)
                    break;
            }
        }
        yield return null;
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
    //enemy를 활성화해주는 함수입니다.
    public void ActiveEnemyPooling()
    {
        nowEnemyIDList.ForEach((monsterID) => { StartCoroutine(IEEnableEnemy(monsterID, true)); });
    }

    //현재 생성되있는 npc객체를 모두 제거하기 위한 함수입니다.
    public void DeleteNpc()
    {
        nowNpcIDList.ForEach((i) =>
        {
            Destroy(npcDictionary[i]);
            npcDictionary[i] = null;
        });

        nowNpcIDList.Clear();
    }

    //npc를 active, disactive하는 함수입니다.
    public void ActiveNpc(int id = -1, bool isActive = true)
    {
        if (id != -1)
        {
            npcDictionary[id].SetActive(isActive);
        }
        else
        {
            nowNpcIDList.ForEach((npcID) =>
            {
                npcDictionary[npcID].SetActive(isActive);
            });
        }
    }

    //현재 생성되있는 boss객체를 모두 제거하기 위한 함수입니다.
    public void DeleteBoss()
    {
        nowBossIDList.ForEach((i) =>
        {
            Destroy(BossDictionary[i]);
            BossDictionary[i] = null;
        });

        nowBossIDList.Clear();
    }

    //npc를 active, disactive하는 함수입니다.
    public void ActiveBoss(int id = -1, bool isActive = true)
    {
        if (id != -1)
        {
            BossDictionary[id].SetActive(isActive);
        }
        else
        {
            nowBossIDList.ForEach((BossID) =>
            {
                BossDictionary[BossID].SetActive(isActive);
            });
        }
    }


    //맵이 이동시 현재 맵에 맞는 pool의 세팅을 도와주는 함수입니다.
    public void ChangeMap(List<Vector3> spwanListTemp, List<int> monsterListTemp, List<NpcLocationData> npcLocationListTemp, List<BossLocationData> bossLocationListTemp)
    {
        //전 맵의 유지 데이터를 지워줍니다.
        DeleteEnemy();
        DeleteNpc();
        DeleteBoss();

        //바꿔줄 맵의 데이터를 저장, 생성합니다.
        ChangeMap_MonsterSpwanList(spwanListTemp);
        ChangeMap_Monster(monsterListTemp);
        ChangeMap_Npc(npcLocationListTemp);
        ChangeMap_Boss(bossLocationListTemp);

        //바꿔줄 맵의 유지데이터를 바탕으로 pool의 활성화를 진행합니다.
        ActivePool();
    }

    //현재 조건에 맞는 오브젝트 풀을 활성화해줍니다. 
    public void ActivePool()
    {
        ActiveEnemyPooling();
        ActiveNpc();
        ActiveBoss();
    }


    //해당하는 맵에 스폰시킬 몬스터들의 아이디를 저장하는 함수입니다.
    void ChangeMap_Monster(List<int> monsterIDList)
    {
        nowEnemyIDList.Clear();

        monsterIDList.ForEach((monsterID) =>
        {
            nowEnemyIDList.Add(monsterID);
            CreateEnemyPooling(DataManager.instance.EnemyInfo(monsterID));
        });
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

    //해당하는 맵에 npc를 생성해주는 함수입니다.
    void ChangeMap_Npc(List<NpcLocationData> npcLocationListTemp)
    {
        npcLocationListTemp.ForEach((LocationData) =>
        {
            nowNpcIDList.Add(LocationData.NpcID);

            var npcTemp = Instantiate<GameObject>(npcPrefabDictionary[LocationData.NpcID], npcPoolLocation.transform);
            npcDictionary[LocationData.NpcID] = npcTemp;

            npcTemp.transform.position = LocationData.Location;
            npcTemp.transform.rotation = Quaternion.Euler(LocationData.Rotation);
            npcTemp.SetActive(false);
        });
    }

    //해당하는 맵에 boss를 생성해주는 함수입니다.
    void ChangeMap_Boss(List<BossLocationData> bossLocationListTemp)
    {
        bossLocationListTemp.ForEach((LocationData) =>
        {
            nowBossIDList.Add(LocationData.BossID);

            var bossTemp = Instantiate<GameObject>(bossPrefabDictionary[LocationData.BossID], bossPoolLocation.transform);
            BossDictionary[LocationData.BossID] = bossTemp;

            bossTemp.transform.position = LocationData.Location;
            bossTemp.SetActive(false);
        });
    }

    //npc에 퀘스트 데이터를 삭제해주는 함수입니다.
    public void NpcQuestDelete(int npcID, int questIndex)
    {
        if (npcDictionary[npcID] == null) return;

        NpcStatment npcStatementTemp = npcDictionary[npcID].GetComponent<NpcStatment>();
        npcStatementTemp.DeleteNpcQuest(questIndex);
    }


}
