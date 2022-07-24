using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterServerButton : MonoBehaviour
{
	public void OnClickMasterServer()
	{
		PhotonLogicHandler.Instance.TryConnectToMaster();
		PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember("Battle");
		PhotonLogicHandler.Instance.TryInstantiate("");

		bool b = PhotonLogicHandler.IsConnected;
	}

	public void OnClickJoinRoom()
	{
		PlatformDB db = new PlatformDB();
		db.InitDataBase();
	}
}