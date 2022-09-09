using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당하는 씬에서만 존재할 UI 속성들
/// </summary>
public class UIElement : MonoBehaviour
{
    public bool isOpen = false;

    public virtual void Open(UIParam param = null)
    {
        gameObject.SetActive(true);
        isOpen = true;
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }
}
