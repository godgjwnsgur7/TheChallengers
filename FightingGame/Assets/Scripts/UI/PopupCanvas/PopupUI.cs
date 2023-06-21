using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;

    [SerializeField] GameObject popupWindow;

    protected virtual void OnEnable()
    {
        isUsing = true;
        Managers.UI.Push_WindowExitStack(OnClick_Exit);
    }

    protected virtual void OnDisable()
    {
        isUsing = false;
        Managers.UI.Pop_WindowExitStack();
    }

    public virtual void OnClick_Exit() { }
    public void OnClick_SoundSFX(int sfxTypeNum)
    {
        Managers.Sound.Play_SFX((FGDefine.ENUM_SFX_TYPE)sfxTypeNum);
    }
}
