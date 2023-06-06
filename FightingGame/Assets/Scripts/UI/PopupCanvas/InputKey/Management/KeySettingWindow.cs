using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class KeySettingWindow : MonoBehaviour
{
    [SerializeField] Slider scaleSizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text scaleSizeText;
    [SerializeField] Text opacityText;

    [SerializeField] Text currentCharText;

    Action<float> onChangeSizeSliderCallBack;
    Action<float> onChageOpacitySliderCallBack;

    public void Init(Action<float> _onChangeSizeSliderCallBack, Action<float> _onChageOpacitySliderCallBack, float _opacity)
    {
        scaleSizeSlider.interactable = false;

        onChangeSizeSliderCallBack = _onChangeSizeSliderCallBack;
        onChageOpacitySliderCallBack = _onChageOpacitySliderCallBack;

        scaleSizeSlider.value = 100;
        opacitySlider.value = _opacity * 100;

        currentCharText.text = Managers.Data.Get_CharNameDict(ENUM_CHARACTER_TYPE.Knight);
    }

    public void Set_SizeSliderValue(float _value)
    {
        scaleSizeSlider.value = _value;
    }

    public void Set_OpacitySliderValue(float _value)
    {
        opacitySlider.value = _value;
    }

    public void OnValueChanged_SizeSlider()
    {
        onChangeSizeSliderCallBack?.Invoke(scaleSizeSlider.value);
        scaleSizeText.text = $"{(int)scaleSizeSlider.value}%";
    }

    public void OnValueChanged_OpacitySlider()
    {
        onChageOpacitySliderCallBack?.Invoke(opacitySlider.value);
        opacityText.text = $"{(int)opacitySlider.value}%";
    }

    public void ChangeCharacterText(ENUM_CHARACTER_TYPE _charType)
    {
        currentCharText.text = Managers.Data.Get_CharNameDict(_charType);
    }

    public void Set_SizeliderInteractable()
    {
        scaleSizeSlider.interactable = true;
    }
}
