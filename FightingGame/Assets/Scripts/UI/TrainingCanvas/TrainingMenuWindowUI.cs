using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingMenuWindowUI : UIElement
{
    TrainingScene trainingScene;

    public void Init()
    {
        trainingScene = Managers.Scene.CurrentScene.GetComponent<TrainingScene>();
    }
    
    public void OnClick_ChangeMap(int mapTypeNum)
    {
        trainingScene.Change_CurrMap((ENUM_MAP_TYPE)mapTypeNum);
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
        Managers.UI.popupCanvas.Open_SettingWindow();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_GoToLobby();       
    }

    public void OnClick_GoToLobby() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비로 돌아가시겠습니까?");

    private void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
}
