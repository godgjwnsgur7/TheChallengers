using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMgr
{
    public BaseCanvas gameCanvas;
    public PopupCanvas popupCanvas = null;

    public void Init()
    {
        gameCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        if(popupCanvas == null)
            popupCanvas = GameObject.FindObjectOfType<PopupCanvas>();
            
    }

    public void Clear()
    {
        gameCanvas = null;
    }
    
    public void Open<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(UIElement))) gameCanvas.Open<T>();
        // else if (typeof(T).IsSubclassOf(typeof(UIPopup))) popupCanvas.Open<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }
    
    public void OpenPopup<T>()
    {
        if (typeof(T).IsSubclassOf(typeof(PopupUI))) popupCanvas.Open<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }

    public void Close<T>()
    {
        if (typeof(T) == typeof(UIElement)) gameCanvas.Close<T>();
        else if (typeof(T) == typeof(PopupUI)) popupCanvas.Close<T>();
        else Debug.Log($"범위 벗어남 : {typeof(T)}");
    }
}