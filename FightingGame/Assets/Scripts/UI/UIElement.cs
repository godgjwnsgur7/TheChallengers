using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 해당하는 씬에서만 존재할 UI 속성들
/// </summary>
public abstract class UIElement : MonoBehaviour
{

    public abstract void Open();
    public abstract void Close();
}
