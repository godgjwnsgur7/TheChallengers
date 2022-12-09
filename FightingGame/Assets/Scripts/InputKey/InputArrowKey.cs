using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputArrowKey : InputKey
{
    Action<float> OnPointEnterCallBack;

    public void Connect_Player(Action<float> _OnPointEnterCallBack)
    {
        OnPointEnterCallBack = _OnPointEnterCallBack;
    }

    public void EventTrigger_PointerEnter(float moveDir)
    {
        if(OnPointEnterCallBack == null)
        {
            Debug.Log("OnPointEnterCallBack is Null");
            return;
        }

        OnPointEnterCallBack(moveDir);
    }
}
