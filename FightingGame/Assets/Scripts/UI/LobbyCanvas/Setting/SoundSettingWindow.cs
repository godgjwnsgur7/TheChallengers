using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundSettingWindow : MonoBehaviour
{
    [SerializeField] SoundSettingArea masterSound;
    [SerializeField] SoundSettingArea bgmSound;
    [SerializeField] SoundSettingArea sfxSound;
    [SerializeField] Image vibrationSettingImage;

    bool isVibration = false;

    public void Open()
    {
        VolumeData volumeData = Managers.Sound.Get_CurrVolumeData();

        bool isMasterMute = volumeData.isBgmMute && volumeData.isSfxMute;
        masterSound.Init(Update_VolumeData, Update_SoundMuteData, volumeData.masterVolume, isMasterMute);
        bgmSound.Init(Update_VolumeData, Update_SoundMuteData, volumeData.bgmVolume, volumeData.isBgmMute);
        sfxSound.Init(Update_VolumeData, Update_SoundMuteData, volumeData.sfxVolume, volumeData.isSfxMute);
        Set_IsVibrationBtn(volumeData.isVibration);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        Managers.Sound.Save_CurrVolumeData();

        gameObject.SetActive(false);
    }
    
    public void Update_VolumeData(ENUM_SOUND_TYPE _soundType, float _volumeValue)
    {
        switch (_soundType)
        {
            case ENUM_SOUND_TYPE.MASTER:
                Managers.Sound.Update_MasterVolumeData(_volumeValue);
                break;
            case ENUM_SOUND_TYPE.BGM:
                Managers.Sound.Update_BGMVolumeData(_volumeValue);
                break;
            case ENUM_SOUND_TYPE.SFX:
                Managers.Sound.Update_SFXVolumeData(_volumeValue);
                break;
        }
    }
    
    public void Update_SoundMuteData(ENUM_SOUND_TYPE _soundType, bool _isMute)
    {
        Managers.Sound.Update_SoundMuteData(_soundType, _isMute);

        if(_soundType == ENUM_SOUND_TYPE.MASTER)
        {
            bgmSound.Change_MuteState(_isMute);
            sfxSound.Change_MuteState(_isMute);
        }
        else if((_isMute == false) && masterSound.Get_MuteState())
        {
            masterSound.Change_MuteState(false);
        }
    }

    public void OnClick_Vibration()
    {
        Set_IsVibrationBtn(!isVibration);
    }

    public void Set_IsVibrationBtn(bool _isVibration)
    {
        if (isVibration == _isVibration)
            return;

        Managers.Sound.Set_Vibration(_isVibration);
        isVibration = _isVibration;

        String str = isVibration ? "Vibration_Button_On" : "Vibration_Button_Off";
        vibrationSettingImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/{str}");
    }
}
