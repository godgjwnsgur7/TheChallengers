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


public partial class PhotonLogicHandler
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

    public static ENUM_MAP_TYPE CurrentMapType
    {
        get
        {
            var table = Instance.GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES);
            if (table == null)
                return ENUM_MAP_TYPE.CaveMap;

            if (!table.TryGetValue(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE.ToString(), out object value))
                return ENUM_MAP_TYPE.CaveMap;

            return (ENUM_MAP_TYPE)value;
        }
    }

    public static string CurrentMasterClientNickname
    {
        get
        {
            return PhotonNetwork.MasterClient.NickName;

        }
    }

    public static string CurrentMyNickname
    {
        get
        {
            return PhotonNetwork.LocalPlayer.NickName;
        }
        set
        {
            PhotonNetwork.LocalPlayer.NickName = value;
            Managers.Platform.DBUpdate(DB_CATEGORY.Nickname, value);
        }
    }

    public static int CurrentRoomMemberCount
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null)
                return 0;

            return PhotonNetwork.CurrentRoom.PlayerCount;
        }
    }

    public static int CurrentRoomMemberCountMax
    {
        get
        {
            if (PhotonNetwork.CurrentRoom == null)
                return 2;

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

    public static bool IsLoadingScene
	{
        get
		{
            return CurrentLoadingProgress > Mathf.Epsilon && CurrentLoadingProgress < 1.0f - Mathf.Epsilon;
        }
	}

    public static float CurrentLoadingProgress
    {
        get
        {
            return PhotonNetwork.LevelLoadingProgress;
        }
    }

    public bool IsConnectedAndReady() => PhotonNetwork.IsConnectedAndReady;
    public bool IsMasterServer() => PhotonNetwork.Server == ServerConnection.MasterServer;
    public bool IsInLobby() => PhotonNetwork.InLobby;
    public bool IsInRoom() => PhotonNetwork.InRoom;

    public ENUM_MATCH_TYPE CurrentLobbyType
    {
        get
        {
            if (PhotonNetwork.CurrentLobby == null)
                return ENUM_MATCH_TYPE.NONE;

            switch(PhotonNetwork.CurrentLobby.Name)
            {
                case RandomVersion:
                    return ENUM_MATCH_TYPE.RANDOM;

                case CustomVersion:
                    return ENUM_MATCH_TYPE.CUSTOM;

                default:
                    return ENUM_MATCH_TYPE.NONE;
            }
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

    public static CustomRoomInfo GetRoomInfoList(bool isCustom)
	{
        var list = Instance.customRoomList;
        return list?.Find(roomInfo => roomInfo.IsCustom == isCustom);
    }

    public static CustomRoomInfo GetRoomInfo(int masterClientId)
	{
        var list = Instance.customRoomList;
        return list?.Find(roomInfo => roomInfo.masterClientId == masterClientId);
    }

    public GameObject TryInstantiate(string prefabPath, Vector3 pos = default, Quaternion quaternion = default)
    {
        return PhotonNetwork.Instantiate(prefabPath, pos, quaternion);
    }

    public void TryDestroy(MonoBehaviourPhoton obj)
    {
        if (obj == null)
            return;

        PhotonNetwork.Destroy(obj.gameObject);
    }

    // 기본적으로 PhotonLogicHandler를 제외한 현재 모든 씬의 포톤 객체를 삭제시킨다.
    // 제외하기 원하는 타입을 매개변수로 넘길 수 있음
    public IEnumerator TryDestroyAllPhotonOnScene(bool doJustMasterClient, params Type[] ignoreTypes)
    {
        var photons = FindObjectsOfType<MonoBehaviourPhoton>();
        if (photons == null)
            yield break;

        var photonTypes = photons.Select(photon => photon.GetType()).Where(type => type != this.GetType());

        if(ignoreTypes != null)
        {
            photonTypes = photonTypes.Where(type => ignoreTypes.Contains(type) == false);
        }

        foreach(var photon in photons)
        {
            yield return null;

            if(IsMasterClient || !doJustMasterClient)
            {
                TryDestroy(photon);
            }
        }

        foreach(var type in photonTypes)
        {
            while(FindObjectOfType(type) != null)
            {
                yield return null;
            }
        }

        RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD);

        while(IsReadyUnloadBattleScene() == false) // 양 쪽 모두 배틀씬을 언로드할 준비가 되었는가?
        {
            yield return null;
        }

        RequestUnSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD);
    }

    public bool IsReadyUnloadBattleScene()
    {
        var propertyValues = GetAllPlayerProperties(ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD);
        return propertyValues.Select(value => (bool)value).All(isReady => isReady == true);
    }

    public IEnumerable<object> GetAllPlayerProperties(ENUM_PLAYER_STATE_PROPERTIES propertyType)
    {
        var players = PhotonNetwork.PlayerList;
        if (players == null || players.Length < 2)
            yield break;

        foreach (var player in players)
        {
            if(player.CustomProperties.TryGetValue(propertyType.ToString(), out var value))
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// 호출 시 현재 참여 중인 방의 OnUpdateRoomProperty가 불림
    /// </summary>

    public void RequestRoomCustomProperty()
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES);
        PhotonNetwork.CurrentRoom.SetCustomProperties(table);
    }

    /// <summary>
    /// 호출 시 현재 로비 내 룸 리스트에 대한 OnUpdateLobby가 불림
    /// </summary>

    public void RequestRoomList()
    {
        PhotonNetwork.GetCustomRoomList(matchLobbyDictionary[ENUM_MATCH_TYPE.CUSTOM], $"{ROOM_PROP_KEY} = ''");
    }

    /// <summary>
    /// 호출 시 본인에 대한 OnUpdateRoomPlayerProperty가 불림
    /// </summary>
    public void RequestCurrentPlayerProperty()
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_PLAYER_STATE_PROPERTIES);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

    /// <summary>
    /// 호출 시 모든 플레이어에 대하여 OnUpdateRoomPlayerProperty가 불림
    /// </summary>
    public void RequestEveryPlayerProperty()
	{
        var players = PhotonNetwork.PlayerList;
        if (players == null)
            return;

        foreach(var player in players)
		{
            player.SetCustomProperties(player.CustomProperties);
		}
	}
}