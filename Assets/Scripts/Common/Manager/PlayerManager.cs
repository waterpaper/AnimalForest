using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    private PlayerState _playerState = null;
    private PlayerCustom _playerCustom = null;
    private Rigidbody _rigidbody;

    private Save _savePlayerData;
    private Load _loadPlayerData;
    
    //로드 아이템 정보 세팅시 statement의 기본 값을 건들이지 않기 위한 변수입니다.(데이터베이스에 저장되어 있음)
    private bool _isLoad = false;

    [Header("PlayerSaveFile")]
    public string playerData_FileName = "Player";

    public void Awake()
    {
        if (instance != this)
        {
            Destroy(this.gameObject);
        }
        else
            DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _playerState = GetComponent<PlayerState>();
        _playerCustom = GetComponent<PlayerCustom>();
        _rigidbody = GetComponent<Rigidbody>();

        _savePlayerData = new Save();
        _loadPlayerData = new Load(); 

        _playerState.StartPlayerSetting();
        transform.position = new Vector3(0.0f, 5.0f, 0.0f);
    }

    public string Id
    {
        get { return _playerState.id; }
        set
        {
            _playerState.id = value;
        }
    }
    public string Name
    {
        get { return _playerState.name; }
        set
        {
            _playerState.name = value;
        }
    }
    public int Kind
    {
        get { return _playerState.kind; }
        set
        {
            _playerState.kind = value;
        }
    }
    public float Hp
    {
        get { return _playerState.hp; }
        set
        {
            if (value > HpMax)
                _playerState.hp = HpMax;
            else if (value < 0)
                _playerState.hp = 0;
            else
                _playerState.hp = value;
        }
    }
    public float HpMax
    {
        get { return _playerState.hpMax; }
        set
        {
            _playerState.hpMax = value;
        }
    }
    public float Mp
    {
        get { return _playerState.mp; }
        set
        {
            if (value > MpMax)
                _playerState.mp = MpMax;
            else
                _playerState.mp = value;
        }
    }
    public float MpMax
    {
        get { return _playerState.mpMax; }
        set
        {
            _playerState.mpMax = value;
        }
    }
    public float Atk
    {
        get { return _playerState.atk; }
        set
        {
            _playerState.atk = value;
        }
    }
    public float Def
    {
        get { return _playerState.def; }
        set
        {
            _playerState.def = value;
        }
    }
    public float AddHp
    {
        get { return _playerState.addHp; }
        set
        {
            float temp = value - _playerState.addHp;

            _playerState.addHp = value;

            if (_isLoad)
            {
                _playerState.hpMax += temp;
                _playerState.hp += temp;
            }
        }
    }
    public float AddMp
    {
        get { return _playerState.addMp; }
        set
        {
            float temp = value - _playerState.addMp;

            _playerState.addMp = value;

            if (_isLoad)
            {
                _playerState.mpMax += temp;
                _playerState.mp += temp;
            }
        }
    }
    public float AddAtk
    {
        get { return _playerState.addAtk; }
        set
        {
            float temp = value - _playerState.addAtk;

            _playerState.addAtk = value;

            if (_isLoad)
            {
                _playerState.atk += temp;
            }
        }
    }
    public float AddDef
    {
        get { return _playerState.addDef; }
        set
        {
            float temp = value - _playerState.addDef;

            _playerState.addDef = value;

            if (_isLoad)
            {
                _playerState.def += temp;
            }
        }
    }

    public int Level
    {
        get { return _playerState.level; }
        set
        {
            _playerState.level = value;
        }
    }
    public int Exp
    {
        get { return _playerState.exp; }
        set
        {
            _playerState.exp = value;

            if (value >= ExpMax)
                LevelUp();
        }
    }
    public int ExpMax
    {
        get { return _playerState.expMax; }
        set
        {
            _playerState.expMax = value;
        }
    }
    public int Money
    {
        get { return _playerState.money; }
        set
        {
            _playerState.money = value;
        }
    }

    public bool IsClearEventList(int id)
    {
        for (int i = 0; i < _playerState.clearEventList.Count; i++)
        {
            if (id == _playerState.clearEventList[i])
                return true;
        }
        return false;
    }
    public bool IsClearQuestList(int id)
    {
        for (int i = 0; i < _playerState.clearQuestList.Count; i++)
        {
            if (id == _playerState.clearQuestList[i])
                return true;
        }
        return false;
    }

    public void Load(string id)
    {
        //플레이더 아이디에 맞는 데이터 연결을 시도합니다.
        _loadPlayerData.LoadConnection(id);
    }

    public void LoadPlayerSetting(PlayerSaveData loadData)
    {
        //데이터를 로드하면서 캐릭터 설정을 합니다.
        _playerState.LoadPlayerSetting(loadData);

        //캐릭터 외형을 변경시키는 세팅을합니다.
        CustomSetting(PlayerCustomKind.PlayerCustomKind_AnimalKind, loadData.Kind);
    }

    public PlayerState GetPlayerState()
    {
        return _playerState;
    }

    public void AddClearEventList(int id)
    {
        _playerState.clearEventList.Add(id);
        _playerState.clearEventList.Sort();
    }

    public void AddClearQuestList(int id)
    {
        _playerState.clearQuestList.Add(id);
        _playerState.clearQuestList.Sort();
    }

    private void LevelUp()
    {
        Level++;
        Exp -= ExpMax;

        PlayerLevelTable temp = DataManager.instance.GetTableData<PlayerLevelTable>(TableDataKind.TableDataKind_PlayerLevel, Level);

        ExpMax = temp.ExpMax;
        Hp += temp.AddHp;
        Mp += temp.AddMp;
        Atk += temp.AddAtk;
        Def += temp.AddDef;

        ParticleManager.instance.Play(ParticleName.ParticleName_Player_LevelUP, transform, Vector3.zero);
        SoundManager.instance.EffectSoundPlay(EffectSoundKind.EffectSoundKind_PlayerLevelUP);
    }

    public bool CustomSetting(PlayerCustomKind kind, int temp)
    {
        if (kind == PlayerCustomKind.PlayerCustomKind_AnimalKind)
        {
            if (_playerCustom.KindChange(temp) == true)
            {
                Kind = temp;
                return true;
            }
            else return false;
        }
        else if (kind == PlayerCustomKind.PlayerCustomKind_AnimalWeapon)
        {
            if (_playerCustom.WeaponChange(temp) == true) return true;
            else return false;
        }
        else if (kind == PlayerCustomKind.PlayerCustomKind_AnimalArmor)
        {
            if (temp > 16) temp -= 16;
            if (_playerCustom.ArmorChange(temp) == true) return true;
            else return false;
        }
        else if (kind == PlayerCustomKind.PlayerCustomKind_AnimalShield)
        {
            if (temp > 8) temp -= 8;
            if (_playerCustom.ShieldChange(temp) == true) return true;
            else return false;
        }

        return false;
    }

    public void PosistionSetting()
    {
        MoveStop();

        if (SceneLoader.instance.NowSceneKind() == SceneKind.Start)
        {
            this.transform.position = new Vector3(89.0f, 0.0f, 38.0f);
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            CameraManager.instance.InitSight(new Vector3(0.0f, 180.0f, 0.0f));
        }
        else if (SceneLoader.instance.NowSceneKind() == SceneKind.Town)
        {
            this.transform.position = new Vector3(97.0f, 54.0f, 89.0f);
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
            this.transform.position = SceneLoader.instance.playerLocationTemp;
    }

    public void Knockback(Vector3 position)
    {
        Vector3 temp = new Vector3(position.x - transform.position.x, 0.0f, position.z - transform.position.z);

        _rigidbody.AddForce(temp.normalized * 10.0f);
    }

    public void MoveStop()
    {
        _rigidbody.velocity = Vector3.zero;
    }


    public void Save()
    {
        _savePlayerData.SaveData();
    }

    public void LoadEnd()
    {
        //로드가 끝나면 세팅변수를 true로 바꿔줍니다
        _isLoad = true;
    }
}
