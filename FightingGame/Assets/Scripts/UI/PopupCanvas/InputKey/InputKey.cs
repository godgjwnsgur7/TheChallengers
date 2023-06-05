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

    public RectTransform rectTr; // 외부에서 너무 가지고 놈. 언젠간 고쳐줄게...
    public int inputKeyNum;

    bool isInit = false;
    float beforeTransparency = 0;


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

        beforeTransparency = Get_Transparency();

        Set_Transparency(beforeTransparency * 0.7f);

        OnPointDownCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public virtual void EventTrigger_PointerUp()
    {
        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointUpCallBack is Null");
            return;
        }

        Set_Transparency(beforeTransparency);

        OnPointUpCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public virtual void Set_Transparency(float _transparency)
    {
        Color changeColor = slotImage.color;
        changeColor.a = _transparency;
        slotImage.color = changeColor;
    }

    public float Get_Transparency()
    {
        return slotImage.color.a;
    }
}
