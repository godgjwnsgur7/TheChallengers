using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class TrainingCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI blueTeamStatusWindow;
    [SerializeField] StatusWindowUI redTeamStatusWindow;
    [SerializeField] ButtonPanel buttonPanel;

    public override void Init()
    {
        base.Init();

        buttonPanel.Init();
    }

    public Action<float> Get_StatusWindowUI(ENUM_TEAM_TYPE _teamType, ENUM_CHARACTER_TYPE _charType)
    {
        if (_teamType == ENUM_TEAM_TYPE.Blue)
            return blueTeamStatusWindow.Connect_Character(_charType);
        else if (_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindow.Connect_Character(_charType);
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

    public void OnClick_Lobby() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비에 돌아가시겠습니까?");

    public void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);

    public override void PlaySFX_CallBack(int sfxNum) 
    {
        Managers.Sound.Play((ENUM_SFX_TYPE)sfxNum, ENUM_SOUND_TYPE.SFX);
    }
}