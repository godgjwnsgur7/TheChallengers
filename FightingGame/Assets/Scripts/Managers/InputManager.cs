using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputManager
{
    public Action<ENUM_EVENT_TYPE> Action = null;

    public Vector2 touchPos
    {
        get;
        private set;
    }

    public void OnUpdate()
    {
        if (!Input.anyKey)
            return;

        if(Action != null)
        {
            
        }
    }

    public void SetTouchPosition(Vector2 _touchPos)
    {
        touchPos = _touchPos;
    }
}