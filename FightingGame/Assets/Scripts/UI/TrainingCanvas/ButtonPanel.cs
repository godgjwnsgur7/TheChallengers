using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class ButtonPanel : UIElement
{
    public string userType;
    [SerializeField] Text panelOpenBtnText;
    [SerializeField] SettingPanel settingPanel;
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] TrainingScene trainingScene;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
        SetPanelOpenButtonText("닫기");
    }

    public override void Close()
    {
        base.Close();
        SetPanelOpenButtonText("설정");
    }

    public void OnClick_OpenSettingPanel()
    {
        inputKeyManagement.Init();
        this.Close();
        settingPanel.Open();
    }

    public void SetPanelOpenButtonText(string text)
    {
        panelOpenBtnText.text = text;
    }

    public void Set_UserType(string _userType)
    {
        userType = _userType;
        Managers.UI.popupCanvas.Open_CharSelectPopup(Onclick_CallCharacter);
    }

    public void Onclick_CallCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        switch (userType)
        {
            case "Player":
                trainingScene.SelectPlayerCharacter(_charType);
                break;
            case "Enemy":
                trainingScene.SelectEnemyCharacter(_charType);
                break;
        }
    }
}
