using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopup : PopupUI
{
    [SerializeField] Text errorCodeText;
    [SerializeField] Text errorMessageText;

    public void Open(short _returnCode, string _message)
    {
        OnClick_SoundSFX((int)FGDefine.ENUM_SFX_TYPE.UI_Click_Notify);

        errorCodeText.text = $"코드번호 : {_returnCode}";
        errorMessageText.text = _message;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        errorCodeText.text = null;
        errorMessageText.text = null;
    }

    public void OnClick_Check()
    {
        // 로그인씬으로 이동?
        Close();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        OnClick_Check();
    }
}
