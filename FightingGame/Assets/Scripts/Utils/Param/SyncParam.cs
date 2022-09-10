using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;
using ExitGames.Client.Photon;
using System.Text;

[Serializable]
public class SyncAttackObjectParam : CharacterParam
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

    public new static object Deserialize(byte[] data)
    {
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<SyncAttackObjectParam>(jsonData);
    }

    public new static byte[] Serialize(object customObject)
    {
        var param = (SyncAttackObjectParam)customObject;
        string jsonData = JsonUtility.ToJson(param);
        return Encoding.UTF8.GetBytes(jsonData);
    }
}
