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

public enum ENUM_CUSTOM_PROPERTIES_TYPE
{
    ENUM_CUSTOM_ROOM_PROPERTIES = 0,
    ENUM_PLAYER_STATE_PROPERTIES = 1,
}

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

    private Hashtable GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE type)
	{
        switch(type)
		{
            case ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES:
                return PhotonNetwork.CurrentRoom.CustomProperties;

            case ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_PLAYER_STATE_PROPERTIES:
                return PhotonNetwork.LocalPlayer.CustomProperties;

            default:
                return null;
        }
	}

    private Hashtable GetCustomPropertyTable(Player player)
	{
        return player.CustomProperties;
	}

    private Hashtable GetCustomPropertyTable(RoomInfo roomInfo)
	{
        return roomInfo.CustomProperties;
	}

    private object GetCustomPropertyTable(RoomInfo roomInfo, ENUM_CUSTOM_ROOM_PROPERTIES propertyType)
	{
        var table = GetCustomPropertyTable(roomInfo);

        if(table.TryGetValue(propertyType.ToString(), out object value))
            return value;

        return null;
    }

    private object GetCustomPropertyTable(Player player, ENUM_PLAYER_STATE_PROPERTIES propertyType)
    {
        var table = GetCustomPropertyTable(player);

        if (table.TryGetValue(propertyType.ToString(), out object value))
            return value;

        return null;
    }

    private void SetCustomPlayerPropertyTable(Player player, ENUM_PLAYER_STATE_PROPERTIES propertyType, object value)
	{
        var table = GetCustomPropertyTable(player);

        if (table.ContainsKey(propertyType))
        {
            table[propertyType] = value;
        }
        else
        {
            table.Add(propertyType, value);
        }
    }

    private void SetCustomRoomPropertyTable(RoomInfo roomInfo, ENUM_PLAYER_STATE_PROPERTIES propertyType, object value)
	{
        var table = GetCustomPropertyTable(roomInfo);

        if (table.ContainsKey(propertyType))
        {
            table[propertyType] = value;
        }
        else
        {
            table.Add(propertyType, value);
        }
    }

    private void SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES propertyType, object value)
	{
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_PLAYER_STATE_PROPERTIES);
        
        if(table.ContainsKey(propertyType))
		{
            table[propertyType] = value;
        }
        else
		{
            table.Add(propertyType, value);
        }
    }

    private void SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES propertyType, object value)
	{
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES);
        
        if (table.ContainsKey(propertyType))
        {
            table[propertyType] = value;
        }
        else
        {
            table.Add(propertyType, value);
        }

    }

}