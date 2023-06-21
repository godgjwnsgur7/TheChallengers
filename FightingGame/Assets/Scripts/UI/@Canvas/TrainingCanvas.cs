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

    [SerializeField] TrainingMenuWindowUI trainingMenuWindow;

    public override void Init()
    {
        base.Init();

        trainingMenuWindow.Init();
    }

    public void Clear_Status()
    {
        blueTeamStatusWindow.Clear();
        redTeamStatusWindow.Clear();
    }

    public Action<float> Get_StatusWindowCallBack(ENUM_TEAM_TYPE _teamType, ENUM_CHARACTER_TYPE _charType)
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
}