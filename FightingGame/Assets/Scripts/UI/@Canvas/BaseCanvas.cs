using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvas : MonoBehaviour
{
    CanvasScaler scaler = null;
    
    private void Start()
    {
        Init(); // 해당하는 씬에서 정보를 받아온다. (임시)
    }

    public void Init()
    {
        scaler = GetComponent<CanvasScaler>();
        SetCanvasScaler();
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    public virtual void Open<T>(UIParam param = null) { }
    public virtual void Close<T>() { }

    public virtual T GetUIComponent<T>() { return default(T); }
}

