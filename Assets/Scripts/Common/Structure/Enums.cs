
/// <summary>
/// data enum
/// </summary>
public enum TableDataKind
{
    TableDataKind_Character,
    TableDataKind_Enemy,
    TableDataKind_Boss,
    TableDataKind_Item,
    TableDataKind_Quest,
    TableDataKind_Map,
    TableDataKind_Shop,
    TableDataKind_Npc,
    TableDataKind_EnemyDropItem,
    TableDataKind_SingleConversation,
    TableDataKind_PlayerLevel,
    TableDataKind_End
}
public enum IconDataKind
{
    IconDataKind_Character,
    IconDataKind_Item,
    IconDataKind_End
}

/// <summary>
/// player enum
/// </summary>
public enum PlayerCustomKind
{
    PlayerCustomKind_AnimalKind,
    PlayerCustomKind_AnimalWeapon,
    PlayerCustomKind_AnimalArmor,
    PlayerCustomKind_AnimalShield,
    PlayerCustomKind_End,
}


/// <summary>
/// enemy enum
/// </summary>
public enum EnemyAction
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Hit,
    Die,
    End
}

/// <summary>
/// npc enum
/// </summary>
public enum NPCTYPE
{
    NPCTYPE_General,
    NPCTYPE_Quest,
    NPCTYPE_WeaponShop,
    NPCTYPE_VarietyShop,
    NPCTYPE_End
}

/// <summary>
/// boss enum
/// </summary>
public enum BossAction
{
    Idle,
    Patrol,
    Trace,
    Attack,
    Skill1,
    Skill2,
    Hit,
    Die,
    End
}

/// <summary>
/// quest enum
/// </summary>
public enum EQuestKind
{
    EQuestKind_Hunting,
    EQuestKind_Collection,
    EQuestKind_Relay,
    EQuestKind_BossHunting,
    EQuestKind_End
}
public enum EQuestProgress
{
    EQuestProgress_Proceeding,
    EQuestProgress_Completed,
    EQuestProgress_End
}

/// <summary>
/// sound enums
/// </summary>
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

/// <summary>
/// quickslot
/// </summary>
public enum QuickSlot_Kind
{
    QuickSlot_Kind_HP,
    QuickSlot_kind_MP,
    QuickSlot_kind_END
}


/// <summary>
/// event
/// </summary>
public enum EventAccess
{
    EventAccess_Player,
    EventAccess_Target,
    EventAccess_Not,
    EventAccess_Null
}
public enum EventKind
{
    EventKind_BlueLight,
    EventKind_ChoiceTerrain
}

/// <summary>
/// ui
/// </summary>
public enum PlayerBarKind
{
    PlayerBarKind_HP,
    PlayerBarKind_MP,
    PlayerBarKind_EXP,
    PlayerBarKind_End
}
public enum TEXTKIND
{
    TEXTKIND_PlayerName,
    TEXTKIND_PlayerMoney,
    TEXTKIND_PlayerLevel,
    TEXTKIND_PlayerExp,
    TEXTKIND_PlayerHp,
    TEXTKIND_PlayerAddHp,
    TEXTKIND_PlayerMp,
    TEXTKIND_PlayerAddMp,
    TEXTKIND_PlayerAtk,
    TEXTKIND_PlayerAddAtk,
    TEXTKIND_PlayerDef,
    TEXTKIND_PlayerAddDef,
    TEXTKIND_End
}
public enum EquipmentSlotType
{
    EquipmentSlotType_Weapon,
    EquipmentSlotType_Armor,
    EquipmentSlotType_Shield,
    EquipmentSlotType_HpPotion,
    EquipmentSlotType_MpPotion,
    EquipmentSlotType_End
}
