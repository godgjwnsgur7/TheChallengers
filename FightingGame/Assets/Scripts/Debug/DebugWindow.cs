using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugWindow : BaseCanvas
{
	[SerializeField] private InputField nicknameInput = null;

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
			SetError, nicknameInput.text);
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
		masterClientNickname: nicknameInput.text);
	}

	public void OnClickFindCustomRoom()
	{
		foreach(var roomInfo in PhotonLogicHandler.AllRoomInfos)
		{
			Debug.Log(roomInfo);
		}
	}

	public void OnClickDBSelect()
	{
		Managers.Platform.DBSelect<long>(DB_CATEGORY.VictoryPoint, ENUM_LOGIN_TYPE.Guest, "solhwi", DebugDBData);
	}

	public void OnClickDBUpdate()
	{
		long inputData = 10L;
		Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, ENUM_LOGIN_TYPE.Guest, "solhwi", inputData, DebugDBData);
	}

	private void DebugDBData(long data)
	{
		Debug.Log(data);
	}

	public void OnClickGuestLogin()
	{
		
	}

	public void OnClickMoveScene()
	{
		PhotonLogicHandler.Instance.TrySceneLoadWithRoomMember(ENUM_SCENE_TYPE.Battle, true);
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
