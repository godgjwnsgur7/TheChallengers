using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NotifyPopup : MonoBehaviour
{
    public bool isUsing
    {
        get { return isUsing; }
        private set { isUsing = value; }
    }

    [SerializeField] Text popupText;

    private Action callBack;

    private void Awake()
    {
        isUsing = false;
        callBack = null;
    }

    public void Open(string _message, Action _callBack = null)
    {
        isUsing = true;
        popupText.text = _message;

        if(_callBack != null) callBack += _callBack;

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
