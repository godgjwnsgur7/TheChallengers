using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class WindowArea : MonoBehaviour
{
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider TransparencySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text TransparencyText;

    /// <summary>
    /// 선택한 인풋키의 size 값 slider에 적용
    /// </summary>
    public void OnClick_SetSliderValue(InputKey _inputKey)
    {
        Set_SIzeSlider(_inputKey.GetComponent<RectTransform>().localScale.x * 100f);
    }

    // 세팅패널 실린더 초기화
    public void Reset_WindowArea()
    {
        Set_SIzeSlider(100f);
        Set_TransparencySlider(100f);
    }

    /// <summary>
    /// 슬라이더 값 조절, 텍스트 내용 변경
    /// </summary>
    public void Set_SIzeSlider(float _value) {
        sizeSlider.value = _value;
        Set_SizeText($"{(int)_value}%");
    }

    public void Set_TransparencySlider(float _value) {
        TransparencySlider.value = _value;
        Set_TransparencyText($"{(int)_value}%");
    }

    /// <summary>
    /// 텍스트 내용 변경
    /// </summary>
    public void Set_SizeText(string _text) => sizeText.text = _text;
    public void Set_TransparencyText(string _text) => TransparencyText.text = _text;
}