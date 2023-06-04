using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    [SerializeField] SoundSettingWindow soundSettingWindow;
    [SerializeField] GameObject controlSettingWindow;
    [SerializeField] GameObject accountsInfoWindow;

    public void Open()
    {
        OnClick_SoundSetting();

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

    public void OnClick_SoundSetting()
    {
        if (soundSettingWindow.gameObject.activeSelf)
            return;

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

        if (soundSettingWindow.gameObject.activeSelf)
            soundSettingWindow.Close();
        if (controlSettingWindow.activeSelf)
            controlSettingWindow.SetActive(false);
        accountsInfoWindow.SetActive(true);
    }

    public void OnClick_InputKeySetting()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(Open_InputKeyManagement, null,
            "키 조작 창을 여시겠습니까?");
    }

    private void Open_InputKeyManagement()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Call_InputKeyManagement);
    }

    private void Call_InputKeyManagement()
    {
        Managers.Input.Get_InputKeyManagement().Open();

        Close();
    }
}
