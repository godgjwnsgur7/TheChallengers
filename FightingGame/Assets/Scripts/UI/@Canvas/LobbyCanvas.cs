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

        // 디버그용
        PhotonLogicHandler.Instance.TryConnectToMaster(
           () => { Debug.Log("마스터 서버 접속 완료"); Join_Lobby(); });

        // 마스터 서버에 접속은 로그인씬에서 체크하고,
        // 로비로 성공적으로 넘어왔다면, 여기서 로비에 접속?
        // 아니면 그냥 둘다 로그인씬에서? (고민, 임시)
    }


    public void Join_Lobby() // 디버그용
    {
        PhotonLogicHandler.Instance.TryJoinLobby(
               () => { Debug.Log("로비 진입 완료"); });
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
    
    public override T GetUIComponent<T>()
    {
        // 지워질 위기인 함수 (임시)
        return default(T);
    }

    
}