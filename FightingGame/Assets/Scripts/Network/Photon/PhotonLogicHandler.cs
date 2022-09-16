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

public enum ENUM_CUSTOM_PROPERTIES
{
    MAP_TYPE = 0,
    MASTER_CLIENT_NICKNAME = 1,
}

public class BroadcastMethodAttribute : PunRPC { }

public delegate void DisconnectCallBack(string cause);
public delegate void FailedCallBack(short returnCode, string message);

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

    public event Func<ENUM_MAP_TYPE, bool> onChangedMap = null;
    public event Func<string, bool> onChangeMasterClientNickname = null;

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

        this.onChangedMap = OnChangedMap;
        this.onChangeMasterClientNickname = OnChangedMasterClient;

        view = gameObject.AddComponent<PhotonView>();

        if (view.ViewID == 0)
            PhotonNetwork.AllocateViewID(view);
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

    #region Try 계열 외부 함수

    /// <summary>
    /// 마스터 서버에 접속을 시도합니다. 해당 함수가 성공된 상태여야 다른 네트워크 함수를 사용할 수 있습니다.
    /// </summary>
    /// <param name="_OnConnectedToMaster"> 마스터 서버 접속 성공 시 불리는 콜백 </param>
    /// <param name="_OnDisconnectedFromMaster"> 마스터 서버 접속이 실패했거나, 접속이 끊어졌을 때 불리는 콜백 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryConnectToMaster(Action _OnConnectedToMaster = null, DisconnectCallBack _OnDisconnectedFromMaster = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError("이미 마스터 서버에 연결되어 있는 상태에서 연결을 시도했습니다.");
            return false;
        }
            
        this._OnConnectedToMaster = _OnConnectedToMaster;
        this._OnDisconnectedFromMaster = _OnDisconnectedFromMaster;

        PhotonNetwork.GameVersion = GameVersion;
        return PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속 시도
    }

    /// <summary>
    /// 랜덤 룸에 접속을 시도합니다.
    /// </summary>
    /// <param name="_OnJoinRoom"> 룸 접속에 성공했을 때 불리는 콜백 </param>
    /// <param name="_OnJoinRoomFailed">룸 접속에 실패했을 때 불리는 콜백 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryJoinRandomRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed, string nickname)
    {
        Debug.Log($"랜덤 룸에 접속을 시도합니다.");

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        PhotonNetwork.LocalPlayer.NickName = nickname;

        ExitGames.Client.Photon.Hashtable optionTable = new ExitGames.Client.Photon.Hashtable()
        {
           
        };

        return PhotonNetwork.JoinRandomRoom(optionTable, 0);
    }

    public bool TryJoinLobby(Action onSuccess = null, FailedCallBack onfailed = null)
    {
        this._OnJoinLobby = onSuccess;
        this._OnJoinLobbyFailed = onfailed;

        return PhotonNetwork.JoinLobby();
    }


    public bool IsConnectedAndReady() => PhotonNetwork.IsConnectedAndReady;
    public bool IsMasterServer() => PhotonNetwork.Server == ServerConnection.MasterServer;
    public bool IsInLobby() => PhotonNetwork.InLobby;
    public bool IsInRoom() => PhotonNetwork.InRoom;

    /// <summary>
    /// 모든 클라이언트가 준비가 되었음을 체크합니다.
    /// </summary>
    /// <returns></returns>
    public bool IsAllReady() 
    {
        var players = PhotonNetwork.PlayerList;
        return players.All(p => p.CustomProperties.ContainsKey("Ready") && (bool)p.CustomProperties["Ready"]);
    }

    /// <summary>
    /// 방장에게 준비가 되었음을 알립니다.
    /// </summary>
    public void Ready() 
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
    public void UnReady()
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        hash["Ready"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }


    /// <summary>
    /// 방장이 해당 함수를 호출하게 되면, 방의 멤버가 모두 씬이 동기화된 채로 이동하게 됨
    /// </summary>
    /// <param name="sceneName"></param>

    public bool TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE sceneType, bool isDataSyncScene = false)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("마스터 클라이언트가 아닌 경우 부를 수 없는 함수입니다.");
            return false;
        }

        PhotonNetwork.LoadLevel(sceneType.ToString());
        TrySyncData(isDataSyncScene);

        return true;
    }

    private void TrySyncData(bool isDataSyncScene)
	{
        if (isDataSyncScene)
        {
            NetworkDataHandler.Instance.StartSync();
        }
        else
        {
            if (NetworkDataHandler.IsAliveInstance)
            {
                NetworkDataHandler.Instance.EndSync();
            }
        }
    }

    /// <summary>
    /// 방을 새로 만듦
    /// </summary>
    /// <param name="roomName"> 방의 이름 </param>
    /// <param name="maxPlayerCount"> 방 인원 최대 수 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryCreateRoom(Action OnCreateRoom = null, FailedCallBack OnCreateRoomFailed = null, string roomName = "이름 없음", string masterClientNickname = "", int maxPlayerCount = 2, ENUM_MAP_TYPE defaultMapType = ENUM_MAP_TYPE.BasicMap)
    {
        this._OnCreateRoom = OnCreateRoom;
        this._OnCreateRoomFailed = OnCreateRoomFailed;

        PhotonNetwork.LocalPlayer.NickName = masterClientNickname;

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = (byte)maxPlayerCount };

        roomOptions.CustomRoomProperties = new Hashtable();
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_PROPERTIES.MAP_TYPE.ToString(), defaultMapType);
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), masterClientNickname);

        return PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    private bool OnChangedMap(ENUM_MAP_TYPE mapType)
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENUM_CUSTOM_PROPERTIES.MAP_TYPE.ToString()))
            return false;

        PhotonNetwork.CurrentRoom.CustomProperties[ENUM_CUSTOM_PROPERTIES.MAP_TYPE.ToString()] = mapType;
        return true;
    }

    public bool OnChangedMasterClient(string nickname)
	{
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENUM_CUSTOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString()))
            return false;

        PhotonNetwork.CurrentRoom.CustomProperties[ENUM_CUSTOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString()] = nickname;
        return true;
    }

    /// <summary>
    /// 프리팹을 룸 내 모든 인원에 대하여 생성
    /// </summary>
    /// <param name="prefabPath">리소스 폴더를 기준으로 한 프리팹 경로</param>
    /// <param name="pos"></param>
    /// <param name="quaternion"></param>

    public GameObject TryInstantiate(string prefabPath, Vector3 pos = default, Quaternion quaternion = default)
    {
        return PhotonNetwork.Instantiate(prefabPath, pos, quaternion);
    }

    /// <summary>
    /// 룸 내 모든 인원에 대하여 파괴, TryInstantiate로 생성된 애들에 한하여 파괴됩니다.
    /// </summary>
    /// <param name="obj"></param>

    public void TryDestroy(MonoBehaviourPhoton obj)
	{
        if (obj == null)
            return;

        PhotonNetwork.Destroy(obj.gameObject);        
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
    }

    /// <summary>
    /// 접속 중 접속이 끊어졌을 때 불리는 콜백
    /// </summary>
    /// <param name="cause"></param>

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"마스터 서버로부터 접속이 끊어졌습니다. 사유 : {cause}");
        _OnDisconnectedFromMaster?.Invoke(cause.ToString()); 
    }

    /// <summary>
    /// 방 만들기 성공 시 콜백
    /// </summary>

	public override void OnCreatedRoom()
	{
        _OnCreateRoom?.Invoke();
    }

    /// <summary>
    /// 방 만들기 실패 시 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
        _OnCreateRoomFailed?.Invoke(returnCode, message);
	}

	/// <summary>
	/// 로비에 접속되었을 때 불리는 콜백
	/// </summary>

	public override void OnJoinedLobby()
	{
        Debug.Log("로비 접속 성공");
        _OnJoinLobby?.Invoke();
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
    }

	/// <summary>
	/// Room에 참가했을 때 불리는 콜백
	/// </summary>
	/// 

	public override void OnJoinedRoom()
    {
        Debug.Log($"룸에 성공적으로 접속하였습니다.");
#if UNITY_EDITOR
        Info();
#endif
        PhotonNetwork.AutomaticallySyncScene = true;
        _OnJoinRoom?.Invoke();
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
    }

	public override void OnConnected()
	{
		base.OnConnected();
	}

	public override void OnLeftRoom()
	{
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.LogWarning($"유저가 방을 떠났습니다.");
    }

	public override void OnLeftLobby()
	{
        Debug.LogWarning($"유저가 로비를 떠났습니다.");
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
            CustomRoomInfo info = new CustomRoomInfo()
            {
                roomName = room.Name,
                roomId = room.masterClientId,
                masterClientNickname = (string)room.CustomProperties[ENUM_CUSTOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString()],
                currentMapType = (ENUM_MAP_TYPE)room.CustomProperties[ENUM_CUSTOM_PROPERTIES.MAP_TYPE.ToString()],
                currentPlayerCount = room.PlayerCount,
                maxPlayerCount = room.MaxPlayers
            };

            customRoomList.Add(info);
        }
    }

	/// <summary>
	/// 방장 바뀜
	/// </summary>
	/// <param name="newMasterClient"></param>

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
        Debug.LogWarning($"{newMasterClient.NickName} 으로 방장이 바뀌었습니다.");
	}

    /// <summary>
    /// 방 설정 변경
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
        Debug.LogWarning($"방 설정이 바뀌었습니다.");
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
