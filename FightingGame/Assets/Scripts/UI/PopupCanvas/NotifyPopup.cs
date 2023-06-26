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
        popupText.text = _message;

        callBack = _callBack;

        this.gameObject.SetActive(true);
    }

    public void Open_Again(string _message, Action _callBack = null)
    {
        this.gameObject.SetActive(false);

        Open(_message, _callBack);
    }

    public void OnClick_Check()
    {
        if (!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Enter);

        callBack?.Invoke();

        this.gameObject.SetActive(false);
        callBack = null;
        popupText.text = null;
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_Check();
    }
}
