using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /*
    public GameObject spawnPointPfb;
    public List<GameObject> spawnPointGroup;

    private static MapManager m_instance;
    private MapTable _mapTemp;

    //싱글톤 접근
    public static MapManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MapManager>();
            }
            return m_instance;
        }
    }

    public void mapChange()
    {
        Setting();
        PoolManager.instance.ChangeMap(_mapTemp.spwanLocation, _mapTemp.spwanMonster);
    }

    void Setting()
    {
        _mapTemp = null;
        SceneKind kind = SceneLoader.instance.NowSceneKind();

        if (kind == SceneKind.Field1)
        {
            _mapTemp = DataManager.instance.MapInfo("Field1");
        }
        else if(kind == SceneKind.Town){
            _mapTemp = DataManager.instance.MapInfo("Town");
        }
        
        else if(){
            mapTemp = DataManager.instance.MapInfo("Field1");
        }
        else if(){
            mapTemp = DataManager.instance.MapInfo("Field1");
        }
        
        if (mapTemp != null)
        {
            monsterSpawnPositionSetting(mapTemp);
        }
        
    }

    
    void monsterSpawnPositionSetting(MapTable temp)
    {
        int max = temp.spawnNum;

        for (int i = 0; i < max; i++)
        {
            GameObject objTemp = Instantiate(spawnPointPfb, temp.spwanLocation[i], Quaternion.identity);
            spawnPointGroup.Add(objTemp);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < spawnPointGroup.Count; i++)
        {
            Destroy(spawnPointGroup[i]);
        }
    }
    */
}
