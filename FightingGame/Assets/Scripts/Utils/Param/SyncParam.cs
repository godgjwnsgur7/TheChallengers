using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

[Serializable]
public class SyncAttackObjectParam : PhotonCustomType<SyncAttackObjectParam>
{
    public ENUM_TEAM_TYPE teamType;
    public bool reverseState;
    public Transform targetTr;

    public SyncAttackObjectParam(ENUM_TEAM_TYPE _teamType, bool _reverseState, Transform _targetTr)
    {
        teamType = _teamType;
        reverseState = _reverseState;
        targetTr = _targetTr;
    }
}
