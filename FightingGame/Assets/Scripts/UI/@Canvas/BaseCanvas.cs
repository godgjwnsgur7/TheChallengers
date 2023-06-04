using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvas : MonoBehaviour
{
    CanvasScaler scaler = null;

	private void Awake()
    {
        Managers.UI.Init();
    }

    public virtual void Init()
    {
        scaler = GetComponent<CanvasScaler>();
        SetCanvasScaler();
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1.0f;
    }

    public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public virtual void Open<T>(UIParam param = null) { }
    public virtual void Close<T>() { }

    public virtual T GetUIComponent<T>() { return default(T); }
}

