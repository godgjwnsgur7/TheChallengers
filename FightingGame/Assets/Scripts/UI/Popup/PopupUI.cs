using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupUI : MonoBehaviour
{
    public virtual void SetPopup<T>() where T : PopupUI
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public abstract void Open<T>() where T : PopupUI;
    public abstract void Close<T>() where T : PopupUI;
}
