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
        if (trainingScene.isCallPlayer)
        {
            trainingScene.DeletePlayer();
            trainingScene.Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
        }

        if (trainingScene.isCallEnemy)
        {
            trainingScene.DeleteEnemy();
            trainingScene.Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
        }

        inputKeyManagement.Init();
        this.Close();
        settingPanel.Open();
    }

    public void SetPanelOpenButtonText(string text)
    {
        panelOpenBtnText.text = text;
    }

    public void OnClick_CallPlayer() => Managers.UI.popupCanvas.Open_CharSelectPopup(OnClick_SelectPlayerCharacter);
    public void OnClick_SelectPlayerCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        trainingScene.SelectPlayerCharacter(_charType);
        this.Close();
    }

    public void OnClick_CallEnemy() => Managers.UI.popupCanvas.Open_CharSelectPopup(OnClick_SelectEnemyCharacter);
    public void OnClick_SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        trainingScene.SelectEnemyCharacter(_charType);
        this.Close();
    }

    public void OnClick_DestroyPlayer()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(trainingScene.DeletePlayer, null, "소환된 캐릭터를 역소환하시겠습니까?");
        this.Close();
    }
    public void OnClick_DestroyEnemy()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(trainingScene.DeleteEnemy, null, "소환된 적를 역소환하시겠습니까?");
        this.Close();
    }
}
