using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;

    [SerializeField] GameObject popupWindow;

    private void OnEnable()
    {
        isUsing = true;
        Managers.UI.Push_WindowExitStack(OnClick_Exit);
    }

    private void OnDisable()
    {
        isUsing = false;
        Managers.UI.Pop_WindowExitStack();
    }

    public virtual void OnClick_Exit() { }
}
