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
    /// 해당 캐릭터 타입
    /// </summary>
    public enum ENUM_CHARACTER_TYPE
    {
        Knight = 0,
        Astronaut = 1,

        Max
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
        
        // Near
        Hammer = 1, 
        Sword = 2, 
        Sycthe = 3,
        
        Max
    }
}

