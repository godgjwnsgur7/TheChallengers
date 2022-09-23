using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMgr
{
    public BaseCanvas currCanvas;
    public PopupCanvas popupCanvas = null;

    public void Init()
    {
        currCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        if(popupCanvas == null)
            popupCanvas = GameObject.FindObjectOfType<PopupCanvas>();

        GameObject go = GameObject.Find("PopupCanvas");
        if (go == null)
            go = Managers.Resource.Instantiate("UI/PopupCanvas");
    }

    public void Clear()
    {
        currCanvas = null;
    }
    
    public void OpenUI<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(UIElement))) currCanvas.Open<T>();
        // else if (typeof(T).IsSubclassOf(typeof(PopupUI))) popupCanvas.Open<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }
    
    public void CloseUI<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(UIElement))) currCanvas.Close<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }

    public T GetUIComponent<T>()
    {

        return default(T);
    }
}