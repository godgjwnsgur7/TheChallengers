using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class InputKey : MonoBehaviour
{
    Action<ENUM_INPUTKEY_NAME> OnPointDownCallBack;
    Action<ENUM_INPUTKEY_NAME> OnPointUpCallBack;

    public Image slotImage;
    public Image iconImage;

    public RectTransform rectTr;
    public int inputKeyNum;
    bool isInit = false;
    
    public void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (isInit) return;

        isInit = true;
        rectTr = this.GetComponent<RectTransform>();
        inputKeyNum = (int)Enum.Parse(typeof(ENUM_INPUTKEY_NAME), gameObject.name);

        OnPointDownCallBack = _OnPointDownCallBack;
        OnPointUpCallBack = _OnPointUpCallBack;
    }

    public virtual void EventTrigger_PointerDown()
    {
        Debug.Log("포인트 다운 실행");

        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointDownCallBack is Null");
            return;
        }

        OnPointDownCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public void EventTrigger_PointerUp()
    {
        Debug.Log("포인트 업 실행");

        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointUpCallBack is Null");
            return;
        }
        
        OnPointUpCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }
}
