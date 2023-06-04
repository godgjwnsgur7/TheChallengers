using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : PopupUI
{
    [SerializeField] Text popupText;

    public void Open(string _message)
    {
        popupText.text = _message;
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
