using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPopup : PopupUI
{
    public void Open()
    {
        isUsing = true;

        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        isUsing = false;
    }
}
