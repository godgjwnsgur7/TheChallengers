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
        popupCanvas = GameObject.FindObjectOfType<PopupCanvas>();
        if (popupCanvas == null)
            popupCanvas = Managers.Resource.Instantiate("UI/PopupCanvas").GetComponent<PopupCanvas>();
        popupCanvas.Init();

        currCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        currCanvas.Init();
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
}