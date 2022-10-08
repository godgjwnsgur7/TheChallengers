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

    public bool IsConnectedAndReady() => PhotonNetwork.IsConnectedAndReady;
    public bool IsMasterServer() => PhotonNetwork.Server == ServerConnection.MasterServer;
    public bool IsInLobby() => PhotonNetwork.InLobby;
    public bool IsInRoom() => PhotonNetwork.InRoom;


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

    public void RequestRoomCustomProperty()
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES);
        PhotonNetwork.CurrentRoom.SetCustomProperties(table);
    }

    public void RequestRoomList()
    {
        PhotonNetwork.GetCustomRoomList(GameLobby, "");
    }

    public void RequestCurrentPlayerProperty()
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_PLAYER_STATE_PROPERTIES);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

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