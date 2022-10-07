using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class DebugWindow : BaseCanvas
{
	[SerializeField] private InputField roomNameInput = null;

	[SerializeField] private Text statusPanel = null;
	[SerializeField] private Text detailStatusPanel = null;

	public override T GetUIComponent<T>()
	{
		return default(T);
	}

	private void Start()
	{
		Managers.Platform.Initialize();
	}

	public void OnClickMasterServer()
	{
		bool a = PhotonLogicHandler.Instance.TryConnectToMaster(
			() => { SetStatus("마스터 서버 접속 완료"); },
			SetError);
	}

	public void OnClickJoinRandomRoom()
	{
		PhotonLogicHandler.Instance.TryJoinRandomRoom(
			() => { SetStatus("매칭 성공"); },
			SetError);
	}

	public void OnClickJoinRoom()
	{
		PhotonLogicHandler.Instance.TryJoinRoom(
		() => { SetStatus("매칭 성공"); },
		SetError, roomNameInput.text);
	}

	public void OnClickJoinLobby()
	{
		PhotonLogicHandler.Instance.TryJoinLobby(
			   () => { SetStatus("로비 진입 완료"); },
			  SetError);

	}

	public void OnClickCreateRoom()
	{
		PhotonLogicHandler.Instance.TryCreateRoom(
		OnCreateRoom: () => { SetStatus("방 만들기 성공"); }, 
		OnCreateRoomFailed: SetError, 
		roomName: roomNameInput.text);
	}

	public void OnClickFindCustomRoom()
	{
		foreach(var roomInfo in PhotonLogicHandler.AllRoomInfos)
		{
			Debug.Log($"룸 아이디 : {roomInfo.masterClientId}, " +
				$"룸 이름 : {roomInfo.roomName}, " +
				$"방장 이름 : {roomInfo.MasterClientNickname} " +
				$"현재 맵 : {roomInfo.CurrentMapType}");
		}
	}

	public void OnClickDBSelect()
	{
		Managers.Platform.DBSelect(ENUM_LOGIN_TYPE.Guest, "solhwi", OnSelectDBData);
	}

	public void OnClickDBUpdate(int inputData)
	{
		Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, (long)inputData, OnUpdateDBData);
	}

	private void OnSelectDBData(DBUserData data)
	{
		Debug.Log(data);
	}

	private void OnUpdateDBData(long inputData)
	{
		Debug.Log(inputData);
	}

	public void OnClickGuestLogin()
	{
		Managers.Platform.Login(ENUM_LOGIN_TYPE.Guest, () => 
		{
			string id = Managers.Platform.GetUserID();
			Debug.Log($"회원번호 : {id} 으로 로그인 완료");
		}, null, null, "godgjwnsgur7@gmail.com", "123456");
	}

	public void OnClickGuestLogout()
	{
		Managers.Platform.Logout();
	}

	public void OnClickGuestLogin2()
	{
		Managers.Platform.Login(ENUM_LOGIN_TYPE.Guest, () =>
		{
			string id = Managers.Platform.GetUserID();
			Debug.Log($"회원번호 : {id} 으로 로그인 완료");
		}, null, null, "psh50zmfhtm@gmail.com", "123456");
	}

	public void OnClickGoogleLogin()
	{
		Managers.Platform.Login(ENUM_LOGIN_TYPE.Google, () =>
		{
			string id = Managers.Platform.GetUserID();
			Debug.Log($"회원번호 : {id} 으로 로그인 완료");
		});
	}

	public void OnClickShowBanner()
	{
		Managers.Platform.ShowBanner();
	}

	public void OnClickHideBanner()
	{
		Managers.Platform.HideBanner();
	}

	public void OnClickMoveScene(string scenename)
	{
		var sceneType = (ENUM_SCENE_TYPE)Enum.Parse(typeof(ENUM_SCENE_TYPE), scenename);
		Managers.Scene.LoadScene(sceneType);
	}

	public void OnClickMoveScene()
	{
		PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE.Battle);
	}

	public void SetError(string cause)
	{
		SetStatus($"{cause} - 해당 사유로 접속 실패, 혹은 끊어짐");
	}

	public void SetError(short returnCode, string message)
	{
		SetStatus($"{returnCode} - {message}");
	}

	public void SetStatus(string txt)
	{
		statusPanel.text = txt;
		detailStatusPanel.text = PhotonLogicHandler.Instance.Info();
	}
}
