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
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum ENUM_RPC_TARGET
{
    All,
    MASTER,
    OTHER
}

public class BroadcastMethodAttribute : PunRPC { }

public delegate void DisconnectCallBack(string cause);
public delegate void FailedCallBack(short returnCode, string message);

public delegate void PlayerCallBack(string nickname);

public partial class PhotonLogicHandler : MonoBehaviourPunCallbacks
{
    private static PhotonLogicHandler instance;
    public static PhotonLogicHandler Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject g = new GameObject("PhotonLogicHandler");
                instance = g.AddComponent<PhotonLogicHandler>();
                instance.Initialize();

                DontDestroyOnLoad(g);
            }

            return instance;
        }
    }

    private readonly string GameVersion = "1";
    private static Dictionary<int, PhotonView> photonViewDictionary = new Dictionary<int, PhotonView>();
    private List<CustomRoomInfo> customRoomList = new List<CustomRoomInfo>();

    private Action _OnCreateRoom = null;
    private FailedCallBack _OnCreateRoomFailed = null;

    private Action _OnConnectedToMaster = null;
    private DisconnectCallBack _OnDisconnectedFromMaster = null;

    private Action _OnJoinRoom = null;
    private FailedCallBack _OnJoinRoomFailed = null;

    private Action _OnJoinLobby = null;
    private FailedCallBack _OnJoinLobbyFailed = null;

    private Action _OnLeftRoom = null;
    private Action _OnLeftLobby = null;

    private List<ILobbyPostProcess> lobbyPostProcesses = new List<ILobbyPostProcess>();
    private List<IRoomPostProcess> roomPostProcesses = new List<IRoomPostProcess>();

    public event PlayerCallBack onChangeMasterClientNickname = null;
    public event PlayerCallBack onLeftRoomPlayer = null;
    public event PlayerCallBack onEnterRoomPlayer = null;

    private void OnDestroy()
    {
        _OnConnectedToMaster = null;
        _OnDisconnectedFromMaster = null;
        _OnJoinRoom = null;
        _OnJoinRoomFailed = null;
        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;
    }

    PhotonView view = null;

    private void Initialize()
    {
        PhotonCustomTypeManagement.Register();

        view = gameObject.AddComponent<PhotonView>();

        if (view.ViewID == 0)
            PhotonNetwork.AllocateViewID(view);
    }

    public void RegisterILobbyPostProcess(ILobbyPostProcess lobbyPostProcess)
	{
        if(!lobbyPostProcesses.Contains(lobbyPostProcess))
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


    public void TryBroadcastMethod<T, TParam1, TParam2, TParam3, TParam4, TParam5>(T owner, Action<TParam1, TParam2, TParam3, TParam4, TParam5> targetMethod, TParam1 param1, TParam2 param2, TParam3 param3, TParam4 param4, TParam5 param5,ENUM_RPC_TARGET targetType = ENUM_RPC_TARGET.All)
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



    #region 포톤 자체 콜백 - 가급적 건드리지 마시오.

    /// <summary>
    /// 마스터 서버 접속 시도 성공 시 콜백
    /// </summary>

    public override void OnConnectedToMaster() 
    {
        Debug.Log("마스터 서버에 성공적으로 접속했습니다.");
        _OnConnectedToMaster?.Invoke();

        _OnConnectedToMaster = null;
    }

    /// <summary>
    /// 접속 중 접속이 끊어졌을 때 불리는 콜백
    /// </summary>
    /// <param name="cause"></param>

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"마스터 서버로부터 접속이 끊어졌습니다. 사유 : {cause}");
        _OnDisconnectedFromMaster?.Invoke(cause.ToString());

        _OnDisconnectedFromMaster = null;
    }

    /// <summary>
    /// 방 만들기 성공 시 콜백
    /// </summary>

	public override void OnCreatedRoom()
	{
        _OnCreateRoom?.Invoke();
        _OnCreateRoom = null;
    }

    /// <summary>
    /// 방 만들기 실패 시 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
        _OnCreateRoomFailed?.Invoke(returnCode, message);
        _OnCreateRoomFailed = null;
    }

	/// <summary>
	/// 로비에 접속되었을 때 불리는 콜백
	/// </summary>

	public override void OnJoinedLobby()
	{
        Debug.Log("로비 접속 성공");
        _OnJoinLobby?.Invoke();

        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;
    }

    /// <summary>
    /// 로비에 접속이 실패했을 때 불리는 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
        Debug.LogError($"{returnCode} - {message} ");
        _OnJoinLobbyFailed?.Invoke(returnCode, message);

        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;
    }

	/// <summary>
	/// Room에 참가했을 때 불리는 콜백
    /// 이 곳에서 로그인이 완료가 되어 있어야 한다.
	/// </summary>
	/// 

	public override void OnJoinedRoom()
    {
        Debug.Log($"룸에 성공적으로 접속하였습니다.");
#if UNITY_EDITOR
        Info();
#endif
        PhotonNetwork.AutomaticallySyncScene = true;

        if(!CheckEnableJoinRoom())
		{
            TryLeaveRoom(() => 
            { 
                TryLeaveLobby(); 
            });
		}

        // 여기서 널이 뜨진 않겠지
        SetUserInfo(Managers.Platform.GetUserID(), Managers.Platform.CurrentLoginType);

        _OnJoinRoom?.Invoke();
        
        _OnJoinRoom = null;
        _OnJoinRoomFailed = null;
    }

    private bool CheckEnableJoinRoom()
	{
        var loginType = Managers.Platform.CurrentLoginType;
        if (loginType == ENUM_LOGIN_TYPE.None)
        {
            Debug.LogError("로그인 안됐음");
            return false;
        }

        var userID = Managers.Platform.GetUserID();
        if (userID == null || userID == string.Empty)
        {
            Debug.LogError("로그인이 유효하지 않음");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Room 참가에 실패했을 때 불리는 콜백
    /// </summary> 
    /// <param name="returnCode"></param> 
    /// <param name="message"></param>

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"룸 접속에 실패하였습니다. 코드 : {returnCode}, 사유 : {message}");
        _OnJoinRoomFailed?.Invoke(returnCode, message);
       
        _OnJoinRoom = null;
        _OnJoinRoomFailed = null;
    }

	public override void OnConnected()
	{
		base.OnConnected();
	}

	public override void OnLeftRoom()
	{
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.LogWarning($"유저가 방을 떠났습니다.");

        // 이 곳에서 Player 세팅 해제해야 합니다.

        _OnLeftRoom?.Invoke();
        _OnLeftRoom = null;
    }

	public override void OnLeftLobby()
	{
        Debug.LogWarning($"유저가 로비를 떠났습니다.");

        _OnLeftLobby?.Invoke();
        _OnLeftLobby = null;
    }

    /// <summary>
    /// 방 리스트 정보 업데이트
    /// </summary>
    /// <param name="roomList"></param>

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
        customRoomList.Clear();
        foreach (var room in roomList)
		{
            string masterClientNickname = string.Empty;
            ENUM_MAP_TYPE currentMapType = ENUM_MAP_TYPE.BasicMap;

            if (room.CustomProperties.TryGetValue(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), out var value))
			{
                masterClientNickname = (string)value;
            }
            if (room.CustomProperties.TryGetValue(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), out value))
            {
                currentMapType = (ENUM_MAP_TYPE)value;
            }

            CustomRoomInfo info = new CustomRoomInfo()
            {
                roomName = room.Name,
                masterClientId = room.masterClientId,

                customProperty = new CustomRoomProperty()
				{
                    masterClientNickname = masterClientNickname,
                    currentMapType = currentMapType,
                },

                currentPlayerCount = room.PlayerCount,
                maxPlayerCount = room.MaxPlayers
            };

            customRoomList.Add(info);
        }

        foreach(var process in lobbyPostProcesses)
		{
            process?.OnUpdateLobby(customRoomList);
		}
    }

	/// <summary>
	/// 방장 바뀜
	/// </summary>
	/// <param name="newMasterClient"></param>

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
        OnChangeRoomMasterClient(newMasterClient?.NickName);

        Debug.LogWarning($"{newMasterClient.NickName} 으로 방장이 바뀌었습니다.");
    }

    /// <summary>
    /// 룸 설정 변경
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        var property = MakeRoomProperty(propertiesThatChanged);

        foreach (var roomPostProcess in roomPostProcesses)
        {
            roomPostProcess?.OnUpdateRoomProperty(property);
        }

        Debug.LogWarning($"방 설정이 바뀌었습니다.");
    }

    private CustomRoomProperty MakeRoomProperty(Hashtable propertiesThatChanged)
	{
        CustomRoomProperty property = new CustomRoomProperty();

        if (propertiesThatChanged.TryGetValue(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), out var value))
        {
            property.masterClientNickname = (string)value;
        }
        if (propertiesThatChanged.TryGetValue(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), out value))
        {
            property.currentMapType = (ENUM_MAP_TYPE)value;
        }

        return property;
    }

    /// <summary>
    /// 룸 내 플레이어 정보 변경
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
        PublishPlayerProperty(targetPlayer);

        Debug.LogWarning($"플레이어 설정이 바뀌어 데이터를 재검색합니다.");
    }

    private void PublishPlayerProperty(Player targetPlayer)
	{
        if (roomPostProcesses.Count <= 0)
            return;

        var propertiesThatChanged = targetPlayer.CustomProperties;

        ENUM_LOGIN_TYPE loginType = ENUM_LOGIN_TYPE.None;
        string userKey = string.Empty;
        bool isReady = false;
        ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.Default;

        if (propertiesThatChanged.TryGetValue(ENUM_PLAYER_STATE_PROPERTIES.READY.ToString(), out var value))
        {
            isReady = (bool)value;
        }
        if (propertiesThatChanged.TryGetValue(ENUM_PLAYER_STATE_PROPERTIES.USERKEY.ToString(), out value))
        {
            userKey = (string)value;
        }
        if (propertiesThatChanged.TryGetValue(ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE.ToString(), out value))
        {
            loginType = (ENUM_LOGIN_TYPE)value;
        }
        if (propertiesThatChanged.TryGetValue(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString(), out value))
        {
            characterType = (ENUM_CHARACTER_TYPE)value;
        }
        
        // DB는 잠시 보류

        //Managers.Platform.DBSelect(loginType, userKey, OnSuccess: (userData) =>
        //{
        //    CustomPlayerProperty property = new CustomPlayerProperty()
        //    {
        //        isReady = isReady,
        //        data = userData,
        //        characterType = characterType
        //    };
    
        //    foreach (var roomPostProcess in roomPostProcesses)
        //    {
        //        roomPostProcess?.OnUpdateRoomPlayerProperty(property);
        //    }

        //}, OnFailed: () => 
        //{ 
        //    Debug.LogError($"{loginType} - {userKey} 검색 실패"); 
        //});
    }

    /// <summary>
    /// 유저 입장
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
		{
            PhotonNetwork.CurrentRoom.IsOpen = false;
		}

        Debug.LogWarning($"{newPlayer.NickName} 님이 입장하였습니다.");

        onEnterRoomPlayer?.Invoke(newPlayer.NickName);
    }

    /// <summary>
    /// 유저 나감
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

        Debug.LogWarning($"{otherPlayer.NickName} 님이 퇴장하였습니다.");

        onLeftRoomPlayer?.Invoke(otherPlayer.NickName);
    }

    public string Info()
    {
        StringBuilder str = new StringBuilder();

        if (PhotonNetwork.InRoom)
        {
            str.Append($"현재 방 이름 : {PhotonNetwork.CurrentRoom.Name} \n");
            str.Append($"현재 방 인원 수 : {PhotonNetwork.CurrentRoom.PlayerCount} \n");
            str.Append($"현재 방 최대 인원 수 : {PhotonNetwork.CurrentRoom.MaxPlayers} \n");
            str.Append($"{PhotonNetwork.CurrentRoom.MasterClientId}, {PhotonNetwork.LocalPlayer.ActorNumber} 두 개가 같으면 나는 마스터");
        }
        else
        {
            str.Append($"접속한 인원 수 : {PhotonNetwork.CountOfPlayers} \n");
            str.Append($"방 개수 : {PhotonNetwork.CountOfRooms} \n");
            str.Append($"모든 방에 있는 인원 수 : {PhotonNetwork.CountOfPlayersInRooms} \n");
            str.Append($"로비에 있는가? : { PhotonNetwork.InLobby} \n");
            str.Append($"연결이 됐는가? : { PhotonNetwork.IsConnected} \n");
        }

        print(str);
        return str.ToString();
    }

    #endregion
}
