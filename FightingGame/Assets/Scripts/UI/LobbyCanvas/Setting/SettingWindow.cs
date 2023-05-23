using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Setting Window
/// 팝업캔버스로 옮겨질 수도 있읍니다.!
/// </summary>
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

    public void Open_InputKeyManagement(Transform _transform)
    {
        InputKeyManagement go = Managers.Input.Get_InputKeyManagement();
        go.transform.parent = Managers.UI.currCanvas.transform;
        go.Init();
    }
}
