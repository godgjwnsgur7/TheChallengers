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
    MASTER_CLIENT_POINT = 2,
    IS_CUSTOM = 3,
    IS_STARTED = 4,
}

public enum ENUM_PLAYER_STATE_PROPERTIES
{
    READY = 0, // 레디 완료
    USERKEY = 1,
    LOGINTYPE = 2,
    CHARACTER = 3,
    DATA_SYNC = 4, // 데이터 싱크 완료
    CHARACTER_SYNC = 5, // 캐릭터 로드 완료
    SCENE_SYNC = 6, // 배틀 씬 로드 완료
    SCENE_UNLOAD = 7 // 배틀 씬 언로드 준비 완료
}

public class CustomRoomInfo
{
    public string roomName;
    public CustomRoomProperty customProperty;
    public int currentPlayerCount;
    public int maxPlayerCount;

    public string MasterClientNickname => customProperty != null ? customProperty.masterClientNickname : string.Empty;
    public ENUM_MAP_TYPE CurrentMapType => customProperty != null ? customProperty.currentMapType : ENUM_MAP_TYPE.CaveMap;
    public bool IsCustom => customProperty != null ? customProperty.isCustom : false;
    public bool IsStarted => customProperty != null ? customProperty.isStarted : false;
}

public class CustomRoomProperty
{
    public string masterClientNickname;
    public ENUM_MAP_TYPE currentMapType = ENUM_MAP_TYPE.CaveMap;
    public bool isCustom;
    public bool isStarted;
    public long masterClientRatingPoint;
}

public class CustomPlayerProperty
{
    public bool isMasterClient = false;
    public bool isDataSync = false;
    public bool isReady = false;
    public bool isSceneSync = false;
    public bool isSceneUnSync = false;
    public bool isCharacterSync = false;
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


public partial class PhotonLogicHandler : ILobbyCallbacks, IInRoomCallbacks
{
    private List<CustomRoomInfo> customRoomList = new List<CustomRoomInfo>();

    private List<ILobbyPostProcess> lobbyPostProcesses = new List<ILobbyPostProcess>();
    private List<IRoomPostProcess> roomPostProcesses = new List<IRoomPostProcess>();

    public void ChangeMap(ENUM_MAP_TYPE mapType)
    {
        SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.MAP_TYPE, mapType);
    }

    private void OnChangeRoomMasterClient(Player masterPlayer)
    {
        var userKeyObj = GetCustomProperty(masterPlayer, ENUM_PLAYER_STATE_PROPERTIES.USERKEY);
        var userLoginType = GetCustomProperty(masterPlayer, ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE);

        string userKey = (string)userKeyObj;
        ENUM_LOGIN_TYPE loginType = (ENUM_LOGIN_TYPE)userLoginType;

        Managers.Platform.DBSelect(loginType, userKey, (userData) =>
        {
            SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_NICKNAME, masterPlayer.NickName);
            SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.MASTER_CLIENT_POINT, userData.ratingPoint);

            onChangeMasterClientNickname?.Invoke(masterPlayer?.NickName);
        });
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

    public bool IsBoolTypeProperty(ENUM_PLAYER_STATE_PROPERTIES property)
    {
        return ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD == property ||
            ENUM_PLAYER_STATE_PROPERTIES.CHARACTER_SYNC == property ||
            ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC == property ||
            ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC == property ||
            ENUM_PLAYER_STATE_PROPERTIES.READY == property;
	}

    public void OnReady()
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, true, true);
    }

    public void OnUnReady()
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, false, true);
    }

    public bool IsAllSync(ENUM_PLAYER_STATE_PROPERTIES playerProperty)
    {
        var players = PhotonNetwork.PlayerList;
        string syncStr = playerProperty.ToString();
        return players.All(p => p.CustomProperties.ContainsKey(syncStr) && (bool)p.CustomProperties[syncStr]);
    }

    public void RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES playerProperty)
    {
        SetCustomPlayerPropertyTable(playerProperty, true, true);
    }

    public void RequestUnSyncData(ENUM_PLAYER_STATE_PROPERTIES playerProperty)
    {
        SetCustomPlayerPropertyTable(playerProperty, false, true);
    }

    public void RequestGameStart()
    {
        SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED, true);
    }

    public void RequestGameEnd()
    {
        SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES.IS_STARTED, false);
    }

    public void RequestSyncDataAll()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC, true);
        }

        RequestEveryPlayerProperty();
    }

    public void RequestReadyAll()
	{
        foreach (var player in PhotonNetwork.PlayerList)
        {
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.READY, true);
        }

        RequestEveryPlayerProperty();
    }

    public void RequestUnReadyAll()
	{
        foreach(var player in PhotonNetwork.PlayerList)
		{
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.READY, false);
        }

        RequestEveryPlayerProperty();
    }

    public void RequestUnSyncDataAll()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC, false);
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.READY, false);
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC, false);
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD, false);
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.CHARACTER_SYNC, false);
            SetCustomPlayerPropertyTable(player, ENUM_PLAYER_STATE_PROPERTIES.CHARACTER, ENUM_CHARACTER_TYPE.Default);
        }

        RequestEveryPlayerProperty();
    }

    private void RequestSetUserInfo(string userKey, ENUM_LOGIN_TYPE loginType)
    {
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.USERKEY, userKey, false);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.LOGINTYPE, loginType, false);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.READY, false, false);

        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.DATA_SYNC, false, false);

        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC, false, false);
        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.SCENE_UNLOAD, false, false);

        SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES.CHARACTER_SYNC, false, false);

        // 얘가 맨 마지막이어야
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
		if (!PhotonNetwork.InRoom)
        {
            return null;
        }

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

        if (table.ContainsKey(propertyType.ToString()))
        {
            table[propertyType.ToString()] = value;
        }
        else
        {
            table.Add(propertyType.ToString(), value);
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

        if (table.ContainsKey(propertyType.ToString()))
        {
            table[propertyType.ToString()] = value;
        }
        else
        {
            table.Add(propertyType.ToString(), value);
        }
    }

    private void SetCustomPlayerPropertyTable(ENUM_PLAYER_STATE_PROPERTIES propertyType, object value, bool isUpdate = true)
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_PLAYER_STATE_PROPERTIES);

        if (table.ContainsKey(propertyType.ToString()))
        {
            table[propertyType.ToString()] = value;
        }
        else
        {
            table.Add(propertyType.ToString(), value);
        }

        if(isUpdate)
            PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

    private void SetCustomRoomPropertyTable(ENUM_CUSTOM_ROOM_PROPERTIES propertyType, object value)
    {
        var table = GetCustomPropertyTable(ENUM_CUSTOM_PROPERTIES_TYPE.ENUM_CUSTOM_ROOM_PROPERTIES);
        if (table == null)
            return;

        if (table.ContainsKey(propertyType.ToString()))
        {
            table[propertyType.ToString()] = value;
        }
        else
        {
            table.Add(propertyType.ToString(), value);
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(table);
    }
}
