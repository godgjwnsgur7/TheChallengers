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
        Item = 7,
        Wall = 8, // 레이어로해? 태그로해? 고민중 (임시)
    }


    /// <summary>
    /// 일단 User 커스텀 태그만 둠
    /// </summary>
    [Serializable]
    public enum ENUM_TAG_TYPE
    {
        Ally = 0,
        Enemy = 1,
    }

    /// <summary>
    /// 해당 캐릭터 타입
    /// </summary>
    public enum ENUM_CHARACTER_TYPE
    {
        Default = 0,
        Knight = 1,
        Astronaut = 2,

        Max
    }

    public enum ENUM_SOUND_TYPE
    {
        BGM,
        SFX,

        Max
    }

    /// <summary>
    /// "Resources/Sounds/BGM/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    public enum ENUM_BGM_TYPE
    {
        TestBGM, // 테스트용
    }

    /// <summary>
    /// "Resources/Sounds/SFX/"
    /// 경로 안에 같은 이름의 Audio Clip 파일이 있어야 함
    /// </summary>
    public enum ENUM_SFX_TYPE
    {
        win, // 테스트용
    }

    public enum ENUM_INPUT_TYPE
    {
        Null = 0,
        Joystick = 1,

        Max
    }

    public enum ENUM_PLAYER_STATE
    {
        Idle,
        Move,
        Attack,
        Expression,
        Hit,
        Die,

        Max
    }

    public enum ENUM_WEAPON_TYPE
    {
        Null = 0,
        
        Hammer = 1, 
        Sword = 2, 
        Sycthe = 3,
        Bow = 4,
        Gun = 5,
        Rifle = 6,
        
        Max
    }

    public enum ENUM_ITEM_TYPE
    {
        Boom = 0,
        Hammer = 1,
        Sword = 2,
        Sycthe = 3,
        Bow = 4,
        Gun = 5,
        Rifle = 6,

        Max
    }
}

