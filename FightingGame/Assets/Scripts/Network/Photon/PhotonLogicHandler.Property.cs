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
    public bool isMasterClient = false;
    public bool isReady = false;
    public ENUM_CHARACTER_TYPE characterType = ENUM_CHARACTER_TYPE.Default;
    public DBUserData data = null;
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

    private Hashtable GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE type)
    {
        switch (type)
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

    private object GetCustomProperty(RoomInfo roomInfo, ENUM_CUSTOM_ROOM_PROPERTIES propertyType)
    {
        var table = GetCustomPropertyTable(roomInfo);

        if (table.TryGetValue(propertyType.ToString(), out object value))
            return value;

        return null;
    }

    private object GetCustomProperty(Hashtable table, ENUM_CUSTOM_ROOM_PROPERTIES propertyType)
	{
        if (table.TryGetValue(propertyType.ToString(), out object value))
            return value;

        return null;
    }

    private object GetCustomProperty(Player player, ENUM_PLAYER_STATE_PROPERTIES propertyType)
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
    private void SetCustomRoomPropertyTable(Hashtable table, ENUM_CUSTOM_ROOM_PROPERTIES property, object value)
    {
        if (table.ContainsKey(property.ToString()))
        {
            table[property.ToString()] = value;
        }
        else
        {
            table.Add(table[property.ToString()], value);
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

        if (table.ContainsKey(propertyType))
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
