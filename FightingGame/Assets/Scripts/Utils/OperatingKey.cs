using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OperatingKey : MonoBehaviour
{
    Action OnPointDownAction;
    Action OnPointUpAction;

    bool isInit = false;

    public void Init(Action _OnPointDownAction, Action _OnPointUpAction)
    {
        if (isInit) return;

        isInit = true;

        OnPointDownAction = _OnPointDownAction;
        OnPointUpAction = _OnPointUpAction;
    }

    public void EventTrigger_PointerDown()
    {
        OnPointDownAction();
    }

    public void EventTrigger_PointerUp()
    {
        OnPointUpAction();
    }
}
