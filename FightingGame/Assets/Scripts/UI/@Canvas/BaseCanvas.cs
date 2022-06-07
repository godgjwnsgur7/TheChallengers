using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvas : MonoBehaviour
{
    CanvasScaler scaler = null;

    private void Start()
    {
        Init();
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

    public abstract void Open<T>(UIParam param = null);
    public abstract void Close<T>();

}

