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

    public void Init()
    {
        dragEntry.callback.AddListener(OnDragListener);

        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);

            inputPanel.Set_InputSkillKeys(ENUM_CHARACTER_TYPE.Knight);
        }

        if (areaPanel == null)
        {
            areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
            areaPanel.Init();
        }

        Set_OnDragCallBack();
    }

    /// <summary>
    /// 인풋키 아이콘 변경하는 함수
    /// </summary>
    public void OnClick_InputSkillIcon(int charType)
    {
        if (charType <= (int)ENUM_CHARACTER_TYPE.Default && charType >= (int)ENUM_CHARACTER_TYPE.Max)
        {
            Debug.Log($"스킬 아이콘 캐릭터 타입 범위 초과 : {(ENUM_CHARACTER_TYPE)charType}");
            return;
        }

        inputPanel.Set_InputSkillKeys((ENUM_CHARACTER_TYPE)charType);
    }

    /// <summary>
    /// 인풋키에 Drag EventTrigger 삽입하는 함수
    /// </summary>
    public void Set_OnDragCallBack()
    {
        Set_DirectionOnDragTrigger();

        for (int i = 1; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            eventTrigger = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i).GetComponent<EventTrigger>();
            eventTrigger.triggers.Add(dragEntry);
        }
    }

    /// <summary>
    /// 방향 인풋키 Drag EventTrigger 함수
    /// </summary>
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

    /// <summary>
    /// Drag EventTrigger 함수 : 드래그 시 마우스 포인터 위치로 인풋키 이동
    /// </summary>
    public void OnDragListener(BaseEventData _data) 
    {
        Vector2 movePos = Input.mousePosition;
        Set_InputKeyTransForm(movePos, (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum);
    }

    /// <summary>
    /// 인풋키 PointUp 함수 Down 이벤트 때 작동한 값들을 유지 혹은 초기화
    /// </summary>
    public void OnPoint_UpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        isMove = false;

        if(!isSameBtn)
            settingPanel.OnClick_SetSliderValue(currInputKey);

        foreach (InputKey key in inputPanel.Get_InputKeys()) {
            //key.Set_Opacity(0.4f);
        }

        isSameBtn = false;
    }

    /// <summary>
    /// 인풋키 PointDown 함수 : 클릭한 인풋키를 Current 변수에 담는다.
    /// </summary>
    public void OnPoint_DownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (isMove)
            return;

        isValueChange = true;
        isMove = true;

        if (currInputKey != null && currInputKey.inputKeyNum == (int)_inputKeyName)
            isSameBtn = true;

        if (!isSameBtn)
            Set_CurrInputKey((int)_inputKeyName);
    }

    /// <summary>
    /// 현재 선택한 인풋키와 영역을 저장한다.
    /// </summary>
    public void Set_CurrInputKey(int inputKeyNum)
    {
        currInputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)inputKeyNum);
        currAreaKey = areaPanel.Get_AreaKey((ENUM_INPUTKEY_NAME)inputKeyNum);
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
        if (currInputKey == null)
            return;

        // 실린더 범위 50 ~ 150, 기본 값 0 + 수식값 0.5 ~ 1.5 = 사이즈 범위 0.5배 ~ 1.5배
        _sizeValue = _sizeValue / 100f;
        Vector3 changeScale = new Vector3(1, 1, 1) * _sizeValue;

        currInputKey.rectTr.localScale = changeScale;
        currAreaKey.rectTr.localScale = changeScale;

        settingPanel.Set_SizeText($"{(int)(_sizeValue * 100)}%");

        Set_InputKeyTransForm(currInputKey.transform.position, _inputKeyName);
    }

    /// <summary>
    /// InputKey 투명도 변경
    /// </summary>
    public void Set_InputKeyTransparency(float _opacityValue, ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (currInputKey == null)
            return;

        // 실린더 범위 30~100, 기본 값 0.3 + 수식값 0~0.7002~ = 투명도 범위 0.3 ~ 10.002
        _opacityValue = 0.3f + _opacityValue / (100 * 1.428f);

        settingPanel.Set_TransparencyText($"{(int)(_opacityValue * 100)}%");

        // InputPanel에서 InputKey들을 전부 가져와 투명도는 전부 변환한다.
        foreach (InputKey key in inputPanel.Get_InputKeys()) {
            key.Set_Opacity(_opacityValue);
        }
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

    /// <summary>
    /// 사이즈 실린더값 변경시 실행, InputKey 사이즈 변경
    /// </summary>
    public void OnValueChanged_SizeSlider(Slider _slider)
    {
        if (currInputKey == null)
        {
            settingPanel.Set_SizeText($"{(int)_slider.value}%");
            return;
        }

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;
        Set_InputKeySize(_slider.value, inputKeyName);
    }

    /// <summary>
    /// 투명도 실린더값 변경시 실행, InputKey 투명도 변경
    /// </summary>
    public void OnValueChanged_TransparencySlider(Slider _slider)
    {
        if (currInputKey == null)
        {
            settingPanel.Set_TransparencyText($"{(int)_slider.value}%");
            return;
        }

        ENUM_INPUTKEY_NAME inputKeyName = (ENUM_INPUTKEY_NAME)currInputKey.inputKeyNum;
        Set_InputKeyTransparency(_slider.value, inputKeyName);
    }

    /// <summary>
    /// Curr인풋키 값 비우는 함수
    /// </summary>
    public void Empty_CurrInputKey()
    {
        if (currInputKey == null && currAreaKey == null)
            return;

        currInputKey = null;
        currAreaKey = null;
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
        Empty_CurrInputKey();
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

    /// <summary>
    /// 변경중인 인풋키 값 초기화
    /// </summary>
    public void Reset_InputKey()
    {
        /*Managers.Resource.Destroy(inputPanel.gameObject);
        Managers.Resource.Destroy(areaPanel.gameObject);

        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Set_isReset(true);
        inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);

        Set_OnDragCallBack();

        areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
        areaPanel.Set_isReset(true);
        areaPanel.Init();*/

        Empty_CurrInputKey();
        inputPanel.Init(OnPoint_DownCallBack, OnPoint_UpCallBack);
        OnClick_InputSkillIcon(1);
        areaPanel.Init();
        settingPanel.Reset_SettingPanel();
    }

    public void Save_InputKey()
    {
        Save_InputKeyDatas();
        Empty_CurrInputKey();
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
