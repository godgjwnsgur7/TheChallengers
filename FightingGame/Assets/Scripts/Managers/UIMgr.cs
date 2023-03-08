using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIMgr
{
    public BaseCanvas currCanvas;
    public PopupCanvas popupCanvas = null;

    private Material blurMaterial = null;

    public void Init()
    {
        currCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        currCanvas.Init();

        if(popupCanvas == null)
            popupCanvas = GameObject.FindObjectOfType<PopupCanvas>();

        GameObject go = GameObject.Find("PopupCanvas");
        if (go == null)
            go = Managers.Resource.Instantiate("UI/PopupCanvas");
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

    public void SetBlurUI(Image image, bool isBlurOn)
	{
        if (image == null)
		{
            Debug.LogError("Blur 처리할 이미지 없음");
            return;
        }

        image.material = isBlurOn ? LoadBlurMaterial() : null;
    }

    private Material LoadBlurMaterial()
	{
        string blurPath = "Materials/Blur";

        if (blurMaterial == null)
            blurMaterial = Resources.Load<Material>(blurPath);

        return blurMaterial;
    }
}