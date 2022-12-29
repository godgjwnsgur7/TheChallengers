using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class InputKey : MonoBehaviour
{
    protected Action<ENUM_INPUTKEY_NAME> OnPointDownCallBack;
    protected Action<ENUM_INPUTKEY_NAME> OnPointUpCallBack;

    [SerializeField] protected Image slotImage;

    public RectTransform rectTr;
    public int inputKeyNum;

    bool isInit = false;
    
    public void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (isInit) return;
        isInit = true;

        if(rectTr == null)
            rectTr = this.GetComponent<RectTransform>();
        
        inputKeyNum = (int)Enum.Parse(typeof(ENUM_INPUTKEY_NAME), gameObject.name);

        OnPointDownCallBack = _OnPointDownCallBack;
        OnPointUpCallBack = _OnPointUpCallBack;
    }

    public virtual void EventTrigger_PointerDown()
    {
        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointDownCallBack is Null");
            return;
        }

        Set_Opacity(0.9f);

        OnPointDownCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public virtual void EventTrigger_PointerUp()
    {
        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointUpCallBack is Null");
            return;
        }

        Set_Opacity(1f);

        OnPointUpCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public virtual void Set_Opacity(float _opacity)
    {
        Color changeColor = slotImage.color;
        changeColor.a = _opacity;
        slotImage.color = changeColor;
    }

    public float Get_Opacity()
    {
        return slotImage.color.a;
    }
}
