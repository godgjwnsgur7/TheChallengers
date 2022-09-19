using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FGDefine;
public class LobbyCanvas : BaseCanvas
{
    [SerializeField] CustomWindow customWindow;

    public override void Init()
    {
        base.Init();

        // 마스터 서버에 접속은 로그인씬에서 체크하고,
        // 로비로 성공적으로 넘어왔다면, 여기서 로비에 접속?
    }

    public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public void OnClick_Training()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(GoTo_TrainingScene, null
            , "훈련장에 입장하시겠습니까?");
    }
     
    public void GoTo_TrainingScene()
    {
        Managers.Scene.FadeLoadScene(ENUM_SCENE_TYPE.Training);
    }

    public override T GetUIComponent<T>()
    {
        // 지워질 위기인 함수
        return default(T);
    }
}