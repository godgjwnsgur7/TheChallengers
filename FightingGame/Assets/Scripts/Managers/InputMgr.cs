using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputMgr
{
    public Action<ENUM_INPUT_TYPE> Action = null;
    private InputKeyController inputKeyController;
    private InputKeyManagement inputKeyManagement;
    private BaseCanvas currCanvas;

    public Vector2 touchPos
    { 
        get;
        private set;
    }

    public void OnUpdate()
    {
        if (!Input.anyKey) return;

        if (Action != null)
        {
            if (touchPos != Vector2.zero) // 이동
                Action.Invoke(ENUM_INPUT_TYPE.Joystick);
        }
        
    }

    public void SetTouchPosition(Vector2 _touchPos)
    {
        touchPos = _touchPos;
    }

    public InputKeyManagement Get_InputKeyManagement()
    {
        currCanvas = Managers.UI.currCanvas;

        if (inputKeyManagement == null)
            inputKeyManagement = Managers.Resource.Instantiate("UI/InputKeyManagement", currCanvas.transform).GetComponent<InputKeyManagement>();

        return inputKeyManagement;
    }

    public InputKeyController Get_InputKeyController()
    {
        currCanvas = Managers.UI.currCanvas;

        if (inputKeyController == null)
            inputKeyController = Managers.Resource.Instantiate("UI/InputKeyController", currCanvas.transform).GetComponent<InputKeyController>();

        return inputKeyController;
    }

    public void Clear()
    {
        inputKeyController = null;
        inputKeyManagement = null;
    }
}