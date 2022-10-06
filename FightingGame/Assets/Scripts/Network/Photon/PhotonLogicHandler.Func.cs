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

public interface ILobbyPostProcess
{
    void OnUpdateLobby(List<CustomRoomInfo> roomList);
}

public interface IRoomPostProcess
{
    void OnUpdateRoomProperty(CustomRoomProperty property);
    void OnUpdateRoomPlayerProperty(CustomPlayerProperty property);
}


public partial class PhotonLogicHandler : ILobbyCallbacks
{
    private List<CustomRoomInfo> customRoomList = new List<CustomRoomInfo>();

    private List<ILobbyPostProcess> lobbyPostProcesses = new List<ILobbyPostProcess>();
    private List<IRoomPostProcess> roomPostProcesses = new List<IRoomPostProcess>();

    public void ChangeMap(ENUM_MAP_TYPE mapType)
    {
        SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE, mapType);
    }

    private void OnChangeRoomMasterClient(string nickname)
    {
        SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME, nickname);
        onChangeMasterClientNickname?.Invoke(nickname);
    }


    public void ChangeCharacter(ENUM_CHARACTER_TYPE characterType)
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER, characterType);
    }

    public bool IsAllReady()
    {
        var players = PhotonNetwork.PlayerList;
        string readyStr = ENUM_PLAYER_STATE_PROPERTIES.READY.ToString();
        return players.All(p => p.CustomProperties.ContainsKey(readyStr) && (bool)p.CustomProperties[readyStr]);
    }

    public void Ready()
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, true);
    }

    public void UnReady()
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, false);
    }

    private void SetUserInfo(string userKey, ENUM_LOGIN_TYPE loginType)
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.USERKEY, userKey);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE, loginType);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, false);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER, ENUM_CHARACTER_TYPE.Default);
    }

    public void RegisterILobbyPostProcess(ILobbyPostProcess lobbyPostProcess)
    {
        if (!lobbyPostProcesses.Contains(lobbyPostProcess))
        {
            lobbyPostProcesses.Add(lobbyPostProcess);
        }
    }

    public void UnregisterILobbyPostProcess(ILobbyPostProcess lobbyPostProcess)
    {
        if (lobbyPostProcesses.Contains(lobbyPostProcess))
        {
            lobbyPostProcesses.Remove(lobbyPostProcess);
        }
    }

    public void RegisterIRoomPostProcess(IRoomPostProcess roomPostProcess)
    {
        if (!roomPostProcesses.Contains(roomPostProcess))
        {
            roomPostProcesses.Add(roomPostProcess);
        }
    }

    public void UnregisterIRoomPostProcess(IRoomPostProcess roomPostProcess)
    {
        if (roomPostProcesses.Contains(roomPostProcess))
        {
            roomPostProcesses.Remove(roomPostProcess);
        }
    }

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


    #region Register 계열 외부 함수, MonoBehaviourPhoton을 등록, 파기할 때 사용
    public static int Register(PhotonView view)
    {
        if (view == null)
            return 0;

        if (view.ViewID == 0)
            PhotonNetwork.AllocateViewID(view);

        if (view.ViewID == 0)
        {
            Debug.LogError("유효한 포톤 뷰 객체가 아님 ㅅㅂ 암튼 아님 문의줘보셈");
            return 0;
        }

        if (!photonViewDictionary.ContainsKey(view.ViewID))
        {
            photonViewDictionary.Add(view.ViewID, view);
        }
        else
        {
            Debug.LogWarning($"같은 MonoBehaviourPhoton 오브젝트를 추가하려 들었음. {view.ViewID}");
            return 0;
        }

        return view.ViewID;
    }

    public static int Unregister(int viewID)
    {
        PhotonView view = null;

        if (photonViewDictionary.TryGetValue(viewID, out view))
        {
            view.ViewID = 0;
            photonViewDictionary.Remove(viewID);
        }
        else
        {
            Debug.LogWarning($"등록되지 않은 MonoBehaviourPhoton 오브젝트를 제거하려 들었음. {viewID}");
        }

        return 0;
    }

    #endregion


}
