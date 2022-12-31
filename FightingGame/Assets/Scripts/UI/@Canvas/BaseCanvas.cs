using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseCanvas : MonoBehaviour
{
    CanvasScaler scaler = null;
    
    private void Awake()
    {
        Managers.UI.Init(); // 임시
        Init();
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
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
    }

    public void OnClick_Activate(GameObject g) => g.SetActive(true);
    public void OnClick_Deactivate(GameObject g) => g.SetActive(false);

    public virtual void Open<T>(UIParam param = null) { }
    public virtual void Close<T>() { }

    public virtual void PlaySFX_CallBack(int sfxNum) { }
    public virtual T GetUIComponent<T>() { return default(T); }
}

