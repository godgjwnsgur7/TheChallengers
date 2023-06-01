using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider TransparencySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text TransparencyText;

    // 클릭 InputKey, Slider 세팅
    public void OnClick_SetSliderValue(InputKey _inputKey)
    {
        Set_SIzeSlider(_inputKey.GetComponent<RectTransform>().localScale.x * Get_SizeMaxValue() - 50);
        //Set_TransparencySlider((_inputKey.Get_Opacity() - 0.3f) * (Get_TransparencyMaxValue() * 1.429f));
    }

    // 세팅패널 실린더 초기화
    public void Reset_SettingPanel()
    {
        Set_SIzeSlider(50f);
        Set_TransparencySlider(50f);
    }

    public void Set_SIzeSlider(float _value) {
        sizeSlider.value = _value;
        Set_SizeText($"{(int)_value}%");
    }

    public void Set_TransparencySlider(float _value) {
        TransparencySlider.value = _value;
        Set_TransparencyText($"{(int)_value}%");
    }

    public void Set_SizeText(string _text) => sizeText.text = _text;
    public void Set_TransparencyText(string _text) => TransparencyText.text = _text;

    public float Get_SizeMaxValue() => sizeSlider.maxValue;
    public float Get_TransparencyMaxValue() => TransparencySlider.maxValue;
}