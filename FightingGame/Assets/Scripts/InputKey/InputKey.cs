using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class InputKey : MonoBehaviour
{
    Action<InputKey> OnPointDownCallBack;
    Action<InputKey> OnPointUpCallBack;

    public Image slotImage;
    public Image iconImage;

    public RectTransform rectTr;
    bool isInit = false;
    
    public void Init(Action<InputKey> _OnPointDownCallBack, Action<InputKey> _OnPointUpCallBack)
    {
        if (isInit) return;

        isInit = true;
        rectTr = this.GetComponent<RectTransform>();

        OnPointDownCallBack = _OnPointDownCallBack;
        OnPointUpCallBack = _OnPointUpCallBack;
    }

    public void EventTrigger_PointerDown()
    {
        if(OnPointDownCallBack == null)
        {
            Debug.Log("OnPointDownCallBack is Null");
            return;
        }

        OnPointDownCallBack(this);
    }

    public void EventTrigger_PointerUp()
    {
        if(OnPointDownCallBack == null)
        {
            Debug.Log("OnPointUpCallBack is Null");
            return;
        }
        
        OnPointUpCallBack(this);
    }
}
