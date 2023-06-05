using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class InputKeyManagement : MonoBehaviour
{
    [SerializeField] KeySettingWindow keySettingWindow;
    [SerializeField] InputKeyAreaPanel inputKeyAreaPanel;
    [SerializeField] GameObject selectCharDropDownAreaObject;

    InputKeyArea selectedKeyArea = null;
    Coroutine moveKeyPosCoroutine = null;

    bool isInit = false;
    bool isChangeValue = false;

    /*
    private void Start()
    {
        Init();
    }
    */

    private void OnEnable()
    {
        isChangeValue = false;
    }

    public void Open()
    {
        Init();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        isInit = false;
        selectedKeyArea = null;

        gameObject.SetActive(false);
    }

    private void Init()
    {
        if (isInit)
            return;

        isInit = true;

        inputKeyAreaPanel.Init(OnPointDownCallBack, OnPointUpCallBack);
        keySettingWindow.Init(OnChangeSizeSliderCallBack, OnChageOpacitySliderCallBack, inputKeyAreaPanel.Get_InputKeyArea(0).Get_Transparency());
    }

    public void OnPointDownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        isChangeValue = true;
        keySettingWindow.Set_SizeliderInteractable();

        if (selectedKeyArea != null && selectedKeyArea.inputKeyNum != (int)_inputKeyName)
        {
            selectedKeyArea.Deselect_AreaImage();
            selectedKeyArea = null;
        }

        selectedKeyArea = inputKeyAreaPanel.Get_InputKeyArea((int)_inputKeyName);
        moveKeyPosCoroutine = StartCoroutine(IMoveInputKeyPosition());
    }

    public void OnPointUpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (moveKeyPosCoroutine != null)
            StopCoroutine(moveKeyPosCoroutine);
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

    protected IEnumerator IMoveInputKeyPosition()
    {
        Vector2 touchPosVec = Input.mousePosition;
        float moveX, moveY;

        while(selectedKeyArea != null)
        {
            moveX = Input.mousePosition.x - touchPosVec.x;
            moveY = Input.mousePosition.y - touchPosVec.y;

            selectedKeyArea.transform.localPosition = new Vector2(
                selectedKeyArea.transform.localPosition.x + moveX,
                selectedKeyArea.transform.localPosition.y + moveY);

            touchPosVec = new Vector2(touchPosVec.x + moveX, touchPosVec.y + moveY);

            yield return null;
        }

        moveKeyPosCoroutine = null;
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

    public void OnClick_Exit()
    {
        string massage = isChangeValue ? "변경된 값이 있습니다.\n저장하지 않고 종료하시겠습니까?" :
            "키 설정 창을 종료하시겠습니까?";
        Managers.UI.popupCanvas.Open_SelectPopup(Exit_Management, null, massage);
    }
    private void Exit_Management()
    {
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Close);
    }

    public void OnClick_Initialize()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(Init_InputKeySetting, null, "키 설정을 초기화하시겠습니까?\n초기화 후엔 되돌릴 수 없습니다.");
    }
    private void Init_InputKeySetting()
    {
        inputKeyAreaPanel.Reset_InputKeyData();
    }

    public void OnClick_Save()
    {
        Managers.UI.popupCanvas.Open_SelectPopup(Save_InputKeyData, null, "저장하시겠습니까?");
    }
    private void Save_InputKeyData()
    {
        inputKeyAreaPanel.Save_InputKeyData();
    }
}
