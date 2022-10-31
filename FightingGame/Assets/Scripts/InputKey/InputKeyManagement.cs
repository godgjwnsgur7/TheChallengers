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

    public bool isActive = false;
    private bool isInit = false;
    private int inputKeyNum;

    public InputPanel inputPanel = null;
    private RectTransform panelTr = null;
    private InputKey inputKey = null;
    private RectTransform inputKeyRectTr = null;

    public AreaPanel areaPanel = null;
    private KeyArea keyArea = null;
    private RectTransform areaRectTr = null;

    private SettingPanel settingPanel;
    private EventTrigger eventTrigger;
    private EventTrigger.Entry triggerEntry = new EventTrigger.Entry
    {
        eventID = EventTriggerType.Drag,
    };

    public void Init()
    {   
        if(inputPanel == null)
        {
            // AreaPanel Instantiate
            areaPanel = Managers.Resource.Instantiate("UI/AreaPanel", this.transform).GetComponent<AreaPanel>();
            areaPanel.Init();

            // InputPanel Instantiate
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(OnClick_BeginClick, OnClick_EndClick);
            panelTr = inputPanel.GetComponent<RectTransform>();
        }

        Set_keySettingDataList();

        // 첫Init 때 드래그 이벤트트리거 생성
        if (!isInit)
        {
            for (int i = 0; i < keySettingDataList.Count; i++)
                Set_DragEventTrigger(inputPanel.Get_InputKey(((ENUM_INPUTKEY_NAME)i)));

            // 세팅패널 활성화
            settingPanel = this.transform.root.Find("@SettingPanel").GetComponent<SettingPanel>();
            settingPanel.Init();

            isInit = true;
        }
            
        Set_PanelActive(true);
    }

    // PlayerPrefs 값 호출
    private void Set_keySettingDataList()
    {
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

                keySettingDataList.Insert(i, new KeySettingData(i, 50, 100, inputKeyRectTr.anchoredPosition.x, inputKeyRectTr.anchoredPosition.y));
                Set_InputKey(i);
                Debug.Log((ENUM_INPUTKEY_NAME)i + "초기화");
            }

            PlayerPrefsManagement.Save_KeySettingData(keySettingDataList);
        }
        else
        {
            for (int i = 0; i < keySettingDataList.Count; i++)
                Set_InputKey(i);
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
        
        keySettingDataList[_inputkeyNum].size = _size;
    }

    // opacity 조절
    public void Set_InputKeyOpacity(float _opacity, int _inputkeyNum)
    {
        Image inputKeyImage;
        float opacityRatio = 0.5f + (_opacity / 200);
        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)_inputkeyNum);

        Transform imageObjectTr = inputKey.transform.Find("SlotImage");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetComponent<Image>();
            Set_ChangeColor(inputKeyImage, opacityRatio);
        }

        imageObjectTr = inputKey.transform.Find("IconArea");
        if (imageObjectTr != null)
        {
            inputKeyImage = imageObjectTr.GetChild(0).GetComponent<Image>();
            Set_ChangeColor(inputKeyImage, opacityRatio);
        }

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
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();

        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)_inputkeyNum);
        areaRectTr = keyArea.GetComponent<RectTransform>();

        Vector2 movePos = CheckTransformRange(new Vector2(_rectTrX, _rectTrY));
        inputKeyRectTr.anchoredPosition = movePos;
        areaRectTr.anchoredPosition = movePos;

        keySettingDataList[_inputkeyNum].rectTrX = movePos.x;
        keySettingDataList[_inputkeyNum].rectTrY = movePos.y;
    }

    // UI 이동 범위 체크
    public Vector2 CheckTransformRange(Vector2 changeVector)
    {
        Vector2 panelHalfSize = panelTr.sizeDelta / 2;

        // InputKey 가로,세로 길이의 반
        float scaleSizeX = (inputKeyRectTr.sizeDelta.x / 2) * inputKeyRectTr.localScale.x;
        float scaleSizeY = (inputKeyRectTr.sizeDelta.y / 2) * inputKeyRectTr.localScale.y;

        // 최대 이동범위 가로축 : InputPanel범위 안, 세로축 : InputPanel의 중심의 아래쪽
        float vecRangeX = Mathf.Clamp(changeVector.x, -panelHalfSize.x + scaleSizeX, panelHalfSize.x - scaleSizeX);
        float vecRangeY = Mathf.Clamp(changeVector.y, -panelHalfSize.y + scaleSizeY, (panelHalfSize.y / 2) - scaleSizeY);

        return new Vector2(vecRangeX, vecRangeY);
    }

    // InputKey 초기화
    public void Reset_InputKeyValue()
    {
        keySettingDataList = PlayerPrefsManagement.Load_KeySettingData();

        for (int i = 0; i < keySettingDataList.Count; i++)
            Set_InputKey(i);
    }

    #endregion 

    // PointerDown Event
    private void OnClick_BeginClick(InputKey _inputKey)
    {
        // 선택한 InputKey에 해당하는 Enum번호 찾기
        inputKeyNum = (int)Enum.Parse(typeof(ENUM_INPUTKEY_NAME), _inputKey.name);

        // 기존 선택 상태이던 버튼 area 색상 변경
        if (keyArea != null)
        {
            keyArea.isSelect = false;
            keyArea.Set_AreaColor();
        }

        inputKey = inputPanel.Get_InputKey((ENUM_INPUTKEY_NAME)inputKeyNum);
        inputKeyRectTr = inputKey.GetComponent<RectTransform>();
        triggerEntry.callback.AddListener(OnDrag);

        // Enum번호로 SettingPanel 등록
        settingPanel.OnClick_SetInputKey(inputKeyNum);

        // keyArea 색상 변경
        keyArea = areaPanel.Get_keyArea((ENUM_INPUTKEY_NAME)inputKeyNum);
        keyArea.isSelect = true;
        keyArea.Set_AreaColor();

        Debug.Log($"{_inputKey.name}세팅");
    }

    // InputKey EventTrigger에 OnDrag추가
    private void Set_DragEventTrigger(InputKey _inputKey)
    {
        eventTrigger = _inputKey.GetComponent<EventTrigger>();
        eventTrigger.triggers.Add(triggerEntry);
    }

    // 드래그로 InputKey 위치이동
    private void OnDrag(BaseEventData _data) 
    {
        if (!settingPanel.isHide)
            OnBeginDrag();

        PointerEventData data = (PointerEventData)_data;
        Vector2 movePos = inputKeyRectTr.anchoredPosition + data.delta;
        Set_InputKeyTransForm(movePos.x, movePos.y, inputKeyNum);
    }

    private void OnBeginDrag() => settingPanel.Hide_SettingPanel();

    // PointerUp Event
    private void OnClick_EndClick(InputKey _ienputKey)
    {
        if (eventTrigger.triggers.Count <= 2)
            return;

        triggerEntry.callback.RemoveListener(OnDrag);
    }

    // Panel들 Active상태 변환
    public void Set_PanelActive(bool _changeBool)
    {
        this.isActive = _changeBool;
        this.inputPanel.gameObject.SetActive(_changeBool);
        this.areaPanel.gameObject.SetActive(_changeBool);

        if (!_changeBool)
        {
            keyArea.isSelect = false;
            keyArea.Set_AreaColor();
            return;
        }
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
