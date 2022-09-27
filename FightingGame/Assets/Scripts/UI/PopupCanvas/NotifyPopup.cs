using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NotifyPopup : PopupUI
{
    [SerializeField] Text popupText;

    private Action callBack = null;

    public void Open(string _message, Action _callBack = null)
    {
        isUsing = true;
        popupText.text = _message;

        callBack = _callBack;

        this.gameObject.SetActive(true);
    }

    public void Open_Again(string _message, Action _callBack = null)
    {
        if(isUsing)
        {
            Open(_message, _callBack);
            return;
        }

        popupText.text = _message;

        callBack = _callBack;

        this.gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        if(callBack != null) callBack();

        this.gameObject.SetActive(false);
        callBack = null;
        popupText.text = null;
        isUsing = false;
    }
}
