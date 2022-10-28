using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] CustomMatchingUI customMatching;
    [SerializeField] MatchingWindowUI matchingWindow;

    public override void Init()
    {
        base.Init();

        // 디버그용 : 마스터 서버에 접속과 로비 접속은 메인 씬에서 하고 넘어옴
        // PhotonLogicHandler.Instance.TryConnectToMaster(() => { Debug.Log("마스터 서버 접속 완료"); Join_Lobby(); });
    }


    public void Join_Lobby() // 디버그용
    {
        PhotonLogicHandler.Instance.TryJoinLobby(
               () => { Debug.Log("로비 진입 완료"); });
    }

    public void Set_InTheCustomRoom()
    {
        customMatching.gameObject.SetActive(true);

        customMatching.Set_InTheCustomRoom();
    }

    public void OnClick_CustomMathing()
    {
        if(PhotonLogicHandler.IsConnected)
        {
            customMatching.gameObject.SetActive(true);
        }
        else
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("마스터 서버에 접속해있지 않습니다.");
        }
    }
    public void OnClick_Mathing() => matchingWindow.OnClick_Matching();
    public void OnClick_Training() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_TrainingScene, null, "훈련장에 입장하시겠습니까?");

    private void GoTo_TrainingScene()
    {
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Training);
    }
}