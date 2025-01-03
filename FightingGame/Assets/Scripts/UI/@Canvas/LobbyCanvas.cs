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

    [SerializeField] Image[] images = new Image[3];

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < images.Length; i++)
            images[i].alphaHitTestMinimumThreshold = 0.1f;

        if (!Managers.Data.isInterstitial)
        {
            // Managers.Platform.ShowInterstitial();
            Managers.Data.isInterstitial = true;
        }

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
        matchingWindow.Close();   
        gameStartWindow.GameStart();
    }

    public void Open_CustomRoomWindow() => customMatching.Open();
    public void Close_CustomRoomWindow() => customMatching.Close();
    public void CustomRoomLeftCallBack() => customMatching.CustomRoomLeftCallBack();

    public void Open_GameStartWindow()
    {
        if(PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_MacthingCompleted);

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

    public void MathingStop()
    {
        matchingWindow.Close();
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

    private void GoTo_MainScene()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Main);
    }

    /*
    public void OnClick_DonationWindow()
    {
        if (PhotonLogicHandler.IsConnected)
        {
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Light);

            donationWindow.Open();
        }
        else
        {
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

            Managers.UI.popupCanvas.Open_NotifyPopup("서버에 접속해있지 않습니다.\n로그인을 위해 메인화면으로 이동합니다.", GoTo_MainScene);
        }
    }
    */

    public void OnClick_CustomMathing()
    {
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.CUSTOM, customMatching.Open);
        }
        else
        {
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

            Managers.UI.popupCanvas.Open_NotifyPopup("서버에 접속해있지 않습니다.\n로그인을 위해 메인화면으로 이동합니다.", GoTo_MainScene);
        }
    }

    public void OnClick_Mathing()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

        if (PhotonLogicHandler.IsConnected)
        {
            Managers.UI.popupCanvas.Open_SelectPopup(MathingStart, null, "랭킹전 매치를 시작하시겠습니까?");
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("서버에 접속해있지 않습니다.\n로그인을 위해 메인화면으로 이동합니다.", GoTo_MainScene);
        }
    }
    public void OnClick_Training()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

        Managers.UI.popupCanvas.Open_SelectPopup(GoTo_TrainingScene, null, "훈련장에 입장하시겠습니까?");
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