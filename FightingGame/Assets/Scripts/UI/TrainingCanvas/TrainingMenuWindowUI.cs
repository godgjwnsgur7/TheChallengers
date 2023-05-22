using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingMenuWindowUI : MonoBehaviour
{
    TrainingScene trainingScene;

    public void Init()
    {
        trainingScene = Managers.Scene.CurrentScene.GetComponent<TrainingScene>();
    }
    
    public void OnClick_ChangeMap()
    {
        // 테스트용임
        trainingScene.Change_CurrMap(ENUM_MAP_TYPE.CaveMap);
    }

    public void OnClick_ChangeEnemyCharacter()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_EnemyCharacter);
    }

    public void OnClick_ChangeMyCharacter()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(trainingScene.Change_MyCharacter);
    }

    public void OnClick_Setting()
    {
        // 로비씬에 있는 설정을 팝업캔버스로 빼서 공용으로 묶어 사용할 수 있게 변경
    }

    public void OnClick_GoToLobby() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비에 돌아가시겠습니까?");

    private void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
}
