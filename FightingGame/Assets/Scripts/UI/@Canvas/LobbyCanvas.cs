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
    [SerializeField] FightingInfoWindow fightingInfoWindow;
    [SerializeField] GameStartWindowUI gameStartWindow;

    public override void Init()
    {
        base.Init();

        if (PhotonLogicHandler.IsJoinedRoom)
        {
            if(PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.RANDOM)
            {
                PhotonLogicHandler.Instance.TryLeaveRoom();
            }
            else
            {
                if (PhotonLogicHandler.IsMasterClient)
                {
                    Managers.Network.Start_SequenceExecuter();
                }

                Set_InTheCustomRoom();
            }
        }
    }

    public void GameStart()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(gameStartWindow.GameStart);
    }

    public void Open_CustomRoomWindow() => customMatching.Open();
    public void Close_CustomRoomWindow() => customMatching.Close();
    public void Open_FightingInfoWindow() => fightingInfoWindow.Open();

    public void Open_GameStartWindow()
    {
        Managers.UI.popupCanvas.Play_FadeOutEffect(gameStartWindow.Open);
    }

    public void OnClick_CustomMathing()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click);

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

    public void MathingStart()
    {
        PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.RANDOM, matchingWindow.Open);
    }

    public void OnClick_Training() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_TrainingScene, null, "훈련장에 입장하시겠습니까?");

    /// <summary>
    /// 커스텀룸 안에 있는 상태로 로비씬으로 넘어왔을 때 호출
    /// </summary>
    public void Set_InTheCustomRoom()
    {
        customMatching.Set_InTheCustomRoom();
    }

    private void GoTo_TrainingScene()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Training);
    }

    public void Open_InputKeyManagement(Transform _transform)
    {
        InputKeyManagement go = Managers.Input.Get_InputKeyManagement();
        go.transform.parent = _transform;
        go.Init();
    }
}