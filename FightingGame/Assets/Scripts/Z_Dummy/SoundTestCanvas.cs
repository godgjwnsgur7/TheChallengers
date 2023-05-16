using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SoundTestCanvas : BaseCanvas
{
    [SerializeField] SettingWindow settingWindow;

    [SerializeField] ENUM_BGM_TYPE BGM_Type = ENUM_BGM_TYPE.CaveMap;
    [SerializeField] ENUM_SFX_TYPE SFX_Type = ENUM_SFX_TYPE.Test1;

    private void Start()
    {
        Managers.Sound.Init();
    }

    public void OnClick_StartBGM()
    {
        Managers.Sound.Play_BGM(BGM_Type);
    }
    
    public void OnClick_StopBGM()
    {
        Managers.Sound.Stop_BGM();
    }

    public void OnClick_StartSFX()
    {
        Managers.Sound.Play_SFX(SFX_Type);
    }

    public void OnClick_SettingWindow()
    {
        settingWindow.Open();
    }
}
