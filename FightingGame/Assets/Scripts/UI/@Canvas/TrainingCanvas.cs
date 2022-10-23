using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class TrainingCanvas : BaseCanvas
{
    [Header("Set In Editor")]
    [SerializeField] StatusWindowUI buleTeamStatusWindowUI;
    [SerializeField] StatusWindowUI redTeamStatusWindowUI;
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] InputKeyController inputKeyController;
    [SerializeField] ButtonPanel buttonPanel;


    public override void Init()
    {
        base.Init();
        inputKeyManagement.Init();
        inputKeyController.Init();
    }


    public void OnClick_OnOffButtonPanel()
    {
        if (buttonPanel.gameObject.activeSelf)
            buttonPanel.Close();
        else
            buttonPanel.Open();
    }
}