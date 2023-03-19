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
        ForestMap = 0,
        VolcanicMap = 1,
        Max = 2,
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

        // Knight
        Knight_JumpEffect = 21,
        Knight_LandingEffect = 22,
        Knight_MoveEffect = 23,
        Knight_DashEffect = 24,

        // Wizard
        Wizard_ThunderCircleEffect = 41,
        Wizard_StarlightEffect = 42, // Particle
        Wizard_MagicalJinEffect = 43, // Particle
        Wizard_DashEffect = 44,
    }

    /// <summary>
    /// "Resources/Sounds/BGM/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_BGM_TYPE
    {
        Unknown = 0,
        Main = 1,
        Battle = 2,
        Test = 3,

    }

    /// <summary>
    /// "Resources/Sounds/SFX/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_SFX_TYPE
    {
        UI_Click = 0,

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
    /// 오브젝트 -스프라이트렌더러 우선순위 타입 (높을수록, 위로 올라감)
    /// </summary>
    [Serializable]
    public enum ENUM_OBJECTLAYERLEVEL_TYPE
    {
        Untagged = 0,
        Back_Effect = 1,
        Character = 2,
        Front_Effect = 3,
        Front_Attack = 5,
        Max = 5,
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

