using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPopup : PopupUI
{
    [SerializeField] Text titleText;
    [SerializeField] Text errorMessageText;

    public void Open(short _returnCode, string _message)
    {
        titleText.text = $"코드번호 : {_returnCode}";
        errorMessageText.text = _message;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        titleText.text = null;
        errorMessageText.text = null;
    }

    public void OnClick_Check()
    {
        Close();
        Debug.Log("로그인 씬으로 이동?");
    }
}
