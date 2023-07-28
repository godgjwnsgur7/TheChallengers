using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

enum ENUM_DIRECTION_TYPE
{
    Left = 1, Right = 2, Up = 3, Down = 4,
}

public class InputKeyManagement : UIElement
{
    [SerializeField] KeySettingWindow keySettingWindow;
    [SerializeField] InputKeyAreaPanel inputKeyAreaPanel;
    [SerializeField] GameObject selectCharDropDownAreaObject;

    InputKeyArea selectedKeyArea = null;
    Coroutine moveKeyToMousePosCoroutine = null;
    Coroutine moveKeyFineAdjustmentCoroutine = null;

    bool isInit = false;
    bool isChangeValue = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        isChangeValue = false;
    }

    public void Open()
    {
        Init();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }

    private void Init()
    {
        if (isInit)
            return;

        isInit = true;

        inputKeyAreaPanel.Init(OnPointDownCallBack_InputKeyArea, OnPointUpCallBack_InputKeyArea);
        keySettingWindow.Init(OnChangeSizeSliderCallBack, OnChageOpacitySliderCallBack, inputKeyAreaPanel.Get_InputKeyArea(0).Get_Transparency());

        selectCharDropDownAreaObject.SetActive(false);
    }

    private void Reset_ManagementSetting()
    {
        Close();
        Open();
        inputKeyAreaPanel.Reset_InputKeyData();
        keySettingWindow.Set_OpacitySliderValue(100f);
    }

    private void Clear()
    {
        isInit = false;
        selectedKeyArea = null;

        inputKeyAreaPanel.Clear();
        Managers.Input.Set_InputKeyControllerPos();
    }

    public void OnPointDownCallBack_InputKeyArea(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        isChangeValue = true;
        keySettingWindow.Set_SizeliderInteractable();

        if (selectedKeyArea != null && selectedKeyArea.inputKeyNum != (int)_inputKeyName)
        {
            selectedKeyArea.Deselect_AreaImage();
            selectedKeyArea = null;
        }

        selectedKeyArea = inputKeyAreaPanel.Get_InputKeyArea((int)_inputKeyName);
        keySettingWindow.Set_SizeSliderValue(selectedKeyArea.rectTr.localScale.x * 100);

        moveKeyToMousePosCoroutine = StartCoroutine(IMoveInputKeyToMousePos());
    }

    public void OnPointUpCallBack_InputKeyArea(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (moveKeyToMousePosCoroutine != null)
            StopCoroutine(moveKeyToMousePosCoroutine);
    }

    /// <summary>
    /// directionType : ENUM_DIRECTION_TYPE
    /// </summary>
    public void OnPointDownCallBack_FineAdjustment(int _directionType)
    {
        if (selectedKeyArea == null)
            return;

        ENUM_DIRECTION_TYPE directionType = (ENUM_DIRECTION_TYPE)_directionType;

        Vector2 vec = Vector2.zero;
        switch(directionType)
        {
            case ENUM_DIRECTION_TYPE.Left: vec = new Vector2(-0.5f, 0); break;
            case ENUM_DIRECTION_TYPE.Right: vec = new Vector2(0.5f, 0); break;
            case ENUM_DIRECTION_TYPE.Up: vec = new Vector2(0, 0.5f); break;
            case ENUM_DIRECTION_TYPE.Down: vec = new Vector2(0, -0.5f); break;
        }

        if(vec != Vector2.zero)
            moveKeyFineAdjustmentCoroutine = StartCoroutine(IMoveInputKeyFineAdjustment(vec));
    }

    public void OnPointUpCallBack_FineAdjustment()
    {
        if (moveKeyFineAdjustmentCoroutine != null)
            StopCoroutine(moveKeyFineAdjustmentCoroutine);
    }

    public void OnChangeSizeSliderCallBack(float _value)
    {
        if (selectedKeyArea == null)
            return;

        selectedKeyArea.Set_ScaleSize(_value);
    }

    public void OnChageOpacitySliderCallBack(float _value)
    {
        inputKeyAreaPanel.Set_OpacityValueAll(_value);
    }

    private void Set_selectedKeyAreaPos(Vector2 _movePosVec)
    {
        // 선택된 인풋키 반지름 값
        float scaleSizeX = (selectedKeyArea.rectTr.sizeDelta.x / 2) * selectedKeyArea.rectTr.localScale.x;
        float scaleSizeY = (selectedKeyArea.rectTr.sizeDelta.y / 2) * selectedKeyArea.rectTr.localScale.y;

        // 인풋키 위치 범위
        float vecRangeX = Mathf.Clamp(_movePosVec.x, -920f + scaleSizeX, 920f - scaleSizeX);
        float vecRangeY = Mathf.Clamp(_movePosVec.y, -500f + scaleSizeY, 80f - scaleSizeY);

        Vector3 moveToPosVec = new Vector3(vecRangeX, vecRangeY, 0);

        selectedKeyArea.transform.localPosition = moveToPosVec;
    }

    protected IEnumerator IMoveInputKeyToMousePos()
    {
        if(moveKeyFineAdjustmentCoroutine != null)
            StopCoroutine (moveKeyFineAdjustmentCoroutine);

        Vector3 touchPosVec = Input.mousePosition;
        float moveX, moveY;

        while(selectedKeyArea != null)
        {
            moveX = Input.mousePosition.x - touchPosVec.x;
            moveY = Input.mousePosition.y - touchPosVec.y;

            Vector3 movePosVec = new Vector3(
                selectedKeyArea.transform.localPosition.x + moveX,
                selectedKeyArea.transform.localPosition.y + moveY, 0);

            Set_selectedKeyAreaPos(movePosVec);

            touchPosVec = new Vector3(touchPosVec.x + moveX, touchPosVec.y + moveY, 0);

            yield return null;
        }

        moveKeyToMousePosCoroutine = null;
    }
    
    protected IEnumerator IMoveInputKeyFineAdjustment(Vector2 vec)
    {
        if (moveKeyToMousePosCoroutine != null)
            StopCoroutine(moveKeyToMousePosCoroutine);

        while (selectedKeyArea != null)
        {
            Vector2 movePosVec = new Vector2(
                selectedKeyArea.transform.localPosition.x + vec.x,
                selectedKeyArea.transform.localPosition.y + vec.y);

            Set_selectedKeyAreaPos(movePosVec);

            yield return null;
        }

        moveKeyFineAdjustmentCoroutine = null;
    }

    public void OnClick_CharSelectArea(bool active)
    {
        selectCharDropDownAreaObject.gameObject.SetActive(active);
    }

    public void OnClick_ChangeCharacter(int _charTypeNum)
    {
        ENUM_CHARACTER_TYPE charType = (ENUM_CHARACTER_TYPE)_charTypeNum;

        keySettingWindow.ChangeCharacterText(charType);
        inputKeyAreaPanel.Set_ChangeIcon(charType);

        OnClick_CharSelectArea(false);
    }

    public override void OnClick_Exit()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);
        
        if (isChangeValue)
            Managers.UI.popupCanvas.Open_SelectPopup(Exit_Management, null
                , "변경된 값이 있습니다.\n저장하지 않고 종료하시겠습니까?");
        else
            Exit_Management();
    }
    private void Exit_Management()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Close);
    }

    public void OnClick_Initialize()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);
        Managers.UI.popupCanvas.Open_SelectPopup(Init_InputKeySetting, null, "키 설정을 초기화하시겠습니까?\n초기화 후엔 되돌릴 수 없습니다.");
    }
    private void Init_InputKeySetting()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Reset_ManagementSetting);
    }

    public void OnClick_Save()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Enter);
        Managers.UI.popupCanvas.Open_SelectPopup(Save_InputKeyData, null, "저장하시겠습니까?");
    }
    private void Save_InputKeyData()
    {
        if (inputKeyAreaPanel.Save_InputKeyData())
            isChangeValue = false;
    }
}
