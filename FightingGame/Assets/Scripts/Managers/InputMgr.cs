using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputMgr
{
    public Action<ENUM_INPUT_TYPE> Action = null;

    public Vector2 touchPos
    { 
        get;
        private set;
    }

    public void OnUpdate()
    {
        if (Action != null)
        {
            if (touchPos != Vector2.zero) // ¿Ãµø
                Action.Invoke(ENUM_INPUT_TYPE.Joystick);
        }
    }

    public void SetTouchPosition(Vector2 _touchPos)
    {
        touchPos = _touchPos;
    }
}