using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class GameMgr
{
    ENUM_CHARACTER_TYPE charType = ENUM_CHARACTER_TYPE.Default;

    /// <summary>
    /// 마스터면 blue, 슬레이브면 red
    /// </summary>
    public void Set_CharacterType(ENUM_CHARACTER_TYPE _charType)
    {
        charType = _charType;
    }

    public ENUM_CHARACTER_TYPE Get_CharacterType()
    {
        return charType;
    }

    public void Init()
    {
        
    }

    public void Clear()
    {

    }
}
