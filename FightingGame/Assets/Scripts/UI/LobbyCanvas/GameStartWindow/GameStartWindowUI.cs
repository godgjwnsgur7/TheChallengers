using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class GameStartWindowUI : UIElement, IRoomPostProcess
{
    [SerializeField] UserInfoUI masterInfoUI;
    [SerializeField] UserInfoUI slaveInfoUI;

    UserInfoUI MyInfoUI
    {
        get
        {
            if(PhotonLogicHandler.IsMasterClient)
                return masterInfoUI;
            else
                return slaveInfoUI;
        }
    }
    UserInfoUI EnemyInfoUI
    {
        get
        {
            if (PhotonLogicHandler.IsMasterClient)
                return slaveInfoUI;
            else
                return masterInfoUI;
        }
    }

    [SerializeField] CharacterSelectArea characterSelectArea;
    [SerializeField] MapInfoUI mapInfo;
    [SerializeField] Text timerText;

    int selectionCharacterTimer = 15;

    ENUM_CHARACTER_TYPE enemySelectionCharacterType = ENUM_CHARACTER_TYPE.Default;

    Coroutine selectCharacterTimerCoroutine = null;
    Coroutine settingInfoCheckCoroutine = null;
    Coroutine waitSelectionCharacterCoroutine = null;
    Coroutine waitGameStartCoroutine = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        this.RegisterRoomCallback();

        PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnExitRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += OnExitRoomCallBack;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        CoroutineStopAll();

        PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnExitRoomCallBack;

        this.UnregisterRoomCallback();
    }

    private void CoroutineStopAll()
    {
        if (selectCharacterTimerCoroutine != null)
            StopCoroutine(selectCharacterTimerCoroutine);
        if (settingInfoCheckCoroutine != null)
            StopCoroutine(settingInfoCheckCoroutine);
        if (waitSelectionCharacterCoroutine != null)
            StopCoroutine(waitSelectionCharacterCoroutine);
        if (waitGameStartCoroutine != null)
            StopCoroutine(waitGameStartCoroutine);
    }
    
    public void Open()
    {
        EnemyInfoUI.Clear();

        PhotonLogicHandler.Instance.onLeftRoomPlayer -= OnExitRoomCallBack;
        PhotonLogicHandler.Instance.onLeftRoomPlayer += OnExitRoomCallBack;

        this.RegisterRoomCallback();

        characterSelectArea.Init(CallBack_SelectionCharacter);
        MyInfoUI.Active_SelectionCompleteBtn(CallBack_SelectionCharacterComplete);
        timerText.text = selectionCharacterTimer.ToString();

        this.gameObject.SetActive(true);

        selectCharacterTimerCoroutine = StartCoroutine(ISelectionCharacterTimer(selectionCharacterTimer));
        settingInfoCheckCoroutine = StartCoroutine(ISettingInfoCheck());
    }

    public void Close()
    {
        PhotonLogicHandler.Instance.RequestUnSyncDataAll();

        characterSelectArea.Close();
        mapInfo.Close();

        masterInfoUI.Clear();
        slaveInfoUI.Clear();

        this.gameObject.SetActive(false);
    }
    
    public void GameStart()
    {
        if(selectCharacterTimerCoroutine != null)
            StopCoroutine(selectCharacterTimerCoroutine);

        EnemyInfoUI.Set_SelectionCharacter(enemySelectionCharacterType);
        MyInfoUI.ChangeInfo_GameStart();
        EnemyInfoUI.ChangeInfo_GameStart();

        mapInfo.Open(PhotonLogicHandler.CurrentMapType);

        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Cilck_Transition);
        waitGameStartCoroutine = StartCoroutine(IWaitGameStart(3.0f));
    }

    public void OnEnterRoomCallBack(string enterUserNickname) { }

    /// <summary>
    /// 게임 시작 중에 상대방이 강제종료 한 경우
    /// </summary>
    public void OnExitRoomCallBack(string exitUserNickname)
    {
        CoroutineStopAll();

        Managers.UI.popupCanvas.Open_NotifyPopup("게임이 취소 되었습니다.", CallBack_ExitUser);
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property) { }
    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (property.isMasterClient)
            masterInfoUI.Init(property.data, property.isMasterClient);
        else
            slaveInfoUI.Init(property.data, property.isMasterClient);

        // 상대방의 캐릭터 선택 완료됨을 변수화해서 담음
        if (property.characterType != ENUM_CHARACTER_TYPE.Default
            && (PhotonLogicHandler.IsMasterClient != property.isMasterClient))
        {
            enemySelectionCharacterType = property.characterType;
        }
    }

    public void CallBack_ExitUser()
    {
        bool isLeaveRoom = PhotonLogicHandler.Instance.TryLeaveRoom(CallBack_ExitRoom);

        if (!isLeaveRoom)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("알 수 없는 에러\n나가기 실패");
        }
    }

    public void CallBack_ExitRoom()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Close);
    }

    public void CallBack_SelectionCharacter(ENUM_CHARACTER_TYPE _selectedCharType)
    {
        MyInfoUI.Set_SelectionCharacter(_selectedCharType);
    }

    public void CallBack_SelectionCharacterComplete(ENUM_CHARACTER_TYPE _selectedCharacterType)
    {
        MyInfoUI.Deactive_SelectionCompleteBtn();
        characterSelectArea.Close();

        if (PhotonLogicHandler.IsMasterClient)
            PhotonLogicHandler.Instance.ChangeCharacter(_selectedCharacterType);
        else // 슬레이브라면, 마스터의 캐릭터 선택을 확인한다.
            waitSelectionCharacterCoroutine = StartCoroutine(IWaitSelectionCharacter(_selectedCharacterType));
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Managers.UI.popupCanvas.Open_SelectPopup(() => { Application.Quit(); }
        , null, "게임을 종료하시겠습니까?\n매칭된 게임은 패배처리 됩니다.");
    }

    private IEnumerator ISelectionCharacterTimer(int _timelimit)
    {
        float updateTime = _timelimit;
        int currTime = _timelimit;
        timerText.text = currTime.ToString();

        while(updateTime > 0.1f)
        {
            updateTime -= Time.deltaTime;
            
            if((int)updateTime != currTime)
            {
                currTime = (int)updateTime;
                timerText.text = currTime.ToString();
            }

            yield return null;
        }

        timerText.text = "0";
        MyInfoUI.Forced_SelectionCharacter();

        selectCharacterTimerCoroutine = null;
    }

    private IEnumerator ISettingInfoCheck()
    {
        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();

        yield return new WaitUntil(() => masterInfoUI.IsInit && slaveInfoUI.IsInit);

        Managers.UI.popupCanvas.Play_FadeInEffect();
        settingInfoCheckCoroutine = null;
    }
    
    private IEnumerator IWaitSelectionCharacter(ENUM_CHARACTER_TYPE _selectedCharacterType)
    {
        if(PhotonLogicHandler.IsMasterClient)
        {
            waitSelectionCharacterCoroutine = null;
            yield break;
        }

        yield return new WaitUntil(() => enemySelectionCharacterType != ENUM_CHARACTER_TYPE.Default);

        PhotonLogicHandler.Instance.ChangeCharacter(_selectedCharacterType);
        waitSelectionCharacterCoroutine = null;
    }

    private IEnumerator IWaitGameStart(float _delayTime)
    {
        yield return new WaitForSeconds(_delayTime);

        Managers.UI.popupCanvas.Play_FadeOutEffect(Managers.UI.popupCanvas.Open_LoadingPopup);

        if(PhotonLogicHandler.IsMasterClient)
        {
            yield return new WaitForSeconds(1.0f);

            Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Battle);
        }

        waitGameStartCoroutine = null;
    }
}
