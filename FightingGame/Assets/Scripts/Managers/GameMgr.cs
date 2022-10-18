using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class GameMgr
{
    ENUM_CHARACTER_TYPE blueTeamCharType = ENUM_CHARACTER_TYPE.Default;
    ENUM_CHARACTER_TYPE redTeamCharType = ENUM_CHARACTER_TYPE.Default;

    /// <summary>
    /// 마스터면 blue, 슬레이브면 red
    /// </summary>
    public void Set_CharacterType(ENUM_CHARACTER_TYPE charType, ENUM_TEAM_TYPE teamType)
    {
        switch (teamType)
        {
            case ENUM_TEAM_TYPE.Red:
                redTeamCharType = charType;
                break;
            case ENUM_TEAM_TYPE.Blue:
                blueTeamCharType = charType;
                break;
        }
    }

    public ENUM_CHARACTER_TYPE Get_CharacterType(ENUM_TEAM_TYPE teamType)
    {
        if (teamType == ENUM_TEAM_TYPE.Red)
            return redTeamCharType;
        else
            return blueTeamCharType;
    }

    public void Init()
    {
        
    }

    public void Clear()
    {

    }
}
