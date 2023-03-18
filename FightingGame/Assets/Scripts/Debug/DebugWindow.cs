using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class DebugWindow : BaseCanvas, ILobbyPostProcess, IRoomPostProcess
{
	[SerializeField] private InputField roomNameInput = null;

	[SerializeField] private Text statusPanel = null;
	[SerializeField] private Text detailStatusPanel = null;

	[SerializeField] private Image blurImage = null;

	public override T GetUIComponent<T>()
	{
		return default(T);
	}

	private void Start()
	{
		Managers.Platform.Initialize();
		this.RegisterLobbyCallback();
		this.RegisterRoomCallback();

		blurImage.SetBlur(true);
	}

	private void OnDestroy()
	{
		this.UnregisterLobbyCallback();
		this.UnregisterRoomCallback();
	}

	public void OnClickMasterServer()
	{
		bool a = PhotonLogicHandler.Instance.TryConnectToMaster(
			() => { SetStatus("마스터 서버 접속 완료"); },
			SetError);
	}

	public void OnClickDisconnectMasterServer()
	{
		PhotonLogicHandler.Instance.TryDisconnectToMaster();
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

	public void OnClickJoinLobby(int type)
	{
		PhotonLogicHandler.Instance.TryJoinLobby((ENUM_MATCH_TYPE)type,
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

	public void OnClickLeaveRoom()
	{
		PhotonLogicHandler.Instance.TryLeaveRoom(() => { SetStatus("방 탈출 완료"); });
	}

	public void OnClickLeaveLobby()
	{
		PhotonLogicHandler.Instance.TryLeaveLobby(() => { SetStatus("로비 탈출 완료"); });
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
		Managers.Platform.DBSelect(ENUM_LOGIN_TYPE.Guest, "vdF1LpBzJYZahUGZLHiYtLKiXtD3", OnSelectDBData);
	}

	public void OnClickDBUpdate(int inputData)
	{
		Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, (long)inputData, OnUpdateDBData);
	}

	public void OnInitDBRatinigPoint(long ratingPoint)
    {
		Debug.Log($"레이팅 점수를 {ratingPoint} 점으로 초기화됐습니다.");
		SetStatus($"DB데이터 초기화");
	}

	public void OnClick_InitDB(int ratingPoint)
    {
		Managers.Platform.DBUpdate(DB_CATEGORY.RatingPoint, (long)ratingPoint, OnInitDBRatinigPoint);
		Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, (long)0);
		Managers.Platform.DBUpdate(DB_CATEGORY.DefeatPoint, (long)0);
	}

	private void OnSelectDBData(DBUserData data)
	{
		Debug.Log("닉네임 : " + data.nickname);
		Debug.Log("승수 : " + data.victoryPoint);
		Debug.Log("패수 : " + data.defeatPoint);
		Debug.Log("RP : " + data.ratingPoint);
		Debug.Log("커피 구매 횟수 : " + data.purchaseCoffeeCount);
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
			SetStatus("로그인 성공");

			PhotonLogicHandler.CurrentMyNickname = "godgjwnsgur";

		}, null, null, email: "godgjwnsgur7@gmail.com", password:"123456");
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
			SetStatus("로그인 성공");

			PhotonLogicHandler.CurrentMyNickname = "sorikun";

		}, null, null, email: "psh50zmfhtm@gmail.com", password: "123456");
	}

	public void OnClickGoogleLogin()
	{
		Managers.Platform.Login(ENUM_LOGIN_TYPE.Google, 
		_OnSignInSuccess: () =>
		{
			string id = Managers.Platform.GetUserID();
			Debug.Log($"회원번호 : {id} 으로 로그인 완료");
		},
		_OnCheckFirstUser: (bool isFirstLogin) =>
		{
			if(isFirstLogin)
			{
				// 최초 로그인이면 창 띄워서 닉네임 받아서 이렇게 세팅을 해줘야 합니다.
				PhotonLogicHandler.CurrentMyNickname = "sorikun";
			}
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

	public void OnClickChangeMyCharacter(int character)
	{
		if (character >= (int)ENUM_CHARACTER_TYPE.Max)
			return;
		PhotonLogicHandler.Instance.ChangeCharacter((ENUM_CHARACTER_TYPE)character);
	}

	public void OnClickChanageMap(int mapType)
	{
		if (mapType > (int)ENUM_MAP_TYPE.ForestMap)
			return;
		PhotonLogicHandler.Instance.ChangeMap((ENUM_MAP_TYPE)mapType);
	}

	public void OnClickReady()
	{
		PhotonLogicHandler.Instance.OnReady();
	}

	public void OnClickUnready()
	{
		PhotonLogicHandler.Instance.OnUnReady();
	}

	public void OnClickRequestRoomCustomProperty()
	{
		PhotonLogicHandler.Instance.RequestRoomCustomProperty();
	}

	public void OnClickRequestRoomList()
	{
		PhotonLogicHandler.Instance.RequestRoomList();
	}

	public void OnClickRequestCurrentPlayerProperty()
	{
		PhotonLogicHandler.Instance.RequestCurrentPlayerProperty();
	}

	public void OnClickRequestEveryPlayerProperty()
	{
		PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
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
		if (this == null || gameObject == null)
			return;

		statusPanel.text = txt;
		detailStatusPanel.text = PhotonLogicHandler.Instance.Info();
	}

	public void OnUpdateLobby(List<CustomRoomInfo> roomList)
	{
		foreach(var room in roomList)
		{
			Debug.Log($"현재 맵 : {room.CurrentMapType}");
			Debug.Log($"방장 고유 ID : {room.masterClientId}");
			Debug.Log($"방장 닉네임 : {room.MasterClientNickname}");
			Debug.Log($"현재 방 인원 : {room.currentPlayerCount}");
			Debug.Log($"방 최대 인원 : {room.maxPlayerCount}");
			Debug.Log($"커스텀한 방인가? : {room.IsCustom}");
			Debug.Log($"이미 게임이 시작된 방인가? : {room.IsStarted}");
		}
	}

	public void OnUpdateRoomProperty(CustomRoomProperty property)
	{
		Debug.Log($"현재 맵 : {property.currentMapType}");
		Debug.Log($"방장 닉네임 : {property.masterClientNickname}");
	}

	public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
	{
		Debug.Log($"현재 캐릭터 : {property.characterType}");

		Debug.Log($"이색휘 마스터 클라이언트임? : {property.isMasterClient}");
		Debug.Log($"레디 한 거임? : {property.isReady}");

		Debug.Log("닉네임 : " + property.data.nickname);
		Debug.Log("승수 : " + property.data.victoryPoint);
		Debug.Log("패수 : " + property.data.defeatPoint);
		Debug.Log("RP : " + property.data.ratingPoint);
		Debug.Log("커피 구매 횟수 : " + property.data.purchaseCoffeeCount);
	}
}
