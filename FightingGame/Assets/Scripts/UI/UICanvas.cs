using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UICanvas : MonoBehaviour
{
    public Canvas canvas = null;
    public CanvasScaler scaler = null;

    private void Start()
    {
        
    }

    public void Init()
    {
        canvas = GetComponent<Canvas>();

        scaler = GetComponent<CanvasScaler>();
        SetCanvasScaler();
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }
}
