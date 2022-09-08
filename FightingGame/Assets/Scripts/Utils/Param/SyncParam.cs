using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;
using ExitGames.Client.Photon;

[Serializable]
public class SyncAttackObjectParam : PhotonCustomType<SyncAttackObjectParam>
{
    public ENUM_TEAM_TYPE teamType;
    public bool reverseState;
    public Transform targetTr; // 트랜스폼은 참조 형식이라 직렬화가 불가능함. Vector3로 변경이 필요함

    public SyncAttackObjectParam()
	{

	}

    public SyncAttackObjectParam(ENUM_TEAM_TYPE _teamType, bool _reverseState, Transform _targetTr)
    {
        teamType = _teamType;
        reverseState = _reverseState;
        targetTr = _targetTr;
    }
}
