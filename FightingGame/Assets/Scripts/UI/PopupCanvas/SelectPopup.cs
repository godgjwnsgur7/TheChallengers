using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SelectPopup : MonoBehaviour
{
    public bool isUsing
    {
        get { return isUsing; }
        private set { isUsing = value; }
    }

    [SerializeField] Text popupText;

    private Action succeededCallBack;
    private Action failedCallBack;

    private void Awake()
    {
        isUsing = false;
        succeededCallBack = null;
        failedCallBack = null;
    }

    public void Open(Action _succeededCallBack, Action _failedCallBack, string _message)
    {
        isUsing = true;
        popupText.text = _message;
        succeededCallBack = _succeededCallBack;
        failedCallBack = _failedCallBack;

        this.gameObject.SetActive(true);
    }

    public void OnClick_Yes()
    {
        if (succeededCallBack != null) 
            succeededCallBack();

        Close();
    }

    public void OnClick_No()
    {
        if (failedCallBack != null)
            failedCallBack();
    
        Close();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        succeededCallBack = null;
        failedCallBack = null;
        isUsing = false;
    }
}
