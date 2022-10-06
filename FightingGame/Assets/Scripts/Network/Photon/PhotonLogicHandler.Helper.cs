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

public enum ENUM_CUSTOM_ROOM_PROPERTIES
{
    MAP_TYPE = 0,
    MASTER_CLIENT_NICKNAME = 1,
}

public enum ENUM_PLAYER_STATE_PROPERTIES
{
    READY = 0,
    USERKEY = 1,
    LOGINTYPE = 2,
    CHARACTER = 3,
}

public interface ILobbyPostProcess
{
    void OnUpdateLobby(List<CustomRoomInfo> roomList);
}

public interface IRoomPostProcess
{
    void OnUpdateRoomProperty(CustomRoomProperty property);
    void OnUpdateRoomPlayerProperty(CustomPlayerProperty property);
}

public class CustomRoomInfo
{
    public string roomName;
    public int masterClientId;
    public CustomRoomProperty customProperty;
    public int currentPlayerCount;
    public int maxPlayerCount;

    public string MasterClientNickname => customProperty != null ? customProperty.masterClientNickname : string.Empty;
    public ENUM_MAP_TYPE CurrentMapType => customProperty != null ? customProperty.currentMapType : ENUM_MAP_TYPE.BasicMap; 
}

public class CustomRoomProperty
{
    public string masterClientNickname;
    public ENUM_MAP_TYPE currentMapType;
}

public class CustomPlayerProperty
{
    public bool isReady = false;
    public ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.Default;
    public DBUserData data = null;
}

public partial class PhotonLogicHandler : ILobbyCallbacks
{
    /// <summary>
    /// 현재 자신이 마스터 서버에 연결이 되어 있는가?
    /// </summary>
    public static bool IsConnected
    {
        get
        {
            return PhotonNetwork.IsConnected;
        }
    }

    public static bool IsMasterClient
    {
        get
        {
            return PhotonNetwork.IsMasterClient;
        }
    }

    public static bool IsMine(int viewID)
    {
        PhotonView view = null;

        if (viewID == 0)
            return false;

        if (photonViewDictionary.TryGetValue(viewID, out view))
            return view.IsMine;

        return false;
    }

    public static bool IsFullRoom
	{
        get
		{
            return CurrentRoomMemberCount == CurrentRoomMemberCountMax;

        }
	}

