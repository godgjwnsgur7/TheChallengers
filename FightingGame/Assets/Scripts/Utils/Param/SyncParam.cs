using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;
using ExitGames.Client.Photon;

[Serializable]
public class SyncAttackObjectParam : PhotonCustomType
{
    public ENUM_TEAM_TYPE teamType;
    public bool reverseState;
    public Transform targetTr; // 트랜스폼은 참조 형식이라 직렬화가 불가능함. Vector3로 변경이 필요함

    public SyncAttackObjectParam()
	{

	}

    public static byte[] memoryBytes = null;
    public static short offset = 0;

    static SyncAttackObjectParam()
    {
        offset = 2 * sizeof(int);
        memoryBytes = new byte[offset];
    }

    public SyncAttackObjectParam(ENUM_TEAM_TYPE _teamType, bool _reverseState, Transform _targetTr)
    {
        teamType = _teamType;
        reverseState = _reverseState;
        targetTr = _targetTr;
    }
    
    public static object Deserialize(StreamBuffer inStream, short length)
	{
        var param = new SyncAttackObjectParam();
        short offset = 2 * sizeof(int);

        lock (memoryBytes)
        {
            inStream.Read(memoryBytes, 0, offset);
            int index = 0;

            int teamTypeInt = 0;
            Protocol.Deserialize(out teamTypeInt, memoryBytes, ref index);
            param.teamType = (ENUM_TEAM_TYPE)teamTypeInt;

            int reverseStateInt = 0;
            Protocol.Deserialize(out reverseStateInt, memoryBytes, ref index);
            param.reverseState = reverseStateInt == 1;
        }

        return param;
    }

	public static short Serialize(StreamBuffer outStream, object customObject)
	{
        var param = (SyncAttackObjectParam)customObject;
        short offset = 2 * sizeof(int);

        lock (memoryBytes)
        {
            byte[] bytes = memoryBytes;
            int index = 0;

            Protocol.Serialize((int)param.teamType, bytes, ref index);
            Protocol.Serialize((int)(param.reverseState ? 1 : 0), bytes, ref index);

            outStream.Write(bytes, 0, offset);
        }

        return offset;
    }
}
