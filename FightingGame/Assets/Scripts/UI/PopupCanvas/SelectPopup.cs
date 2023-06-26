using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SelectPopup : PopupUI
{
    [SerializeField] Text popupText;

    private Action succeededCallBack = null;
    private Action failedCallBack = null;

    public void Open(Action _succeededCallBack, Action _failedCallBack, string _message)
    {
        popupText.text = _message;
        succeededCallBack = _succeededCallBack;
        failedCallBack = _failedCallBack;

        this.gameObject.SetActive(true);
    }

    public void OnClick_Yes()
    {
        Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);

        succeededCallBack?.Invoke();

        Close();
    }

    public void OnClick_No()
    {
        if(!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Cancel);

        failedCallBack?.Invoke();

        Close();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        succeededCallBack = null;
        failedCallBack = null;
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_No();
    }
}
