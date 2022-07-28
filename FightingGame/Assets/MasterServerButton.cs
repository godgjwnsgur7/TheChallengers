using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 테스트용 스크립트
/// </summary>
public class MasterServerButton : MonoBehaviour
{
	[SerializeField] Text DebugText;


	public void OnClickMasterServer()
	{
		PhotonLogicHandler.Instance.TryConnectToMaster();
		// PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember("Battle");
		// PhotonLogicHandler.Instance.TryInstantiate("");

		DebugText.text = $"IsConnected : {PhotonLogicHandler.IsConnected}";
	}

	public void OnClickJoinRandomRoom()
	{
		FailedCallBack test = delegate (short a, string b) { }; // 이거 맞냐? ㅋㅋ
		bool a = PhotonLogicHandler.Instance.TryJoinRandomRoom(
			() => { DebugText.text = "생성되어있는 방이 없습니다."; }, test);

		// PlatformDB db = new PlatformDB();
		// db.InitDataBase();

		DebugText.text = $"성공 여부 : {a}";

		if(a && PhotonLogicHandler.IsConnected)
        {
			// 블루 팀으로 설정?
        }
	}

	public void OnClickCreatRoom()
    {
		bool a = PhotonLogicHandler.Instance.TryCreateRoom();

		DebugText.text = $"성공 여부 : {a}";

		if(a && PhotonLogicHandler.IsConnected)
        {
			// 레드 팀으로 설정?
        }
	}

	public void LoadWithBattle()
    {
		PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember("Battle");
	
	}

}