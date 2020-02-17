using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMap : MonoBehaviour
{
    public MapTable nowMapTemp;
    public GameObject spawnPointPfb;
    public List<Vector3> spawnListTemp;
    public List<int> monsterKindListTemp;
    public List<NpcLocationData> npcLocationListTemp;
    public List<BossLocationData> bossLocationListTemp;

    void Start()
    {
        spawnListTemp = new List<Vector3>();
        monsterKindListTemp = new List<int>();
        npcLocationListTemp = new List<NpcLocationData>();
        bossLocationListTemp = new List<BossLocationData>();

        SceneKind kind = SceneLoader.instance.NowSceneKind();

        if(kind == SceneKind.Town)
        {
            nowMapTemp = DataManager.instance.MapInfo("Town");
        }
        else if (kind == SceneKind.Start)
        {
            nowMapTemp = DataManager.instance.MapInfo("Start");
        }
        else if (kind == SceneKind.Field1)
        {
            nowMapTemp = DataManager.instance.MapInfo("Field1");
        }
        else if (kind == SceneKind.Field1_Boss)
        {
            nowMapTemp = DataManager.instance.MapInfo("Field1_Boss");
        }

        Setting(nowMapTemp);
    }

    void Setting(MapTable temp)
    {
        MonsterSpawnPositionSetting(temp);
        MonsterKindSetting(temp);
        NpcSetting(temp);
        BossSetting(temp);

        PoolManager.instance.ChangeMap(spawnListTemp, monsterKindListTemp, npcLocationListTemp, bossLocationListTemp);
        Destroy(this.gameObject);
    }

    void MonsterSpawnPositionSetting(MapTable temp)
    {
        for (int i = 0; i < temp.spawnNum; i++)
        {
            GameObject objTemp = Instantiate(spawnPointPfb, temp.spwanLocation[i], Quaternion.identity);
            spawnListTemp.Add(objTemp.transform.position);
        }
    }

    void MonsterKindSetting(MapTable temp)
    {
        for (int i = 0; i < temp.spwanMonster.Count; i++)
        {
            monsterKindListTemp.Add(temp.spwanMonster[i]);
        }
    }

    void NpcSetting(MapTable temp)
    {
        temp.NpcLocation.ForEach((locationTemp) => { npcLocationListTemp.Add(locationTemp); });
    }

    void BossSetting(MapTable temp)
    {
        temp.BossLocation.ForEach((locationTemp) => { bossLocationListTemp.Add(locationTemp); });
    }

    private void OnDestroy()
    {
        spawnListTemp.Clear();
    }
}
