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
    }

    /// <summary>
    /// 해당 캐릭터 타입
    /// </summary>
    public enum ENUM_CHARACTER_TYPE
    {
        Knight = 0,

        Max
    }

    public enum ENUM_INPUT_TYPE
    {
        Null = 0,
        Joystick = 1,
        Keyboard = 2,

        Max
    }

    public enum ENUM_PLAYER_STATE
    {
        Idle,
        Move,
        Jump,
        Attack,
        Interect,
        Die,

        Max
    }
}

