using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : UIElement
{
    [SerializeField] SoundSettingWindow soundSettingWindow;
    [SerializeField] GameObject controlSettingWindow;
    [SerializeField] GameObject accountsInfoWindow;

    public void Open()
    {
        soundSettingWindow.Open();
        
        if (controlSettingWindow.activeSelf)
            controlSettingWindow.SetActive(false);
        if (accountsInfoWindow.activeSelf)
            accountsInfoWindow.SetActive(false);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (soundSettingWindow.gameObject.activeSelf)
            soundSettingWindow.Close();
        if (controlSettingWindow.activeSelf)
            controlSettingWindow.SetActive(false);
        if (accountsInfoWindow.activeSelf)
            accountsInfoWindow.SetActive(false);
    }

    public override void OnClick_Exit()
    {
        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Cancel);

        base.OnClick_Exit();

        Close();
    }

    public void OnClick_SoundSetting()
    {  
        if (soundSettingWindow.gameObject.activeSelf)
            return;

        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Cilck_Heavy2);

        if (controlSettingWindow.activeSelf)
            controlSettingWindow.SetActive(false);
        if (accountsInfoWindow.activeSelf)
            accountsInfoWindow.SetActive(false);
        soundSettingWindow.Open();
    }

    public void OnClick_ControlSetting()
    {
        if (controlSettingWindow.activeSelf)
            return;

        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Cilck_Heavy2);

        if (soundSettingWindow.gameObject.activeSelf)
            soundSettingWindow.Close();
        if(accountsInfoWindow.activeSelf)
            accountsInfoWindow.SetActive(false);
        controlSettingWindow.SetActive(true);
    }

    public void OnClick_AccountsInfo()
    {
        if (accountsInfoWindow.activeSelf)
            return;

        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Cilck_Heavy2);

        if (soundSettingWindow.gameObject.activeSelf)
            soundSettingWindow.Close();
        if (controlSettingWindow.activeSelf)
            controlSettingWindow.SetActive(false);
        accountsInfoWindow.SetActive(true);
    }

    public void OnClick_InputKeySetting()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);
        
        Managers.UI.popupCanvas.Open_SelectPopup(Open_InputKeyManagement, null,
            "키 조작 창을 여시겠습니까?");
    }

    private void Open_InputKeyManagement()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Call_InputKeyManagement);
    }

    private void Call_InputKeyManagement()
    {
        Close();

        Managers.Input.Get_InputKeyManagement().Open();
    }
}