    public static string CurrentMapName
	{
        get
		{
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(typeof(ENUM_MAP_TYPE).ToString()))
                return string.Empty;

            return (string)PhotonNetwork.CurrentRoom.CustomProperties[typeof(ENUM_MAP_TYPE).ToString()];
		}
	}

    public static string CurrentMasterClientNickname
	{
        get
		{
            return PhotonNetwork.MasterClient.NickName;

        }
	}

    public static int CurrentRoomMemberCount
    {
        get
        {
            return PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

    public static int CurrentRoomMemberCountMax
    {
        get
        {
            return PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    public static string CurrentRoomName
	{
        get
		{
            return PhotonNetwork.CurrentRoom.Name;
		}
	}

    public static bool IsJoinedRoom
    {
        get
        {
            return PhotonNetwork.InRoom;
        }
    }

    public static int UsableRoomCountOnServer
    {
        get
        {
            return PhotonNetwork.CountOfRooms;
        }
    }

    public static int Ping
    {
        get
        {
            return PhotonNetwork.GetPing();
        }
    }

    public static List<CustomRoomInfo> AllRoomInfos
	{
        get
		{
            return Instance.customRoomList;
		}
	}        

    public static CustomRoomInfo GetRoomInfo(string roomName)
	{
        var list = Instance.customRoomList;
        return list?.Find(roomInfo => roomInfo.roomName == roomName);
    }

    public static CustomRoomInfo GetRoomInfo(int masterClientId)
	{
        var list = Instance.customRoomList;
        return list?.Find(roomInfo => roomInfo.masterClientId == masterClientId);
    }

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

        if (!CheckEnableJoinRoom())
            return false;

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

    public bool TryJoinRandomRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed)
    {
        Debug.Log($"랜덤 룸에 접속을 시도합니다.");

        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        PhotonNetwork.LocalPlayer.NickName = "";

        ExitGames.Client.Photon.Hashtable optionTable = new ExitGames.Client.Photon.Hashtable()
        {

        };

        return PhotonNetwork.JoinRandomOrCreateRoom(optionTable, 0);
    }

    public bool TryJoinRoom(Action _OnJoinRoom, FailedCallBack _OnJoinRoomFailed, string roomName)
    {
        if (!CheckEnableJoinRoom())
            return false;

        this._OnJoinRoom = _OnJoinRoom;
        this._OnJoinRoomFailed = _OnJoinRoomFailed;

        PhotonNetwork.LocalPlayer.NickName = "";
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

        return PhotonNetwork.JoinLobby();
    }


    public bool IsConnectedAndReady() => PhotonNetwork.IsConnectedAndReady;
    public bool IsMasterServer() => PhotonNetwork.Server == ServerConnection.MasterServer;
    public bool IsInLobby() => PhotonNetwork.InLobby;
    public bool IsInRoom() => PhotonNetwork.InRoom;


    /// <summary>
    /// 방장이 해당 함수를 호출하게 되면, 방의 멤버가 모두 씬이 동기화된 채로 이동하게 됨
    /// </summary>
    /// <param name="sceneName"></param>

    public bool TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE sceneType, bool isDataSyncScene = false)
    {
        if (!CheckEnableJoinRoom())
            return false;

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("마스터 클라이언트가 아닌 경우 부를 수 없는 함수입니다.");
            return false;
        }

        PhotonNetwork.LoadLevel(sceneType.ToString());
        return true;
    }

    /// <summary>
    /// 방을 새로 만듦
    /// </summary>
    /// <param name="roomName"> 방의 이름 </param>
    /// <param name="maxPlayerCount"> 방 인원 최대 수 </param>
    /// <returns> 성공 여부 </returns>

    public bool TryCreateRoom(Action OnCreateRoom = null, FailedCallBack OnCreateRoomFailed = null, string roomName = "이름 없음", int maxPlayerCount = 2, ENUM_MAP_TYPE defaultMapType = ENUM_MAP_TYPE.BasicMap)
    {
        if (!CheckEnableJoinRoom())
            return false;

        this._OnCreateRoom = OnCreateRoom;
        this._OnCreateRoomFailed = OnCreateRoomFailed;

        PhotonNetwork.LocalPlayer.NickName = "허준혁";

        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = (byte)maxPlayerCount };

        roomOptions.CustomRoomProperties = new Hashtable();

        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), defaultMapType);
        roomOptions.CustomRoomProperties.Add(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString(), "허준혁");

        roomOptions.CustomRoomPropertiesForLobby = new string[] { ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString() };

        return PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    /// <summary>
    /// 현재 맵 변경
    /// </summary>
    /// <param name="mapType"></param>
    /// <returns></returns>
    public bool ChangeMap(ENUM_MAP_TYPE mapType)
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString()))
            return false;

        PhotonNetwork.CurrentRoom.CustomProperties[ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString()] = mapType;
        return true;
    }

    /// <summary>
    /// 현재 방장이 변경된 경우 알아서 호출됨
    /// </summary>
    /// <param name="nickname"></param>
    private void OnChangeRoomMasterClient(string nickname)
    {
        if (!PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString()))
            return;

        PhotonNetwork.CurrentRoom.CustomProperties[ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME.ToString()] = nickname;

        onChangeMasterClientNickname?.Invoke(nickname);
    }

    /// <summary>
    /// 캐릭터 변경
    /// </summary>
    /// <param name="characterType"></param>

    public void ChangeCharacter(ENUM_CHARACTER_TYPE characterType)
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString()))
            return;

        PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString()] = characterType;
    }

    /// <summary>
    /// 모두 준비됐나요?
    /// </summary>
    /// <returns></returns>

    public bool IsAllReady()
    {
        var players = PhotonNetwork.PlayerList;
        string readyStr = ENUM_PLAYER_STATE_PROPERTIES.READY.ToString();
        return players.All(p => p.CustomProperties.ContainsKey(readyStr) && (bool)p.CustomProperties[readyStr]);
    }

    /// <summary>
    /// 레디
    /// </summary>

    public void Ready()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()))
            return;

        PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()] = true;
    }

    /// <summary>
    /// 레디 풀기
    /// </summary>

    public void UnReady()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()))
            return;

        PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()] = false;
    }

    /// <summary>
    /// 유저가 방에 들어가기 전 반드시 자신의 정보를 세팅해야 한다.
    /// </summary>
    /// <param name="userKey"></param>
    /// <param name="loginType"></param>
    private void SetUserInfo(string userKey, ENUM_LOGIN_TYPE loginType)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.USERKEY.ToString()))
		{
            PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.USERKEY.ToString()] = userKey;
        }
		else
		{
            PhotonNetwork.LocalPlayer.CustomProperties.Add(ENUM_PLAYER_STATE_PROPERTIES.USERKEY.ToString(), userKey);
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE.ToString()))
        {
            PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE.ToString()] = loginType;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add(ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE.ToString(), loginType);
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()))
        {
            PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.READY.ToString()] = false;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add(ENUM_PLAYER_STATE_PROPERTIES.READY.ToString(), false);
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString()))
        {
            PhotonNetwork.LocalPlayer.CustomProperties[ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString()] = ENUM_CHARACTER_TYPE.Default;
        }
        else
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER.ToString(), ENUM_CHARACTER_TYPE.Default);
        }
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
}