using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingWindow : MonoBehaviour
{
    [SerializeField] SoundSettingArea masterSound;
    [SerializeField] SoundSettingArea bgmSound;
    [SerializeField] SoundSettingArea sfxSound;
    [SerializeField] Image vibrationSettingImage;

    bool isVibration;

    public void Open()
    {
        // 셋팅된 사운드 정보를 가져와서 셋팅

        gameObject.SetActive(true);
    }

    public void Close()
    {

        gameObject.SetActive(false);
    }

    
}
