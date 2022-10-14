using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using UnityEngine.EventSystems;
using System;

public class InputKeyManagement : MonoBehaviour
{
    List<KeySettingData> keySettingDataList = null;

    private InputPanel inputPanel = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    private AreaPanel areaPanel = null;
    private KeyArea keyArea = null;
    private RectTransform areaRectTr = null;

    private SettingPanel settingPanel;
    private EventTrigger eventTrigger;

    public void Init()
    {
        // AreaPanel Instantiate
        areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
        areaPanel.Init();

        // InputPanel Instantiate
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        inputPanel.Init(OnClick_BeginClick, OnClick_EndClick);

        // 설정된 PlayerPrefs 호출
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();

        // 설정된 PlayerPrefs가 없으면 초기화
        if (keySettingDataList == null)
        {
            keySettingDataList = new List<KeySettingData>();
            for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
            {
                inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)i);
                inputKeyRectTr = inputKey.GetComponent<RectTransform>();

                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.position.x, inputKeyRectTr.position.y));
                Set_InputKey(i);
                Debug.Log((ENUM_INPUTKEY_NAME)i + "초기화");
            }
        }

        settingPanel = this.transform.root.Find("SettingPanel").GetComponent<SettingPanel>();
        settingPanel.Init();
    }

    public void Set_InputKey(int _inputkeyNum)
    {
        Set_InputKeySize(keySettingDataList[_inputkeyNum].size, _inputkeyNum);
        Set_InputKeyOpacity(keySettingDataList[_inputkeyNum].opacity, _inputkeyNum);
        Set_InputKeyTransForm(keySettingDataList[_inputkeyNum].rectTrX, keySettingDataList[_inputkeyNum].rectTrY, _inputkeyNum);
    }

    public void Set_InputKeySize(float _size, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)_inputkeyNum);

        float sizeRatio = (50 + _size) / 100;
        Vector3 changeScale = new Vector3(1, 1, 1) * sizeRatio;

        // InputKey 크기 변경
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.localScale = changeScale;

        // KeyArea 크기 변경
        areaRectTr = keyArea.GetComponent<RectTransform>();
        areaRectTr.localScale = changeScale;

        // 변경값 임시 저장
        keySettingDataList[_inputkeyNum].size = _size;
    }

    public void Set_InputKeyOpacity(float _opacity, int _inputkeyNum)
    {
        float opacityRatio = 0.5f + (_opacity / 200);
        Color changeColor;
        Image inputKeyImage;

        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        Transform imageObjectTr = inputKey.transform.Find("SlotImage");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
        }

        imageObjectTr = inputKey.transform.Find("IconArea");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
            changeColor = inputKeyImage.color;
            changeColor.a = opacityRatio;
            inputKeyImage.color = changeColor;
        }

        // 변경값 임시 저장
        keySettingDataList[_inputkeyNum].opacity = _opacity;
    }

    public void Set_InputKeyTransForm(float _rectTrX, float _rectTrY, int _inputkeyNum)
    {
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);
        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Vector2 changeVector = new Vector2(_rectTrX, _rectTrY);

        // InputKey 위치 변경
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        inputKeyRectTr.position = changeVector;

        // KeyArea 위치 변경
        areaRectTr = keyArea.GetComponent<RectTransform>();
        areaRectTr.position = changeVector;

        // 변경값 임시 저장
        keySettingDataList[_inputkeyNum].rectTrX = _rectTrX;
        keySettingDataList[_inputkeyNum].rectTrY = _rectTrY;
    }

    // InputPanel Init 테스트 용 임시
    private void OnClick_BeginClick(InputKey _inputKey)
    {
        // 기존 선택 상태이던 버튼 area 색상 변경
        keyArea.isSelect = false;
        keyArea.Set_AreaColor();

        // 선택한 InputKey에 해당하는 Enum번호 찾기
        for (int i = 0; i < (int)ENUM_INPUTKEY_NAME.Max; i++)
        {
            if (((ENUM_INPUTKEY_NAME)i).ToString() == _inputKey.name)
            {
                int inputKeyNum = i;
                // Enum번호로 SettingPanel 등록
                settingPanel.OnClick_SetInputKey(inputKeyNum);

                // keyArea 색상 변경
                keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)inputKeyNum);
                keyArea.isSelect = true;
                keyArea.Set_AreaColor();

                // 드래그 이벤트 트리거 생성
                Set_DragEventTrigger(_inputKey, inputKeyNum);
                break;
            }
        }
        Debug.Log($"{_inputKey.name}세팅");
    }

    private void Set_DragEventTrigger(InputKey _inputKey, int _inputKeyNum)
    {
        eventTrigger = _inputKey.GetComponent<EventTrigger>();

        if (eventTrigger.triggers.Count > 2)
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.BeginDrag;
        entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data, _inputKeyNum); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnBeginDrag(PointerEventData data, int _inputKeyNum)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDrag((PointerEventData)data, _inputKeyNum); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnDrag(PointerEventData data, int _inputKeyNum) 
    {
        Set_InputKeyTransForm(data.position.x, data.position.y, _inputKeyNum);
    }

    private void OnClick_EndClick(InputKey _inputKey)
    {
        EventTrigger eventTrigger = _inputKey.GetComponent<EventTrigger>();
        OnEndDrag(eventTrigger);
    }

    private void OnEndDrag(EventTrigger _eventTrigger)
    {
        if (_eventTrigger.triggers.Count <= 2)
            return;

        _eventTrigger.triggers.RemoveRange(2, eventTrigger.triggers.Count - 2);
    }

    public void Save_KeySettingData()
    {
        PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
    }

    public KeySettingData Get_KeySettingData(int _inputKeyNum)
    {
        return keySettingDataList[_inputKeyNum];
    }
}
