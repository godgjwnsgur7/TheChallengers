using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider BgmSlider;
    [SerializeField] Slider SfxSlider;

    bool isChange = false;
    bool isInit = false;

    List<VolumeData> volumeDataList = new List<VolumeData>();

    private void OnEnable()
    {
        Init();

        volumeDataList = Managers.Sound.Get_VolumeDatas();

        Changed_Volume();
    }

    private void OnDisable()
    {
        if(isChange)
            Managers.UI.popupCanvas.Open_SelectPopup(SaveVolume, Changed_Volume, "변경된 소리를 저장하시겠습니까?");
    }

    public void Init()
    {
        if (isInit)
            return;

        BgmSlider.onValueChanged.AddListener(Managers.Sound.OnValueChanged_BGMVolume);
        SfxSlider.onValueChanged.AddListener(Managers.Sound.OnValueChanged_SFXVolume);

        isInit = true;
    }

    public void Changed_Volume()
    {
        BgmSlider.value = volumeDataList[0].volume;
        SfxSlider.value = volumeDataList[1].volume;

        Set_isChange(false);
    }

    public void Set_isChange(bool _isChange) => isChange = _isChange;
    public void OnClick_SaveVolumeBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(SaveVolume, null, "사운드 값을 저장하시겠습니까?");

    public void SaveVolume()
    {
        Managers.Sound.Save_SoundData();
        Set_isChange(false);
    }
}
