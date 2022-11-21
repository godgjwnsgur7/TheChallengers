using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class SettingPanel : UIElement
{
    public bool isHide = false;
    public bool isInit = false;

    private RectTransform thisRect = null;
    private Coroutine runningCoroutine = null;

    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;

    public override void Close()
    {
        Reset_SettingPanel();
        base.Close();
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public void Init()
    {
        if (isInit)
            return;

        this.thisRect = GetComponent<RectTransform>();

        isInit = true;
    }

    // 클릭 InputKey, Slider 세팅
    public void OnClick_SetSliderValue(KeySettingData _keySettingData)
    {
        // Slider 세팅
        sizeSlider.value = _keySettingData.size;
        opacitySlider.value = _keySettingData.opacity;
        Set_SizeSliderText($"{(int)sizeSlider.value}%");
        Set_OpacitySliderText($"{(int)opacitySlider.value}%");
    }

    // 세팅패널 실린더 초기화
    public void Reset_SettingPanel()
    {
        this.sizeSlider.value = 50f;
        this.opacitySlider.value = 100f;

        Set_SizeSliderText("50%");
        Set_OpacitySliderText("100%");
    }

    public void Set_SizeSliderText(string _text) => sizeText.text = _text;
    public void Set_OpacitySliderText(string _text) => opacityText.text = _text;

    #region 세팅패널 숨기기, 보이기
    public void Move_SettingPanel()
    {
        if (runningCoroutine != null)
            return;

        if (isHide)
            Show_SettingPanel();
        else
            Hide_SettingPanel();
    }

    public void Hide_SettingPanel()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        isHide = true;


        float showPos = Screen.height;
        Vector3 target = thisRect.position;
        target.y = showPos + thisRect.sizeDelta.y;

        runningCoroutine = StartCoroutine(MoveVec(target));
    }

    public void Show_SettingPanel()
    {
        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        isHide = false;

        float showPos = Screen.height;
        Vector3 target = thisRect.position;
        target.y = showPos;

        runningCoroutine = StartCoroutine(MoveVec(target));
    }

    IEnumerator MoveVec(Vector3 vec)
    {
        while (thisRect.position != vec)
        {
            thisRect.position = Vector3.MoveTowards(thisRect.position, vec, 30);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        runningCoroutine = null;
    }
    #endregion
}
