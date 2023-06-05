using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class InputKeyManagement : MonoBehaviour
{
    [SerializeField] KeySettingWindow keySettingWindow;
    [SerializeField] InputKeyAreaPanel inputKeyAreaPanel;

    public InputKeyArea selectedKeyArea = null;
    public float moveX = 0;
    public float moveY = 0;

    Coroutine moveKeyPosCoroutine = null;

    bool isInit = false;

    private void Start()
    {
        Init();
    }

    public void Open()
    {
        Init();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        isInit = false;

        gameObject.SetActive(false);
    }

    private void Init()
    {
        if (isInit)
            return;

        isInit = true;

        inputKeyAreaPanel.Init(OnPointDownCallBack, OnPointUpCallBack);
    }

    private void Instantiate_InputKeyAreaPanel()
    {
        if (inputKeyAreaPanel != null)
            Managers.Resource.Destroy(inputKeyAreaPanel.gameObject);

        inputKeyAreaPanel = Managers.Resource.Instantiate("UI/InputKeyAreaPanel", this.transform).GetComponent<InputKeyAreaPanel>();
        inputKeyAreaPanel.transform.SetSiblingIndex(4);
        inputKeyAreaPanel.Init(OnPointDownCallBack, OnPointUpCallBack);
    }

    public void OnPointDownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (selectedKeyArea != null && selectedKeyArea.inputKeyNum != (int)_inputKeyName)
        {
            selectedKeyArea.Deactive_AreaImage();
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

    protected IEnumerator IMoveInputKeyPosition()
    {
        Vector2 touchPosVec = Input.mousePosition;

        while(selectedKeyArea != null)
        {
            Debug.Log($"{touchPosVec.x}, {touchPosVec.y}");
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

    public void OnClick_ChangeCharacter(int _charTypeNum)
    {

    }

    public void OnClick_Exit()
    {
        Close();
    }

    public void OnClick_Initialize()
    {
        inputKeyAreaPanel.Reset_InputKeyData();
    }

    public void OnClick_Save()
    {
        inputKeyAreaPanel.Save_InputKeyData();
    }
}
