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
    public Image TouchArea;

    bool isInit = false;
    public bool isSelect = false;
    public int triggerCount = 0;

    public void Init(Action<InputKey> _OnPointDownCallBack, Action<InputKey> _OnPointUpCallBack)
    {
        if (isInit) return;

        isInit = true;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.UI)
            triggerCount++;

        Debug.Log(collision.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerCount > 0)
            Set_AreaColor();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerCount--;

        if (triggerCount < 1)
            Set_AreaColor();
    }

    public void Set_AreaColor()
    {
        Color changeColor;

        if (triggerCount > 0)
        {
            changeColor = new Color(255, 0, 0, 0.3f);
            TouchArea.color = changeColor;
        }
        else if (isSelect && triggerCount < 1)
        {
            changeColor = new Color(0, 255, 0, 0.3f);
            TouchArea.color = changeColor;
        }
        else if (!isSelect && triggerCount < 1)
        {
            changeColor = new Color(255, 255, 255, 0f);
            TouchArea.color = changeColor;
        }
    }

    public bool Get_Updatable()
    {
        if (triggerCount > 0)
            return false;
        else
            return true;
    }
}
