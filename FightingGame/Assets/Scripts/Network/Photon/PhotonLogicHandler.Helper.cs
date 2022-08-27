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

    public bool IsFullRoom
	{
        get
		{
            return CurrentRoomMemberCount == CurrentRoomMemberCountMax;

        }
	}

    public string CurrentMapName
	{
        get
		{
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(typeof(ENUM_MAP_TYPE).ToString()))
                return string.Empty;

            return (string)PhotonNetwork.CurrentRoom.CustomProperties[typeof(ENUM_MAP_TYPE).ToString()];
		}
	}

    public string CurrentMasterClientNickname
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
}