using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FGDefine
{
    [Serializable]
    public enum ENUM_LAYER_TYPE
    {
        /// <summary>
        /// Built-In 레이어
        /// </summary>
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2,
        Water = 4,
        UI = 5,

        /// <summary>
        /// User 커스텀 레이어
        /// </summary>
        Player = 6, // 유저
        Ground = 7,
    }

    /// <summary>
    /// User 커스텀 태그
    /// </summary>
    [Serializable]
    public enum ENUM_TAG_TYPE
    {
        Ground = 0,
    }

    /// <summary>
    /// Master : Blue (위, 왼쪽)
    /// </summary>
    [Serializable]
    public enum ENUM_TEAM_TYPE
    {
        Defalut = 0,
        Blue = 1,
        Red = 2,

        Max
    }

    /// <summary>
    /// 캐릭터 타입
    /// </summary>
    [Serializable]
    public enum ENUM_CHARACTER_TYPE
    {
        Default = 0,
        Knight = 1,
        Wizard = 2,
        Max = 3,
    }

    /// <summary>
    /// Prefabs/UI의 InputPanel, AreaPanel가 가진 조작키의 오브젝트 이름들과 같아야 함
    /// </summary>
    [Serializable]
    public enum ENUM_INPUTKEY_NAME
    {
        Direction = 0,
        Jump = 1,
        Dash = 2,
        Attack = 3,
        Skill1 = 4,
        Skill2 = 5,
        Skill3 = 6,
        Skill4 = 7,
        Max = 8
    }

    /// <summary>
    /// "Resources/Prefabs/Maps/" 경로 안에 같은 이름의 프리팹이 필요
    /// </summary>
    [Serializable]
    public enum ENUM_MAP_TYPE
    {
        CaveMap = 0,
        TempleMap = 1,
        VolcanicMap = 2,
        Max = 3,
    }

    /// <summary>
    /// "Resources/Prefabs/AttackObjects/" 경로 안의 프리팹 이름 리스트
    /// GenerateObject는 공격에 대한 정보가 없음
    /// </summary>
    [Serializable]
    public enum ENUM_ATTACKOBJECT_NAME
    {
        // Knight
        Knight_Attack1 = 1,
        Knight_Attack2 = 2,
        Knight_Attack3 = 3,
        Knight_DashSkill_1 = 4,
        Knight_DashSkill_2 = 5,
        Knight_DashSkill_3 = 6,
        Knight_JumpAttack = 7,
        Knight_ThrowSkillObject = 8,
        Knight_SmashSkillObject = 9, // GenerateObject
        Knight_SmashSkillObject_1 = 10,
        Knight_SmashSkillObject_2 = 11,
        Knight_SmashSkillObject_3 = 12,
        Knight_ComboSkill_1 = 13,
        Knight_ComboSkill_2 = 14,
        Knight_ComboSkill_3 = 15,
        Knight_ComboSkill_4 = 16,
        Knight_ComboSkill_5 = 17,
        Knight_DashSkillObject = 18, // GenerateObject
        Knight_ComboSkillObject = 19, // GenerateObject

        // Wizard
        Wizard_Attack1 = 21,
        Wizard_Attack2 = 22,
        Wizard_ThrowAttackObject = 23,
        Wizard_ThrowJumpAttackObject = 24,
        Wizard_ThunderSkillObject = 25, // GenerateObject
        Wizard_ThunderSkillObject_1 = 26,
        Wizard_ThunderSkillObject_2 = 27,
        Wizard_ThunderSkillObject_3 = 28,
        Wizard_IceSkillObject = 29, // GenerateObject
        Wizard_IceSkillObject_1 = 30,
        Wizard_MeteorSkillObject = 31, // GenerateObject
        Wizard_MeteorSkillObject_Falling = 32,
        Wizard_MeteorSkillObject_Explode = 33,
        Wizard_PushOutSkillObject = 34, // GenerateObject
        Wizard_PushOutSkillObject_1 = 35,
    }

    /// <summary>
    /// "Resources/Prefabs/EffectObjects/" 경로 안의 프리팹 이름 리스트
    /// </summary>
    [Serializable]
    public enum ENUM_EFFECTOBJECT_NAME
    {
        // Public
        Basic_AttackedEffect1 = 0,
        Basic_AttackedEffect2 = 1,
        Basic_AttackedEffect3 = 2,
        Basic_SkillAttackedEffect1 = 3,
        Basic_SkillAttackedEffect2 = 4,

        // Knight
        Knight_SmokeEffect_JumpUp = 21,
        Knight_SmokeEffect_Landing = 22,
        Knight_SmokeEffect_Move = 23,
        Knight_SmokeEffect_Dash = 24,
        Knight_SmokeEffect_DashSkill = 25,
        Knight_SmokeEffect_ComboSkill1 = 26,
        Knight_SmokeEffect_ComboSkill2 = 27,
        Knight_SmokeEffect_ComboSkill3 = 28,
        Knight_SmokeEffect_ComboSkill4 = 29,
        Knight_SmokeEffect_ComboSkill5 = 30,
        Knight_SmokeEffect_ComboSkill6 = 31,

        // Wizard
        Wizard_ThunderCircleEffect = 41,
        Wizard_StarlightEffect = 42, // Particle
        Wizard_MagicalJinEffect = 43, // Particle
        Wizard_DashEffect = 44,
        Wizard_FlameEffect = 45,
    }

    /// <summary>
    /// "Resources/Sounds/BGM/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_BGM_TYPE
    {
        Unknown = 0,
        Test = 1,
        Main = 2,
        CaveMap = 3,
        TempleMap = 4,
        VolcanicMap = 5,

        MAX,
    }

    /// <summary>
    /// "Resources/Sounds/SFX/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_SFX_TYPE
    {
        UI_MacthingCompleted = 0,
        UI_MacthingStart = 1,
        UI_Result_Defeat = 2,
        UI_Result_Victory = 3,
        UI_Result_Draw = 4, // 아직 없음
        UI_GameStartInfo = 5,

        // Knight Sound
        Knight_Attack1 = 101,
        Knight_Attack2 = 102,
        Knight_Attack3 = 103,
        Knight_JumpAttack = 104,
        Knight_JumpUp = 105,
        Knight_Landing = 106,
        Knight_Attack3_Voice1 = 107,
        Knight_Attack3_Voice2 = 108,
        
        Knight_ThrowSkill = 109,
        Knight_ThrowSkill_Voice = 110,
        Knight_ThrowSkillObject = 111,
        
        Knight_SmashSkill = 112,
        Knight_SmashSkillObject = 113,
        
        Knight_DashSkill = 114,
        Knight_DashSkill_Voice = 115,
        Knight_DashSkillObject = 116,
        
        Knight_ComboSkill_1 = 117,
        Knight_ComboSkill_2 = 118,
        Knight_ComboSkill_3 = 119,
        Knight_ComboSkill_4 = 120,
        Knight_ComboSkill_5 = 121,
        
        Knight_Dash = 122,

        // Wizard Sound
        Wizard_Attack1 = 201,
        Wizard_Attack2 = 202,
        Wizard_Attack3 = 203, // 미사용
        Wizard_JumpAttack = 204,
        Wizard_Landing = 205,
        Wizard_JumpUp = 206,

        Wizard_Dash = 207,
        Wizard_Attack3_Object = 208,
        Wizard_Attack3_Voice = 209,
        Wizard_JumpAttackObject = 210,

        Wizard_IceSkillObject = 211,
        Wizard_IceSkillObject_Broken = 212,
        Wizard_IceSkillObject_Voice = 213,
        
        Wizard_ThunderSkill_Voice = 214,
        Wizard_ThunderSkillObject = 215,
        Wizard_ThunderSkillObject_1 = 216,
        Wizard_ThunderSkillObject_2 = 217,
        Wizard_ThunderSkillObject_3 = 218,
        
        Wizard_MeteorSkill_Voice = 219,
        Wizard_MeteorSkillObject_Falling = 220,
        Wizard_MeteorSkillObject_Explode = 221,
        
        Wizard_PushOutSkillObject = 222,
        Wizard_PushOutSkill_Voice = 223,

        // Hit Sound
        Dummy_HitSound = 300,
        Hit_Knight_Attack1 = 301,
        Hit_Knight_Attack2 = 302,
        Hit_Knight_Attack3 = 303,
        Hit_Knight_JumpAttack = 304,
        
        Hit_Knight_ComboSkill1 = 305,
        Hit_Knight_ComboSkill2 = 306,
        Hit_Knight_ComboSkill3 = 307,
        Hit_Knight_ComboSkill4 = 308,
        Hit_Knight_ComboSkill5 = 309,
        
        Hit_Knight_DashSkill1 = 310,
        Hit_Knight_DashSkill2 = 311,
        Hit_Knight_DashSkill3 = 312,

        Hit_Knight_SmashSkill1 = 313,
        Hit_Knight_SmashSkill2 = 314,
        Hit_Knight_SmashSkill3 = 315,

        Hit_Knight_ThrowSkill = 316,
    }

    [Serializable]
    public enum ENUM_PLAYER_STATE
    {
        Idle,
        Move,
        Jump,
        Dash,
        Attack,
        Skill,
        Down,
        Hit,
        Die,

        Max
    }

    /// <summary>
    /// 씬 이름과 같아야 함
    /// Unknown 제외
    /// </summary>
    [Serializable]
    public enum ENUM_SCENE_TYPE
    {
        Unknown,
        Lobby,
        Battle,
        Main,
        Training,
        Debug, // 테스트씬
    }
}

