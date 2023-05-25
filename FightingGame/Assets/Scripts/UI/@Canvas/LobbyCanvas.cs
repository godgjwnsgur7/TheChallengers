using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] CustomMatchingUI customMatching;
    [SerializeField] MatchingWindowUI matchingWindow;
    [SerializeField] GameStartWindowUI gameStartWindow;

    public override void Init()
    {
        base.Init();

        if (PhotonLogicHandler.IsJoinedRoom)
        {
            if(PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
            {
                Set_InTheCustomRoom();
            }
        }
    }

    public void GameStart()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(GameStart_CallBack);
    }

    public void GameStart_CallBack()
    {
        gameStartWindow.GameStart();

        matchingWindow.Close();
    }

    public void Open_CustomRoomWindow() => customMatching.Open();
    public void Close_CustomRoomWindow() => customMatching.Close();

    public void Open_GameStartWindow()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(CallBack_GameStart);
    }

    public void CallBack_GameStart()
    {
        gameStartWindow.Open();
        matchingWindow.Close();
    }

    public void MathingStart()
    {
        StartCoroutine(IWaitMatching());
    }

    /// <summary>
    /// 커스텀룸 안에 있는 상태로 로비씬으로 넘어왔을 때 호출
    /// </summary>
    public void Set_InTheCustomRoom()
    {
        customMatching.Open();
    }

    private void GoTo_TrainingScene()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Training);
    }

    public void OnClick_CustomMathing()
    {
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.CUSTOM, customMatching.Open);
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("서버에 접속해있지 않습니다.");
        }
    }

    public void OnClick_Mathing()
    {
        if (PhotonLogicHandler.IsConnected)
        {
            Managers.UI.popupCanvas.Open_SelectPopup(MathingStart, null, "랭킹전(매칭)을 돌리시겠습니까?");
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("서버에 접속해있지 않습니다.");
        }
    }
    public void OnClick_Training()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(GoTo_TrainingScene, null, "훈련장에 입장하시겠습니까?");
    }

    public void OnClick_Setting()
    {
        Managers.UI.popupCanvas.Open_SettingWindow();
    }

    private IEnumerator IWaitMatching()
    {
        int count = 0;

        if (!PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.RANDOM, matchingWindow.Open))
        {
            while (!PhotonLogicHandler.Instance.TryLeaveLobby() && count < 5)
            {
                yield return null;
                PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.RANDOM, matchingWindow.Open);
                count++;
            }
        }
    }
}