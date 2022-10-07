using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using System.Reflection;
using System.Linq;
using System.Text;
using FGDefine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum ENUM_RPC_TARGET
{
    All,
    MASTER,
    OTHER
}

public class BroadcastMethodAttribute : PunRPC { }

public partial class PhotonLogicHandler
{
    public void TryBroadcastMethod<T, TParam1, TParam2, TParam3, TParam4, TParam5>(T owner, Action<TParam1, TParam2, TParam3, TParam4, TParam5> targetMethod, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType, param1, param2, param3, param4);
    }

    public void TryBroadcastMethod<T, TParam1, TParam2, TParam3, TParam4>(T owner, Action<TParam1, TParam2, TParam3, TParam4> targetMethod, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType, param1, param2, param3, param4);
    }

    public void TryBroadcastMethod<T, TParam1, TParam2, TParam3>(T owner, Action<TParam1, TParam2, TParam3> targetMethod, TParam1 param1, TParam2 param2, TParam3 param3, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType, param1, param2, param3);
    }

    public void TryBroadcastMethod<T, TParam1, TParam2>(T owner, Action<TParam1, TParam2> targetMethod, TParam1 param1, TParam2 param2, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType, param1, param2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">함수의 주인</typeparam>
    /// <typeparam name="TParam">byte, bool, short, int, long, float, double, string, byte[], T[], object[]</typeparam>
    public void TryBroadcastMethod<T, TParam>(T owner, Action<TParam> targetMethod, TParam param, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType, param);
    }

    /// <summary>
    /// 1. 넘기는 Action Method에 람다식은 허용되지 않습니다.
    /// 2. Method의 속성에 [BroadcastMethodAttribute]가 추가되어야 합니다.
    /// </summary>
    // [BroadcastMethodAttribute] 다음과 같이 함수 위에 추가
    public void TryBroadcastMethod<T>(T owner, Action targetMethod, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsConnected)
            return;

        BroadcastMethod(owner, targetMethod.Method, targetType);
    }

    private bool IsValidBroadcastMethod<T>(T owner, MethodInfo methodInfo)
        where T : MonoBehaviourPun
    {
        string methodName = methodInfo.Name;
        var ownerType = typeof(T);

        if (owner == null || owner.photonView == null)
        {
            Debug.LogError("동기화될 객체가 없거나 네트워킹 가능 상태가 아닙니다. 객체의 상태를 확인해주세요.");
            return false;
        }
        else if (ownerType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static) == null)
        {
            Debug.LogError("넘기는 Action Method에 람다식은 허용되지 않습니다. 객체 내부에 Method를 구현 후 인자로 넘겨주세요.");
            return false;
        }
        else if (!methodInfo.IsDefined(typeof(BroadcastMethodAttribute)))
        {
            Debug.LogError("Broadcast할 메소드에 [BroadcastMethodAttribute] 속성이 없습니다. 추가해주세요.");
            return false;
        }

        return true;
    }

    private RpcTarget ConvertRPCTargetType(ENUM_RPC_TARGET targetType)
    {
        RpcTarget RPCTargetType = RpcTarget.All;

        switch (targetType)
        {
            case ENUM_RPC_TARGET.All:
                RPCTargetType = RpcTarget.AllBuffered;
                break;

            case ENUM_RPC_TARGET.MASTER:
                RPCTargetType = RpcTarget.MasterClient;
                break;

            case ENUM_RPC_TARGET.OTHER:
                RPCTargetType = RpcTarget.OthersBuffered;
                break;

            default:
                break;
        }

        return RPCTargetType;
    }

    private void BroadcastMethod<T>(T owner, MethodInfo methodInfo, ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
        where T : MonoBehaviourPun
    {
        if (!IsValidBroadcastMethod(owner, methodInfo))
            return;

        var RPCTargetType = ConvertRPCTargetType(targetType);
        owner.photonView.RPC(methodInfo.Name, RPCTargetType);
    }

    private void BroadcastMethod<T>(T owner, MethodInfo methodInfo, ENUM_RPC_TARGET targetType, params object[] parameters)
        where T : MonoBehaviourPun
    {
        if (!IsValidBroadcastMethod(owner, methodInfo))
            return;

        var RPCTargetType = ConvertRPCTargetType(targetType);
        owner.photonView.RPC(methodInfo.Name, RPCTargetType, parameters);
    }

}
