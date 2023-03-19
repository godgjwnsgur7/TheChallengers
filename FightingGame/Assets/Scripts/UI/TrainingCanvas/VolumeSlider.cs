using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Text bgmSliderText;
    [SerializeField] Text sfxSliderText;

    bool isChange = false;
    bool isInit = false;

    List<VolumeData> volumeDataList = new List<VolumeData>();

    private void OnEnable()
    {
        Init();

        // volumeDataList = Managers.Sound.Get_VolumeDatas(); // 저장된 사운드 크기 값 가져오기 -> DataMgr

        Changed_Volume();
    }

    private void OnDisable()
    {
        if (isChange)
            Managers.UI.popupCanvas.Open_SelectPopup(SaveVolume, Changed_Volume, "변경된 소리를 저장하시겠습니까?");
    }

    public void Init()
    {
        if (isInit)
            return;

        VolumeData volumeData = PlayerPrefsManagement.Load_VolumeData();
        bgmSlider.value = volumeData.bgmVolume;
        sfxSlider.value = volumeData.sfxVolume;

        bgmSlider.onValueChanged.AddListener(Managers.Sound.Update_BGMVolumeData);
        bgmSlider.onValueChanged.AddListener(Update_BgmSliderText);

        sfxSlider.onValueChanged.AddListener(Managers.Sound.Update_SFXVolumeData);
        sfxSlider.onValueChanged.AddListener(Update_SfxSliderText);

        isInit = true;
    }

    public void Changed_Volume()
    {
       // bgmSlider.value = volumeDataList[0].volume;
       // sfxSlider.value = volumeDataList[1].volume;

        Update_BgmSliderText(bgmSlider.value);
        Update_SfxSliderText(sfxSlider.value);

        Set_isChange(false);
    }

    public void Set_isChange(bool _isChange) => isChange = _isChange;
    public void OnClick_SaveVolumeBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(SaveVolume, null, "사운드 값을 저장하시겠습니까?");

    public void SaveVolume()
    {
        // Managers.Sound.Save_SoundData();
        Set_isChange(false);
    }

    public void Update_BgmSliderText(float _value) => bgmSliderText.text = $"BGM Volume : {(int)(_value * 100)}%";
    public void Update_SfxSliderText(float _value) => sfxSliderText.text = $"SFX Volume : {(int)(_value * 100)}%";
}

