using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;
using UnityEngine.EventSystems;

public class InputKeyManagement : MonoBehaviour
{
    public bool isMove = false;
    public bool isValueChange = false;

    [SerializeField] SettingPanel settingPanel;
    public InputPanel inputPanel = null;
    public InputKey currInputKey = null;
    public AreaPanel areaPanel = null;
    public AreaKey currAreaKey = null;

    public void Init()
    {
        //PlayerPrefs.DeleteAll();
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack, OnDragCallBack);
            inputPanel.transform.SetAsFirstSibling();
        }

        if (areaPanel == null)
        {
            areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
            areaPanel.Init();
            areaPanel.transform.SetAsFirstSibling();
        }

        if (!settingPanel.gameObject.activeSelf)
            Open_SettingPanel();
    }

    public void OnDragCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        Vector2 mousePos = Input.mousePosition;
        Set_InputKeyTransForm(mousePos, _inputKeyName);
    }

    public void OnPoint_UpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        // 담겨있는 코루틴 변수에 스탑코루틴 하고 후처리할거 하셈
        isMove = false;
        //currCoroutine = null;

        settingPanel.Show_SettingPanel();
        // 후처리 내용 - 겹치는 오브젝트 확인?

    }

    public void OnPoint_DownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (isMove)
            return;

        isValueChange = true;
        isMove = true;

        bool isSame = false;

        if (currInputKey != null && currInputKey.inputKeyNum == (int)_inputKeyName)
            isSame = true;

        currInputKey = inputPanel.Get_InputKey(_inputKeyName);
        currAreaKey = areaPanel.Get_AreaKey(_inputKeyName);

        if(!isSame)
            settingPanel.OnClick_SetSliderValue(_inputKeyName);
    }

    public void Set_InputKeyTransForm(Vector2 _movePos, ENUM_INPUTKEY_NAME _inputkeyNum)
    {
        RectTransform panelRectTr = inputPanel.GetComponent<RectTransform>();

        float scaleSizeX = (currInputKey.rectTr.sizeDelta.x / 2) * currInputKey.rectTr.localScale.x;
        float scaleSizeY = (currInputKey.rectTr.sizeDelta.y / 2) * currInputKey.rectTr.localScale.y;

        float vecRangeX = Mathf.Clamp(_movePos.x, 0 + scaleSizeX, panelRectTr.sizeDelta.x - scaleSizeX);
        float vecRangeY = Mathf.Clamp(_movePos.y, 0 + scaleSizeY, (panelRectTr.sizeDelta.y * 0.75f) - scaleSizeY);

        currInputKey.transform.position = new Vector2(vecRangeX, vecRangeY);
        currAreaKey.transform.position = new Vector2(vecRangeX, vecRangeY);
    }

    public void Set_InputKeySize(float _sizeValue, ENUM_INPUTKEY_NAME _inputKeyName)
    {
        _sizeValue = (50 + _sizeValue) / settingPanel.Get_SizeMaxValue();
        Vector3 changeScale = new Vector3(1, 1, 1) * _sizeValue;

        currInputKey.rectTr.localScale = changeScale;
        currAreaKey.rectTr.localScale = changeScale;
    }

    public void Set_InputKeyOpacity(float _opacityValue, ENUM_INPUTKEY_NAME _inputKeyName)
    {
        _opacityValue = 0.5f + _opacityValue / (settingPanel.Get_OpacityMaxValue() * 2);

        Image inputKeyImage = currInputKey.slotImage;
        if (inputKeyImage != null)
            Set_ChangeColor(inputKeyImage, _opacityValue);

        inputKeyImage = currInputKey.iconImage;
        if (inputKeyImage != null)
            Set_ChangeColor(inputKeyImage, _opacityValue);
    }

    public void Set_ChangeColor(Image _inputKeyImage, float _opacityValue)
    {
        Color changeColor = _inputKeyImage.color;
        changeColor.a = _opacityValue;
        _inputKeyImage.color = changeColor;
    }

    /// <summary>
    /// 저장 성공 시 true, 실패 시 false
    /// </summary>
    public bool Save_InputKeyDatas()
    {
        InputKey[] inputKeys = inputPanel.Get_InputKeys();

        if(InputKey_OverlapCheckAll())
        {
            // 겹치는 영역이 있는 것
            Managers.UI.popupCanvas.Open_NotifyPopup("겹치는 UI가 있어 저장할 수 없습니다.");
            return false;
        }

        List<KeySettingData> keySettingDatas = new List<KeySettingData>();

        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            KeySettingData keySettingData = new KeySettingData(i,
                inputKeys[i].rectTr.localScale.x, inputKeys[i].slotImage.color.a,
                inputKeys[i].rectTr.position.x, inputKeys[i].rectTr.position.y);
            
            keySettingDatas.Add(keySettingData);
        }

        PlayerPrefsManagement.Save_KeySettingData(keySettingDatas);

        return true;
    }

    public bool InputKey_OverlapCheck()
    {

        // 코루틴 안에 들어갈 아이로 현재 제어중인 currInputKey가 겹치는지 확인한다.

        return false; // 임시로 해놈
    }

    /// <summary>
    /// 겹치는 UI가 있는지 확인 (겹칠 경우 true)
    /// </summary>
    public bool InputKey_OverlapCheckAll()
    {
        InputKey[] inputKeys = inputPanel.Get_InputKeys();

        List<InputKey> inputKeysList = new List<InputKey>();

        // 각 인풋키의 끝 좌표를 받아오는데, 아마 크기로만 받아오면 안될거임
        // 사이즈도 감안해서 수식세우고 꼭짓점 4곳에 레이캐스트를 쏴서 확인.
        // 만약 겹치는 아이가 있다면 해당하는 이름을 확인하여 자신만 컬러변경
        // 겹치는 아이가 없다면 영역표시 끄고


        return false; // 그냥 임시로 해놈 수정바람
    }

    public void Open_SettingPanel()
    {
        settingPanel.gameObject.SetActive(true);

        if(!settingPanel.isInit)
            settingPanel.Init();
    }

    public void OnValueChanged_SizeSlider(Slider _slider)
    {
        if (currInputKey == null)
            return;

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;

        settingPanel.Set_SizeSliderText($"{(int)_slider.value}%");
        Set_InputKeySize(_slider.value, inputKeyName);
    }

    public void OnValueChanged_OpacitySlider(Slider _slider)
    {
        if (currInputKey == null)
            return;

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;

        settingPanel.Set_OpacitySliderText($"{(int)_slider.value}%");
        Set_InputKeyOpacity(_slider.value, inputKeyName);
    }

    public void OnClick_CloseBtn()
    {
        if (isValueChange)
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "값을 저장하지않고 종료하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "버튼 설정을 종료하시겠습니까?");
    }

    public void Close_SettingPanel()
    {
        currInputKey = null;

        settingPanel.Reset_SettingPanel();
        settingPanel.gameObject.SetActive(false);

        Destroy(inputPanel.gameObject);
        Destroy(areaPanel.gameObject);
    }

    public void OnClick_ResetBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKey, null, "버튼 설정을 초기화하시겠습니까?");
    public void OnClick_SaveBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Save_InputKey, null, "버튼 설정을 저장하시겠습니까?");

    public void Reset_InputKey()
    {
        Destroy(inputPanel.gameObject);
        Destroy(areaPanel.gameObject);

        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack, OnDragCallBack);
        inputPanel.transform.SetAsFirstSibling();

        areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
        areaPanel.Init();
        areaPanel.transform.SetAsFirstSibling();
    }

    public void Save_InputKey()
    {
        Save_InputKeyDatas();
    }

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
        if (currInputKey == null)
            return;

        Vector2 movePos = currInputKey.GetComponent<RectTransform>().position;
        isMove = true;
        StartCoroutine(MovePosYCoroutine(movePos, _moveSpeed));
    }

    public void OnPointerDown_MovePosX(float _moveSpeed)
    {
        if (currInputKey == null)
            return;

        Vector2 movePos = currInputKey.GetComponent<RectTransform>().position;
        isMove = true;
        StartCoroutine(MovePosXCoroutine(movePos, _moveSpeed));
    }

    IEnumerator MovePosYCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (isMove)
        {
            _movePos.y += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            Set_InputKeyTransForm(_movePos, (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum);
            yield return null;
        }
    }

    IEnumerator MovePosXCoroutine(Vector2 _movePos, float _moveSpeed)
    {
        while (isMove)
        {
            _movePos.x += _moveSpeed;
            _moveSpeed *= (1 + Time.deltaTime);

            Set_InputKeyTransForm(_movePos, (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum);
            yield return null;
        }
    }
}
