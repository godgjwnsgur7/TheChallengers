using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundSettingArea : MonoBehaviour
{
    [SerializeField] ENUM_SOUND_TYPE soundType;
    [SerializeField] Slider slider;
    [SerializeField] Image muteImage;

    public bool isMute = false;
    bool isMuteLock = false;

    Action<ENUM_SOUND_TYPE, float> updataVolumeDataCallBack;
    Action<ENUM_SOUND_TYPE, bool> updateMuteStateCallBack;

    Coroutine muteLockCoroutine;

    private void OnDisable()
    {
        if (muteLockCoroutine != null)
            StopCoroutine(muteLockCoroutine);
    }

    public void Init(Action<ENUM_SOUND_TYPE, float> _updataVolumeDataCallBack
        , Action<ENUM_SOUND_TYPE, bool> _updateMuteStateCallBack, float _volumeValue, bool _isMute)
    {
        slider.value = _volumeValue;

        updataVolumeDataCallBack = _updataVolumeDataCallBack;
        updateMuteStateCallBack = _updateMuteStateCallBack;

        Change_MuteState(_isMute);
    }

    public void Change_MuteState(bool _isMute)
    {
        if (isMute == _isMute)
            return;

        isMute = _isMute;

        String str = isMute ? "Mute_Button_On" : "Mute_Button_Off";
        muteImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/{str}");
    }

    public bool Get_MuteState() => isMute;

    public void OnClick_Mute()
    {
        if (isMuteLock)
            return;

        muteLockCoroutine = StartCoroutine(IMuteButtonLock(0.5f));

        isMute = !isMute;

        String str = isMute ? "Mute_Button_On" : "Mute_Button_Off";
        muteImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/{str}");

        updateMuteStateCallBack?.Invoke(soundType, isMute);
    }

    public void OnValueChanged_UpdateVolumeData()
    {
        if (updataVolumeDataCallBack == null)
            return;

        if (isMute)
        {
            isMuteLock = false;
            OnClick_Mute();
        }

        updataVolumeDataCallBack?.Invoke(soundType, slider.value);
    }

    protected IEnumerator IMuteButtonLock(float _time)
    {
        isMuteLock = true;
        yield return new WaitForSeconds(_time);
        isMuteLock = false;
    }
}
