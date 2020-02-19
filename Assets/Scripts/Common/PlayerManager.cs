using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerCustomKind
    {
        PlayerCustomKind_AnimalKind,
        PlayerCustomKind_AnimalWeapon,
        PlayerCustomKind_AnimalArmor,
        PlayerCustomKind_AnimalShield,
        PlayerCustomKind_End,
    }

    private PlayerState _playerState = null;
    private PlayerCustom _playerCustom = null;
    private Rigidbody _rigidbody;
    private static PlayerManager m_instance;

    //싱글톤 접근
    public static PlayerManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<PlayerManager>();
                DontDestroyOnLoad(m_instance);

                if (m_instance._playerState == null)
                {
                    return null;
                }
            }
            else if(m_instance._playerState == null)
            {
                return null;
            }

            return m_instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerState = GetComponent<PlayerState>();
        _playerCustom = GetComponent<PlayerCustom>();
        _rigidbody = GetComponent<Rigidbody>();

        PlayerData info = DataManager.instance.playerInfo();
        _playerState.loadCharacterStatement(info);
    }

    public int Id
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
            _playerState.hpMax += temp;
            _playerState.hp += temp;
        }
    }
    public float AddMp
    {
        get { return _playerState.addMp; }
        set
        {
            float temp = value - _playerState.addMp;

            _playerState.addMp = value;
            _playerState.mpMax += temp;
            _playerState.mp += temp;
        }
    }
    public float AddAtk
    {
        get { return _playerState.addAtk; }
        set
        {
            float temp = value - _playerState.addAtk;

            _playerState.addAtk = value;
            _playerState.atk += temp;
        }
    }
    public float AddDef
    {
        get { return _playerState.addDef; }
        set
        {
            float temp = value - _playerState.addDef;

            _playerState.addDef = value;
            _playerState.def += temp;
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
        for(int i=0;i<_playerState.clearEventList.Count;i++)
        {
            if (id == _playerState.clearEventList[i])
                return true;
        }
        return false;
    }

    public void AddClearEventList(int id)
    {
        _playerState.clearEventList.Add(id);
    }

    private void LevelUp()
    {
        Level++;
        Exp -= ExpMax;

        PlayerLevelTable temp = DataManager.instance.PlayerLevelInfo(Level);

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
        if(kind ==PlayerCustomKind.PlayerCustomKind_AnimalKind)
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

}
