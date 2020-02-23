using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMap : MonoBehaviour
{
    //맵 이동시 처음에 실행되어 맵에 enemy, npc, boss와 같은 부가정보를 세팅하고 없애주는 클래스입니다.
    //몬스터들이 이동할 수 있는 스폰포인트를 저장하고 있는 프리팹과 생성한 스폰포인트 리스트입니다.
    public GameObject spawnPointPfb;
    public List<Vector3> spawnPointList;

    //해당 맵에서 몬스터 정보를 유지하고 있는 리스트입니다.
    public List<int> monsterKindList;
    //해당 맵에서 npc의 위치를 가지고 있는 리스트입니다.
    public List<NpcLocationData> npcLocationList;
    //해당 맵에서 boss의 위치를 가지고 있는 리스트입니다.
    public List<BossLocationData> bossLocationList;

    void Start()
    {
        //초기화와 맵에 따른 데이터를 세팅해줍니다.
        spawnPointList = new List<Vector3>();
        monsterKindList = new List<int>();
        npcLocationList = new List<NpcLocationData>();
        bossLocationList = new List<BossLocationData>();

        Setting();
    }

    void Setting()
    {
        //현재 맵과 정보를 불러옵니다.
        SceneKind kind = SceneLoader.instance.NowSceneKind();
        
        MapTable nowMapTemp = GetMapTable(kind);

        //정보에 맞게 세팅해줍니다.
        if (nowMapTemp != null)
        {
            //해당 구역은 맵 데이터가 있는 경우에만 처리합니다.
            MonsterSpawnPositionSetting(nowMapTemp);
            MonsterKindSetting(nowMapTemp);
            NpcSetting(nowMapTemp);
            BossSetting(nowMapTemp);
        }

        BGMSetting(kind);

        //세팅 값에 맞게 오브젝트 풀을 바꿔주고 객체를 파괴합니다.
        if (kind != SceneKind.Custom && kind != SceneKind.Title)
            PoolManager.instance.ChangeMap(spawnPointList, monsterKindList, npcLocationList, bossLocationList);

        Destroy(this.gameObject);
    }

    //종류에 맞는 maptable을 datamanager에서 불러오는 함수입니다.
    MapTable GetMapTable(SceneKind kind)
    {
        switch (kind)
        {
            case SceneKind.Town:
                return DataManager.instance.MapInfo("Town");

            case SceneKind.Start:
                return DataManager.instance.MapInfo("Start");

            case SceneKind.Field1:
                return DataManager.instance.MapInfo("Field1");

            case SceneKind.Field1_Boss:
                return DataManager.instance.MapInfo("Field1_Boss");

            default:
                return null;
        }
    }

    void MonsterSpawnPositionSetting(MapTable temp)
    {
        //몬스터 스폰 포인트를 맵 설정에 맞게 생성하고 저장합니다.
        GameObject groupObjTemp = null;

        for (int i = 0; i < temp.spawnNum; i++)
        {
            if (i == 0)
            {
                //스폰 포인트를 모와줄 오브젝트를 생성합니다.
                groupObjTemp = Instantiate<GameObject>(new GameObject());
                groupObjTemp.name = "SpawnPointGroup";
            }

            GameObject objTemp = Instantiate(spawnPointPfb, groupObjTemp.transform);
            objTemp.transform.position = temp.spwanLocation[i];
            spawnPointList.Add(temp.spwanLocation[i]);
        }
    }

    void MonsterKindSetting(MapTable temp)
    {
        //현재 맵에 나타나는 몬스터 id를 저장합니다.
        temp.spwanMonster.ForEach((monsterTemp) => { monsterKindList.Add(monsterTemp); });
    }

    void NpcSetting(MapTable temp)
    {
        //npc정보를 저장합니다.
        temp.NpcLocation.ForEach((locationTemp) => { npcLocationList.Add(locationTemp); });
    }

    void BossSetting(MapTable temp)
    {
        //boss정보를 저장합니다.
        temp.BossLocation.ForEach((locationTemp) => { bossLocationList.Add(locationTemp); });
    }

    void BGMSetting(SceneKind kind)
    {
        //맵 종류에 맞는 bgm을 재생합니다.
        switch (kind)
        {
            case SceneKind.Title:
                SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_Title);
                break;
            case SceneKind.Custom:
                break;
            case SceneKind.Start:
                SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_StartGame);
                break;
            case SceneKind.Town:
                SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_Town);
                break;
            case SceneKind.Field1:
                SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_BeforeFelid);
                break;
            case SceneKind.Field1_Boss:
                SoundManager.instance.BGMPlay(BGMSoundKind.BGMSoundKind_Boss1);
                break;
            default:
                SoundManager.instance.BGMStop();
                break;
        }
    }

    private void OnDestroy()
    {
        spawnPointList.Clear();
        monsterKindList.Clear();
        npcLocationList.Clear();
        bossLocationList.Clear();
    }
}
