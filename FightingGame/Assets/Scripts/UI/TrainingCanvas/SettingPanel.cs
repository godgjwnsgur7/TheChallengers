using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class SettingPanel : UIElement
{
    public bool isValueChange = false;
    public bool isHide = false;
    private int inputKeyNum = -1;

    private InputKey inputKey = null;
    private RectTransform thisRect = null;
    private Coroutine runningCoroutine = null;

    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider opacitySlider;
    [SerializeField] Text sizeText;
    [SerializeField] Text opacityText;

    public override void Close()
    {
        inputKeyManagement.Set_PanelActive(false);
        Reset_SettingPanel();
        base.Close();
    }

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public void Init()
    {
        this.thisRect = GetComponent<RectTransform>();
    }

    // 클릭 InputKey, Slider 세팅
    public void OnClick_SetInputKey(InputKey _inputKey, KeySettingData _keySettingData)
    {
        // InputKey세팅
        this.inputKey = _inputKey;
        inputKeyNum = (int)Enum.Parse(typeof(ENUM_INPUTKEY_NAME), _inputKey.name);

        // Slider 세팅
        sizeSlider.value = _keySettingData.size;
        opacitySlider.value = _keySettingData.opacity;
        Set_SizeSliderText($"{(int)sizeSlider.value}%");
        Set_OpacitySliderText($"{(int)opacitySlider.value}%");
    }

    private void Set_SizeSliderText(string _text) => sizeText.text = _text;
    private void Set_OpacitySliderText(string _text) => opacityText.text = _text;

    #region 실린더값 변경
    // SizeSlider 값 변경
    public void OnValueChanged_SetSizeSlider()
    {
        if (inputKey == null)
            return;

        int sizeValue = (int)this.sizeSlider.value;

        Set_SizeSliderText($"{sizeValue}%");
        this.inputKeyManagement.Set_InputKeySize(sizeValue, this.inputKeyNum);

        if (!isValueChange)
            isValueChange = true;
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider()
    {
        if (inputKey == null)
            return;

        int opacityValue = (int)this.opacitySlider.value;

        Set_OpacitySliderText($"{opacityValue}%");
        this.inputKeyManagement.Set_InputKeyOpacity(opacityValue, this.inputKeyNum);

        if (!isValueChange)
            isValueChange = true;
    }
    #endregion

    #region InputKey 이동버튼
    // InputKey 이동 중지
    public void OnPointerUp_MovePos()
    {
        inputKeyManagement.isMove = false;

        if (!isValueChange)
            isValueChange = true;
    }

    // InputKet 이동
    public void OnPointerDown_MovePosY(float _moveSpeed)
    {
        if (this.inputKey == null)
            return;

        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().position;
        inputKeyManagement.isMove = true;
        StartCoroutine(MovePosYCoroutine(movePos, _moveSpeed));
    }

    public void OnPointerDown_MovePosX(float _moveSpeed)
    {
        if (this.inputKey == null)
            return;

        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().position;
        inputKeyManagement.isMove = true;
        StartCoroutine(MovePosXCoroutine(movePos, _moveSpeed));
    }

    IEnumerator MovePosYCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (inputKeyManagement.isMove)
        {
            _movePos.y += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            this.inputKeyManagement.Set_InputKeyTransForm(_movePos.x, _movePos.y, this.inputKeyNum);
            yield return null;
        }
    }

    IEnumerator MovePosXCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (inputKeyManagement.isMove)
        {
            _movePos.x += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            this.inputKeyManagement.Set_InputKeyTransForm(_movePos.x, _movePos.y, this.inputKeyNum);
            yield return null;
        }
    }
    #endregion

    #region Close, Reset, Save 버튼
    public void OnClick_CloseBtn()
    {
        if(isValueChange)
            Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "값을 저장하지않고 종료하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_SelectPopup(Close, null, "버튼 설정을 종료하시겠습니까?");
    }
    public void OnClick_ResetBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKeyValue, null, "버튼 설정을 초기화하시겠습니까?");
    public void OnClick_SaveBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(inputKeyManagement.Save_KeySettingData, null, "버튼 설정을 저장하시겠습니까?");
    #endregion

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

    public void Reset_InputKeyValue()
    {
        inputKeyManagement.Reset_InputKeyValue();

        if (this.inputKeyNum < 0)
            return;

        sizeSlider.value = inputKeyManagement.Get_KeySettingData(this.inputKeyNum).size;
        opacitySlider.value = inputKeyManagement.Get_KeySettingData(this.inputKeyNum).opacity;
    }

    // 세팅패널 초기화
    public void Reset_SettingPanel()
    {
        this.inputKeyNum = -1;
        this.inputKey = null;

        this.sizeSlider.value = 50;
        this.opacitySlider.value = 100;

        Set_SizeSliderText("50%");
        Set_OpacitySliderText("100%");
    }
}
