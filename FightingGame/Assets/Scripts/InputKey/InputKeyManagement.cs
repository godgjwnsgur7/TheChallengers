using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyManagement : UIElement
{
    List<KeySettingData> keySettingDataList = null;

    public bool isMove = false;
    public bool isValueChange = false;
    private int inputKeyNum;

    public SettingPanel settingPanel;
    public InputPanel inputPanel = null;
    private InputKey inputKey = null;

    public void Init()
    {
        if (this.inputPanel == null)
        {
            this.inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            this.inputPanel.Init(OnClick_CallBackDown, OnClick_CallBackUp);
            this.inputPanel.transform.SetAsFirstSibling();
        }

        Set_keySettingDataList();
        this.inputPanel.Set_InputKeyData(keySettingDataList);
        Set_PanelActive(true);

        if(!settingPanel.isInit)
            settingPanel.Init();
    }

    // keySettingDataList 값 호출
    private void Set_keySettingDataList()
    {
        // 설정된 PlayerPrefs 호출
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();

        // 없으면 초기값 생성
        if (keySettingDataList == null)
        {
            keySettingDataList = new List<KeySettingData>();
            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKey.inputKeyRectTr.position.x, inputKey.inputKeyRectTr.position.y));
                Debug.Log((ENUM_INPUTKEY_NAME)i + "초기화");
            }
            return;
        }
    }

    #region inputKey 조절
    public void Set_InputKey(int _inputkeyNum)
    {
        Set_InputKeySize(keySettingDataList[_inputkeyNum].size, _inputkeyNum);
        Set_InputKeyOpacity(keySettingDataList[_inputkeyNum].opacity, _inputkeyNum);
        Set_InputKeyTransForm(keySettingDataList[_inputkeyNum].rectTrX, keySettingDataList[_inputkeyNum].rectTrY, _inputkeyNum);
    }

    // size 조절
    public void Set_InputKeySize(float _size, int _inputkeyNum)
    {
        float sizeRatio = (50 + _size) / 100;
        Vector3 changeScale = new Vector3(1, 1, 1) * sizeRatio;

        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        inputKey.inputKeyRectTr.localScale = changeScale;
        
        keySettingDataList[_inputkeyNum].size = _size;
    }

    // opacity 조절
    public void Set_InputKeyOpacity(float _opacity, int _inputkeyNum)
    {
        float opacityRatio = 0.5f + (_opacity / 200);
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Image inputKeyImage = inputKey.Get_SlotImage();
        if (inputKeyImage != null)
            Set_ChangeColor(inputKeyImage, opacityRatio);

        inputKeyImage = inputKey.Get_IconImage();
        if (inputKeyImage != null)
            Set_ChangeColor(inputKeyImage, opacityRatio);

        keySettingDataList[_inputkeyNum].opacity = _opacity;
    }

    public void Set_ChangeColor(Image _inputKeyImage, float _opacityRatio)
    {
        Color changeColor = _inputKeyImage.color;
        changeColor.a = _opacityRatio;
        _inputKeyImage.color = changeColor;
    }

    // 위치 조절
    public void Set_InputKeyTransForm(float _rectTrX, float _rectTrY, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Vector2 movePos = CheckRange_Position(new Vector2(_rectTrX, _rectTrY));
        inputKey.inputKeyRectTr.position = movePos;

        keySettingDataList[_inputkeyNum].rectTrX = movePos.x;
        keySettingDataList[_inputkeyNum].rectTrY = movePos.y;
    }

    // UI 이동 범위 체크
    private Vector2 CheckRange_Position(Vector2 _changeVector)
    {
        float scaleSizeX = (inputKey.inputKeyRectTr.sizeDelta.x / 2) * inputKey.inputKeyRectTr.localScale.x;
        float scaleSizeY = (inputKey.inputKeyRectTr.sizeDelta.y / 2) * inputKey.inputKeyRectTr.localScale.y;

        float vecRangeX = Mathf.Clamp(_changeVector.x, 0 + scaleSizeX, inputPanel.thisRectTr.sizeDelta.x - scaleSizeX);
        float vecRangeY = Mathf.Clamp(_changeVector.y, 0 + scaleSizeY, (inputPanel.thisRectTr.sizeDelta.y * 0.75f) - scaleSizeY);

        _changeVector = new Vector2(vecRangeX, vecRangeY);

        return new Vector2(vecRangeX, vecRangeY);
    }

    // InputKey 초기화
    public void Reset_InputKeyValue()
    {
        Destroy(inputPanel.gameObject);
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(OnClick_CallBackDown, OnClick_CallBackUp);

        keySettingDataList = new List<KeySettingData>();
        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
            keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKey.inputKeyRectTr.position.x, inputKey.inputKeyRectTr.position.y));
        }

        inputKeyNum = -1;

        settingPanel.Reset_SettingPanel();
    }

    #endregion 

    // CallBackDown Event
    private void OnClick_CallBackDown(InputKey _inputKey)
    {
        // 선택한 InputKey에 해당하는 Enum번호 찾기
        inputKeyNum = (int)Enum.Parse(typeof(ENUM_INPUTKEY_NAME), _inputKey.name);
        
        // 기존 선택 상태이던 버튼 area 색상 변경
        if (inputKey != null)
        {
            inputKey.isSelect = false;
            inputKey.Set_AreaColor();
        }

        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)inputKeyNum);

        // InputKey를 SettingPanel 등록
        settingPanel.OnClick_SetSliderValue(keySettingDataList[inputKeyNum]);

        if (inputKey != null)
        {
            inputKey.isSelect = true;
            inputKey.Set_AreaColor();
        }

        isMove = true;
        StartCoroutine(MouseDrag(inputKeyNum));

        Debug.Log($"{_inputKey.name}세팅");
    }

    // CallBackUp Event
    private void OnClick_CallBackUp(InputKey _inputKey)
    {
        isMove = false;
    }

    // Panel들 Active상태 변환
    public void Set_PanelActive(bool _changeBool)
    {
        this.inputPanel.gameObject.SetActive(_changeBool);

        if (!_changeBool && inputKey != null)
        {
            inputKey.isSelect = false;
            inputKey.Set_AreaColor();
            return;
        }
    }

    public void Save_KeySettingData()
    {
        if (!Get_Updatable())
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("겹치는 영역이 있어 수정이 불가능합니다.");
            return;
        }

        PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
        isValueChange = false;
    }

    public bool Get_Updatable()
    {
        bool isUpdate = false;

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            isUpdate = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i).Get_Updatable();
            if (isUpdate)
                continue;
            else
                break;
        }

        return isUpdate;
    }

    public KeySettingData Get_KeySettingData(int _inputKeyNum)
    {
        return keySettingDataList[_inputKeyNum];
    }

    IEnumerator MouseDrag(int _inputKeyNum)
    {
        settingPanel.Hide_SettingPanel();

        while (isMove)
        {
            Vector2 mousePos = Input.mousePosition;
            Set_InputKeyTransForm(mousePos.x, mousePos.y, _inputKeyNum);

            yield return null;
        }

        settingPanel.Show_SettingPanel();
    }

    #region SettingPanel 관련 함수

    #region Slider값 OnValueChanged
    public void OnValueChanged_SetSizeSlider(Slider _slider)
    {
        if (inputKeyNum < 0)
            return;

        int sizeValue = (int)_slider.value;

        settingPanel.Set_SizeSliderText($"{sizeValue}%");
        Set_InputKeySize(sizeValue, inputKeyNum);

        if (!isValueChange)
            isValueChange = true;
    }

    // Opacity 값 변경
    public void OnValueChanged_SetOpacitySlider(Slider _slider)
    {
        if (inputKeyNum < 0)
            return;

        int opacityValue = (int)_slider.value;

        settingPanel.Set_OpacitySliderText($"{opacityValue}%");
        Set_InputKeyOpacity(opacityValue, inputKeyNum);

        if (!isValueChange)
            isValueChange = true;
    }
    #endregion

    #region Close, Reset, Save 버튼
    public void OnClick_CloseBtn()
    {
        if (isValueChange)
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "값을 저장하지않고 종료하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "버튼 설정을 종료하시겠습니까?");
    }
    public void Close_SettingPanel()
    {
        settingPanel.Close();
        Set_PanelActive(false);
        inputKeyNum = -1;
    }

    public void OnClick_ResetBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKeyValue, null, "버튼 설정을 초기화하시겠습니까?");
    public void OnClick_SaveBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Save_KeySettingData, null, "버튼 설정을 저장하시겠습니까?");
    #endregion

    #region InputKey 이동버튼
    // InputKey 이동 중지
    public void OnPointerUp_MovePos()
    {
        isMove = false;

        if (!isValueChange)
            isValueChange = true;
    }

    // InputKet 이동
    public void OnPointerDown_MovePosY(float _moveSpeed)
    {
        if (inputKeyNum < 0)
            return;

        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().position;
        isMove = true;
        StartCoroutine(MovePosYCoroutine(movePos, _moveSpeed));
    }

    public void OnPointerDown_MovePosX(float _moveSpeed)
    {
        if (inputKeyNum < 0)
            return;

        Vector2 movePos = this.inputKey.GetComponent<RectTransform>().position;
        isMove = true;
        StartCoroutine(MovePosXCoroutine(movePos, _moveSpeed));
    }

    IEnumerator MovePosYCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (isMove)
        {
            _movePos.y += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            Set_InputKeyTransForm(_movePos.x, _movePos.y, this.inputKeyNum);
            yield return null;
        }
    }

    IEnumerator MovePosXCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (isMove)
        {
            _movePos.x += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            Set_InputKeyTransForm(_movePos.x, _movePos.y, this.inputKeyNum);
            yield return null;
        }
    }
    #endregion

    #endregion
}
