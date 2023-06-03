using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class InputArrowKey : InputKey
{
    Action<float> OnPointEnterCallBack;

    [SerializeField] Image leftArrowImage;
    [SerializeField] Image rightArrowImage;

    public void Connect_Player(Action<float> _OnPointEnterCallBack)
    {
        OnPointEnterCallBack = _OnPointEnterCallBack;
    }

    public override void EventTrigger_PointerDown()
    {
        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointDownCallBack is Null");
            return;
        }

        GetComponent<Image>().sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/DirectionKey/move_key_selected_total");
        
        OnPointDownCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public override void EventTrigger_PointerUp()
    {
        if (OnPointDownCallBack == null)
        {
            Debug.Log("OnPointUpCallBack is Null");
            return;
        }
        OnPointUpCallBack((ENUM_INPUTKEY_NAME)inputKeyNum);
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
