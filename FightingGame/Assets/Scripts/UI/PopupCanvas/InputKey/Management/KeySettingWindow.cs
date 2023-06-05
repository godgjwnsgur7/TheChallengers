using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class KeySettingWindow : MonoBehaviour
{
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;

    [SerializeField] Text currentCharText;

    Action<float> onChangeSizeSliderCallBack;
    Action<float> onChageOpacitySliderCallBack;

    public void Init(Action<float> _onChangeSizeSliderCallBack, Action<float> _onChageOpacitySliderCallBack, float _opacity)
    {
        sizeSlider.interactable = false;

        onChangeSizeSliderCallBack = _onChangeSizeSliderCallBack;
        onChageOpacitySliderCallBack = _onChageOpacitySliderCallBack;

        opacitySlider.value = _opacity;
    }

    public void OnValueChanged_SizeSlider(Slider _slider)
    {
        float value = _slider.value;

        onChangeSizeSliderCallBack?.Invoke(value);
        sizeText.text = $"{(int)value}%";
    }

    public void OnValueChanged_TransparencySlider(Slider _slider)
    {
        float value = _slider.value;

        onChageOpacitySliderCallBack?.Invoke(value);
        opacityText.text = $"{(int)value}%";
    }

    public void ChangeCharacterText(ENUM_CHARACTER_TYPE _charType)
    {
        currentCharText.text = Managers.Data.Get_CharNameDict(_charType);
    }

    public void Set_SizeliderInteractable()
    {
        sizeSlider.interactable = true;
    }
}
