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
    private bool isSameBtn = false;

    [SerializeField] SettingPanel settingPanel;

    public InputPanel inputPanel = null;
    public InputKey currInputKey = null;
    public AreaPanel areaPanel = null;
    public AreaKey currAreaKey = null;

    private EventTrigger eventTrigger;
    private EventTrigger.Entry dragEntry = new EventTrigger.Entry
    {
        eventID = EventTriggerType.Drag,
    };

    private ENUM_CHARACTER_TYPE skillIconType = ENUM_CHARACTER_TYPE.Knight;

    public void Init()
    {
        dragEntry.callback.AddListener(OnDragListener);

        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);
            inputPanel.transform.SetAsFirstSibling();

            inputPanel.Set_InputSkillKeys(skillIconType);
        }

        if (areaPanel == null)
        {
            areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
            areaPanel.Init();
            areaPanel.transform.SetAsFirstSibling();
        }

        Set_OnDragCallBack();

        if (!settingPanel.gameObject.activeSelf)
            Open_SettingPanel();
    }

    public void OnChange_InputSkillIcon()
    {
        skillIconType += 1;

        if (skillIconType == ENUM_CHARACTER_TYPE.Max)
            skillIconType = ENUM_CHARACTER_TYPE.Knight;

        inputPanel.Set_InputSkillKeys(skillIconType);
    }

    public void Set_OnDragCallBack()
    {
        Set_DirectionOnDragTrigger();

        for (int i = 1; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            eventTrigger = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i).GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(dragEntry);
        }
    }

    private void Set_DirectionOnDragTrigger()
    {
        InputArrowKey inputArrowKey = inputPanel.Get_InputKey(ENUM_INPUTKEY_NAME.Direction).GetComponent<InputArrowKey>();

        if (inputArrowKey == null)
        {
            Debug.LogError("inputArrowKey is Null!!");
            return;
        }

        eventTrigger = inputArrowKey.transform.Find("LeftArrow").gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(dragEntry);
        eventTrigger = inputArrowKey.transform.Find("RightArrow").gameObject.AddComponent<EventTrigger>();
        eventTrigger.triggers.Add(dragEntry);
    }

    public void OnDragListener(BaseEventData _data) 
    {
        Vector2 movePos = Input.mousePosition;
        Set_InputKeyTransForm(movePos, (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum);
    }

    public void OnPoint_UpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        isMove = false;

        if(!isSameBtn)
            settingPanel.OnClick_SetSliderValue(currInputKey);

        isSameBtn = false;
    }

    public void OnPoint_DownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (isMove)
            return;

        isValueChange = true;
        isMove = true;

        if (currInputKey != null && currInputKey.inputKeyNum == (int)_inputKeyName)
            isSameBtn = true;

        if (!isSameBtn)
        {
            currInputKey = inputPanel.Get_InputKey(_inputKeyName);

            if (currAreaKey != null)
                currAreaKey.Set_isSelect(false);

            currAreaKey = areaPanel.Get_AreaKey(_inputKeyName);
            currAreaKey.Set_isSelect(true);
        }
    }

    /// <summary>
    /// InputKey 위치 변경
    /// </summary>
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

    /// <summary>
    /// InpuyKey 사이즈 변경
    /// </summary>
    public void Set_InputKeySize(float _sizeValue, ENUM_INPUTKEY_NAME _inputKeyName)
    {
        _sizeValue = (50 + _sizeValue) / settingPanel.Get_SizeMaxValue();
        Vector3 changeScale = new Vector3(1, 1, 1) * _sizeValue;

        currInputKey.rectTr.localScale = changeScale;
        currAreaKey.rectTr.localScale = changeScale;

        Set_InputKeyTransForm(currInputKey.transform.position, _inputKeyName);
    }

    /// <summary>
    /// InputKey 투명도 변경
    /// </summary>
    public void Set_InputKeyOpacity(float _opacityValue, ENUM_INPUTKEY_NAME _inputKeyName)
    {
        _opacityValue = 0.5f + _opacityValue / (settingPanel.Get_OpacityMaxValue() * 2);

        currInputKey.Set_Opacity(_opacityValue);
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
                inputKeys[i].rectTr.localScale.x, inputKeys[i].Get_Opacity(),
                inputKeys[i].rectTr.position.x, inputKeys[i].rectTr.position.y);
            
            keySettingDatas.Add(keySettingData);
        }

        PlayerPrefsManagement.Save_KeySettingData(keySettingDatas);

        return true;
    }

    /// <summary>
    /// 겹치는 UI가 있는지 확인 (겹칠 경우 true)
    /// </summary>
    public bool InputKey_OverlapCheckAll()
    {
        AreaKey[] areaKeys = areaPanel.Get_AreaKeys();

        for (int i = 0; i < areaKeys.Length; i++)
            if (areaKeys[i].Get_isOverlap())
                return true;

        return false;
    }

    public void Open_SettingPanel()
    {
        settingPanel.gameObject.SetActive(true);

        if(!settingPanel.isInit)
            settingPanel.Init();
    }

    /// <summary>
    /// 사이즈 실린더값 변경시 실행, InputKey 사이즈 변경
    /// </summary>
    public void OnValueChanged_SizeSlider(Slider _slider)
    {
        if (currInputKey == null)
            return;

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;

        settingPanel.Set_SizeSliderText($"{(int)_slider.value}%");
        Set_InputKeySize(_slider.value, inputKeyName);
    }

    /// <summary>
    /// 투명도 실린더값 변경시 실행, InputKey 투명도 변경
    /// </summary>
    public void OnValueChanged_OpacitySlider(Slider _slider)
    {
        if (currInputKey == null)
            return;

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;

        settingPanel.Set_OpacitySliderText($"{(int)_slider.value}%");
        Set_InputKeyOpacity(_slider.value, inputKeyName);
    }

    /// <summary>
    /// 버튼 설정 종료
    /// </summary>
    public void OnClick_CloseBtn()
    {
        if (isValueChange)
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "값을 저장하지않고 종료하시겠습니까?");
        else
            Managers.UI.popupCanvas.Open_SelectPopup(Close_SettingPanel, null, "버튼 설정을 종료하시겠습니까?");
    }

    public void Close_SettingPanel()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Close);
    }

    public void Close()
    {
        currInputKey = null;

        settingPanel.Reset_SettingPanel();
        settingPanel.gameObject.SetActive(false);

        Destroy(this.gameObject);
    }

    /// <summary>
    /// 버튼 설정 리셋, 저장
    /// </summary>
    public void OnClick_ResetBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Reset_InputKey, null, "버튼 설정을 초기화하시겠습니까?");
    public void OnClick_SaveBtn()
        => Managers.UI.popupCanvas.Open_SelectPopup(Save_InputKey, null, "버튼 설정을 저장하시겠습니까?");

    public void Reset_InputKey()
    {
        Managers.Resource.Destroy(inputPanel.gameObject);
        Managers.Resource.Destroy(areaPanel.gameObject);

        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Set_isReset(true);
        inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);
        inputPanel.transform.SetAsFirstSibling();

        Set_OnDragCallBack();

        areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
        areaPanel.Set_isReset(true);
        areaPanel.Init();
        areaPanel.transform.SetAsFirstSibling();

        settingPanel.Reset_SettingPanel();
    }

    public void Save_InputKey()
    {
        Save_InputKeyDatas();
    }

    /// <summary>
    /// InputKey 이동 중지
    /// </summary>
    public void OnPointerUp_MovePos()
    {
        isMove = false;

        if (!isValueChange)
            isValueChange = true;
    }

    /// <summary>
    /// InputKet 이동
    /// </summary>
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
