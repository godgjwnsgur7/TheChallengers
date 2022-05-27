using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCanvas : MonoBehaviour
{
    CanvasScaler scaler = null;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
        scaler = GetComponent<CanvasScaler>();
        SetCanvasScaler();
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    public void Open<T>()
    {

    }

    public void Close<T>()
    {

    }
}
