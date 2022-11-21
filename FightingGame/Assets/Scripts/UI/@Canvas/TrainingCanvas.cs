using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class TrainingCanvas : BaseCanvas
{
    public BaseMap map;
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI blueTeamStatusWindowUI;
    [SerializeField] StatusWindowUI redTeamStatusWindowUI;
    [SerializeField] ButtonPanel buttonPanel;
    private InputKeyManagement inputKeyManagement;
    private InputKeyController inputKeyController;

    public override void Init()
    {
        base.Init();

        string mapName = Enum.GetName(typeof(ENUM_MAP_TYPE), 0);
        map = Managers.Resource.Instantiate($"Maps/{mapName}").GetComponent<BaseMap>();

        inputKeyManagement = Managers.Input.Get_InputKeyManagement();
        inputKeyController = Managers.Input.Get_InputKeyController();

        buttonPanel.Set_Map(map);
        buttonPanel.Init();
    }

    public void OnClick_OnOffButtonPanel()
    {
        if (inputKeyManagement.settingPanel.isOpen)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼설정 중에 누를 수 없습니다.");
            return;
        }

        if (buttonPanel.gameObject.activeSelf)
            buttonPanel.Close();
        else
            buttonPanel.Open();
    }

    public StatusWindowUI Get_StatusWindowUI(ENUM_TEAM_TYPE _teamType)
    {
        if (_teamType == ENUM_TEAM_TYPE.Blue)
            return blueTeamStatusWindowUI;
        else if (_teamType == ENUM_TEAM_TYPE.Red)
            return redTeamStatusWindowUI;
        else
        {
            Debug.Log($"_teamType 오류 : {_teamType}");
            return null;
        }
    }

    public void OnClick_Lobby() => Managers.UI.popupCanvas.Open_SelectPopup
        (GoTo_LobbyScene, null, "로비에 돌아가시겠습니까?");

    public void GoTo_LobbyScene() => Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);

    public override T GetUIComponent<T>()
    {
        if (typeof(T) == typeof(InputKeyManagement))
            return inputKeyManagement.GetComponent<T>();
        else if (typeof(T) == typeof(InputKeyController))
            return inputKeyController.GetComponent<T>();
        else
        {
            Debug.Log("범위 벗어남");
            return default;
        }
    }
}