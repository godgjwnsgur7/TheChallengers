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
        /// 여기는 Built-In 레이어
        /// </summary>
        Default = 0,
        TransparentFX = 1,
        IgnoreRaycast = 2, // 레이캐스트 무시
        Water = 4, // 물 등 특이 장애물
        UI = 5,

        /// <summary>
        /// 여기부터 User 커스텀 레이어
        /// </summary>
        Player = 6, // 유저
        Interaction = 7,
        Wall = 8,
        Ground = 9, // 없애기 직전 (임시)
    }

    /// <summary>
    /// 일단 User 커스텀 태그만 둠
    /// </summary>
    [Serializable]
    public enum ENUM_TAG_TYPE
    {
        Ground = 0,
        Enemy = 1, // 사라질 예정?
        Ally = 2, // 사라질 예정?
    }

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
        Max
    }

    /// <summary>
    /// "Resources/Prefabs/Maps/" 경로 안에 같은 이름의 프리팹이 필요
    /// </summary>
    [Serializable]
    public enum ENUM_MAP_TYPE
    {
        BasicMap = 0,
    }

    /// <summary>
    /// "Resources/Prefabs/AttackObjects/" 경로 안의 프리팹 이름 리스트
    /// </summary>
    [Serializable]
    public enum ENUM_ATTACKOBJECT_NAME
    {
        Knight_JumpAttack = 0,
        Knight_Attack1 = 1,
        Knight_Attack2 = 2,
        Knight_Attack3 = 3,
        Knight_ThrowSkill = 4,
        Knight_SuperArmourSkill = 5,
        Knight_Skill3 = 6,
        Knight_SuperArmourSkill_1 = 7,
        Knight_SuperArmourSkill_2 = 8,
        Knight_SuperArmourSkill_3 = 9,

    }

    /// <summary>
    /// "Resources/Sounds/BGM/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_BGM_TYPE
    {
        TestBGM, // 테스트용
    }

    /// <summary>
    /// "Resources/Sounds/SFX/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    [Serializable]
    public enum ENUM_SFX_TYPE
    {
        win, // 테스트용
    }

    [Serializable]
    public enum ENUM_INPUT_TYPE
    {
        Null = 0,
        Joystick = 1,

        Max
    }

    [Serializable]
    public enum ENUM_PLAYER_STATE
    {
        Idle,
        Move,
        Jump,
        Attack,
        Skill,
        Down,
        Hit,
        Die,

        Max
    }

    [Serializable]
    public enum ENUM_SCENE_TYPE
    {
        Unknown,
        Login,
        Lobby,
        Battle,
        Loading,
        Main,
        Training,
        Debug, // 테스트씬
    }

    [Serializable]
    public enum ENUM_BTNPREFS_TYPE
    {
        LeftMoveBtn = 0,
        RightMoveBtn = 1,
        AttackBtn = 2,
        JumpBtn = 3,
        SkillBtn1 = 4,
        SkillBtn2 = 5,
        SkillBtn3 = 6,

        Max
    }
}

