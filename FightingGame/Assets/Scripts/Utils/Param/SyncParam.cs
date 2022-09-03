using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class SyncParam { }

[Serializable]
public struct SyncAttackObjectParam
{
    public ENUM_TEAM_TYPE teamType;
    public bool reverseState;

    public SyncAttackObjectParam(ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        teamType = _teamType;
        reverseState = _reverseState;
    }
}
