using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingArea : MonoBehaviour
{
    [SerializeField] ENUM_SOUND_TYPE soundType;
    [SerializeField] Slider slider;
    [SerializeField] Image muteImage;

    public void OnValueChanged_UpdateVolumData()
    {

    }
}
