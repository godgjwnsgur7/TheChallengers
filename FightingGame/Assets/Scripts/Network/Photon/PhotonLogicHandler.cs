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
    TypedLobby GameLobby = new TypedLobby("1", LobbyType.SqlLobby);
    private readonly string ROOM_PROP_KEY = "C0";

    private static Dictionary<int, PhotonView> photonViewDictionary = new Dictionary<int, PhotonView>();

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

    public event PlayerCallBack onChangeMasterClientNickname = null;
    public event PlayerCallBack onLeftRoomPlayer = null;
    public event PlayerCallBack onEnterRoomPlayer = null;

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

    PhotonView view = null;

    private void Initialize()
    {
        PhotonCustomTypeManagement.Register();

        view = gameObject.AddComponent<PhotonView>();

        if (view.ViewID == 0)
            PhotonNetwork.AllocateViewID(view);

        PhotonNetwork.NetworkingClient.AddCallbackTarget(instance);
    }
    private void OnDestroy()
    {
        _OnConnectedToMaster = null;
        _OnDisconnectedFromMaster = null;
        _OnJoinRoom = null;
        _OnJoinRoomFailed = null;
        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;

        PhotonNetwork.NetworkingClient.RemoveCallbackTarget(instance);
    }

    public bool TryConnectToMaster(Action _OnConnectedToMaster = null, DisconnectCallBack _OnDisconnectedFromMaster = null)
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError("이미 마스터 서버에 연결되어 있는 상태에서 연결을 시도했습니다.");
            return false;
        }

        if (!CheckEnableJoinRoom())
            return false;

        this._OnConnectedToMaster = _OnConnectedToMaster;
        this._OnDisconnectedFromMaster = _OnDisconnectedFromMaster;

        PhotonNetwork.GameVersion = GameVersion;
        return PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속 시도
    }

    public void TryDisconnectToMaster()
	{
        PhotonNetwork.Disconnect();
    }

    public bool TryJoinRandomRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed)
    {
        Debug.Log($"랜덤 룸에 접속을 시도합니다.");

        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        return PhotonNetwork.JoinRandomRoom();
    }

    public bool TryJoinOrCreateRandomRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed, ENUM_MAP_TYPE mapType = ENUM_MAP_TYPE.ForestMap, int maxPlayerCount = 2,  bool isCustomRoom = false)
    {
        Debug.Log($"랜덤 룸에 접속을 시도합니다.");

        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        var roomOptions = MakeRoomOptions(CurrentMyNickname, (byte)maxPlayerCount, mapType, isCustomRoom);
        return PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: roomOptions.CustomRoomProperties,
            matchingType: MatchmakingMode.RandomMatching, 
            typedLobby: GameLobby,
            roomOptions: roomOptions);
    }

    public bool TryCreateRoom(string roomName, Action OnCreateRoom = null, FailedCallBack OnCreateRoomFailed = null, bool isCustomRoom = false, int maxPlayerCount = 2, ENUM_MAP_TYPE defaultMapType = ENUM_MAP_TYPE.ForestMap)
    {
        if (!CheckEnableJoinRoom())
            return false;

        this._OnCreateRoom = OnCreateRoom;
        this._OnCreateRoomFailed = OnCreateRoomFailed;

        var roomOptions = MakeRoomOptions(CurrentMyNickname, (byte)maxPlayerCount, defaultMapType, isCustomRoom);

        return PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby: GameLobby);
    }

    private RoomOptions MakeRoomOptions(string nickname, byte maxPlayerCount, ENUM_MAP_TYPE mapType, bool isCustomRoom)
	{
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = (byte)maxPlayerCount };

        roomOptions.CustomRoomProperties = new Hashtable();
        roomOptions.CustomRoomProperties.Add(ROOM_PROP_KEY, "");
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), mapType);
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), nickname);
		roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.IS_CUSTOM.ToString(), isCustomRoom);
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED.ToString(), false);

		roomOptions.CustomRoomPropertiesForLobby = new string[] { 
            ROOM_PROP_KEY, 
            ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), 
            ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), 
            ENUM_CUSTOM_ROOM_PROPERTIES.IS_CUSTOM.ToString(),
            ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED.ToString()
        };

        return roomOptions;
    }

    public bool TryJoinRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed, string roomName)
    {
        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        return PhotonNetwork.JoinRoom(roomName);
    }

    public bool TryLeaveRoom(Action _OnLeftRoom = null)
    {
        this._OnLeftRoom = _OnLeftRoom;
        return PhotonNetwork.LeaveRoom();
    }

    public bool TryLeaveLobby(Action _OnLeftLobby = null)
    {
        this._OnLeftLobby = _OnLeftLobby;
        return PhotonNetwork.LeaveLobby();
    }

    public bool TryJoinLobby(Action onSuccess = null, FailedCallBack onfailed = null)
    {
        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinLobby = onSuccess;
        this._OnJoinLobbyFailed = onfailed;

        return PhotonNetwork.JoinLobby(GameLobby);
    }

    public bool TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE sceneType, Action<float> OnProgress = null)
    {
        if (!CheckEnableJoinRoom())
            return false;

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("마스터 클라이언트가 아닌 경우 부를 수 없는 함수입니다.");
            return false;
        }

        PhotonNetwork.LoadLevel(sceneType.ToString());

        StartCoroutine(LoadingSceneProgress(OnProgress));

        return true;
    }

    private IEnumerator LoadingSceneProgress(Action<float> OnProgress)
	{
        while(PhotonNetwork.LevelLoadingProgress < 1.0f)
		{
            yield return null;

            OnProgress?.Invoke(PhotonNetwork.LevelLoadingProgress);
        }

        OnProgress?.Invoke(1.0f);
    }


    public override void OnConnectedToMaster() 
    {
        Debug.Log("마스터 서버에 성공적으로 접속했습니다.");
        _OnConnectedToMaster?.Invoke();

        _OnConnectedToMaster = null;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"마스터 서버로부터 접속이 끊어졌습니다. 사유 : {cause}");
        _OnDisconnectedFromMaster?.Invoke(cause.ToString());

        _OnDisconnectedFromMaster = null;
    }

	public override void OnCreatedRoom()
	{
        _OnCreateRoom?.Invoke();
        _OnCreateRoom = null;
        _OnCreateRoomFailed = null;
    }

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
        _OnCreateRoomFailed?.Invoke(returnCode, message);
        _OnCreateRoom = null;
        _OnCreateRoomFailed = null;
    }

	public override void OnJoinedLobby()
	{
        Debug.Log("로비 접속 성공");
        _OnJoinLobby?.Invoke();

        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;
    }

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
        Debug.LogError($"{returnCode} - {message} ");
        _OnJoinLobbyFailed?.Invoke(returnCode, message);

        _OnJoinLobby = null;
        _OnJoinLobbyFailed = null;
    }

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

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"룸 접속에 실패하였습니다. 코드 : {returnCode}, 사유 : {message}");
        _OnJoinRoomFailed?.Invoke(returnCode, message);
       
        _OnJoinRoom = null;
        _OnJoinRoomFailed = null;
    }

	public override void OnLeftRoom()
	{
        PhotonNetwork.AutomaticallySyncScene = false;
        Debug.LogWarning($"유저가 방을 떠났습니다.");

        _OnLeftRoom?.Invoke();
        _OnLeftRoom = null;
    }

	public override void OnLeftLobby()
	{
        Debug.LogWarning($"유저가 로비를 떠났습니다.");

        _OnLeftLobby?.Invoke();
        _OnLeftLobby = null;
    }

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
        customRoomList.Clear();
        foreach (var room in roomList)
		{
            var nickname = GetCustomProperty(room, ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME);
            var mapType = GetCustomProperty(room, ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE);
            var isCustom = GetCustomProperty(room, ENUM_CUSTOM_ROOM_PROPERTIES.IS_CUSTOM);
            var isStarted = GetCustomProperty(room, ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED);

            if (nickname == null || mapType == null)
                continue;

            CustomRoomInfo info = new CustomRoomInfo()
            {
                roomName = room.Name,
                masterClientId = room.masterClientId,

                customProperty = new CustomRoomProperty()
                {
                    masterClientNickname = (string)nickname,
                    currentMapType = (ENUM_MAP_TYPE)mapType,
                    isCustom = (bool)isCustom,
                    isStarted = (bool)isStarted,
                },

                currentPlayerCount = room.PlayerCount,
                maxPlayerCount = room.MaxPlayers,
            };

            customRoomList.Add(info);
        }

        foreach(var process in lobbyPostProcesses)
		{
            process?.OnUpdateLobby(customRoomList);
		}
    }

	public override void OnMasterClientSwitched(Player newMasterClient)
	{
        OnChangeRoomMasterClient(newMasterClient?.NickName);

        Debug.LogWarning($"{newMasterClient.NickName} 으로 방장이 바뀌었습니다.");
    }


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        var property = MakeRoomProperty(propertiesThatChanged);
        if (property == null)
            return;

        foreach (var roomPostProcess in roomPostProcesses)
        {
            roomPostProcess?.OnUpdateRoomProperty(property);
        }

        Debug.LogWarning($"{property} : 방 설정이 바뀌었습니다.");
    }

    private CustomRoomProperty MakeRoomProperty(Hashtable propertiesThatChanged)
	{
        CustomRoomProperty property = new CustomRoomProperty();

        var nickname = GetCustomProperty(propertiesThatChanged, ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME);
        var mapType = GetCustomProperty(propertiesThatChanged, ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE);
        var isCustom = GetCustomProperty(propertiesThatChanged, ENUM_CUSTOM_ROOM_PROPERTIES.IS_CUSTOM);
        var isStarted = GetCustomProperty(propertiesThatChanged, ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED);

        if (nickname == null || mapType == null)
            return null;

        property.masterClientNickname = (string)nickname;
        property.currentMapType = (ENUM_MAP_TYPE)mapType;
        property.isCustom = (bool)isCustom;
        property.isStarted = (bool)isStarted;

        return property;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
        PublishPlayerProperty(targetPlayer);

        Debug.LogWarning($"플레이어 설정이 바뀌어 데이터를 재검색합니다. 확실히 레디만 해도 불리는 게 좀 그렇긴 하다.");
    }

    private void PublishPlayerProperty(Player targetPlayer)
	{
        if (roomPostProcesses.Count <= 0)
            return;

        if (targetPlayer == null)
            return;

        var isReady = GetCustomProperty(targetPlayer, ENUM_PLAYER_STATE_PROPERTIES.READY);
        var userKey = GetCustomProperty(targetPlayer, ENUM_PLAYER_STATE_PROPERTIES.USERKEY);
        if (isReady == null || userKey == null)
            return;

        ENUM_LOGIN_TYPE loginType = (ENUM_LOGIN_TYPE)GetCustomProperty(targetPlayer, ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE);
        ENUM_CHARACTER_TYPE characterType = (ENUM_CHARACTER_TYPE)GetCustomProperty(targetPlayer, ENUM_PLAYER_STATE_PROPERTIES.CHARACTER);

        MakePlayerProperty(targetPlayer.IsMasterClient, (bool)isReady, (string)userKey, loginType, characterType);
    }

    private void MakePlayerProperty(bool isMasterClient, bool isReady, string userKey, ENUM_LOGIN_TYPE loginType, ENUM_CHARACTER_TYPE characterType)
	{
        Managers.Platform.DBSelect(loginType, userKey, OnSuccess: (userData) =>
        {
            CustomPlayerProperty property = new CustomPlayerProperty()
            {
                isMasterClient = isMasterClient,
                isReady = isReady,
                data = userData,
                characterType = characterType
            };

            foreach (var roomPostProcess in roomPostProcesses)
            {
                roomPostProcess?.OnUpdateRoomPlayerProperty(property);
            }

        }, OnFailed: () =>
        {
            Debug.LogError($"{loginType} - {userKey} 검색 실패");
        });
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
}
