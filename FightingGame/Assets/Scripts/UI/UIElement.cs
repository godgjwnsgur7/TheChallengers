using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 뒤로가기 버튼 작동 위한 클래스
/// </summary>
public class UIElement : MonoBehaviour
{
    public bool isOpen = false;

    protected virtual void OnEnable()
    {
        isOpen = true;
        Managers.UI.Push_WindowExitStack(OnClick_Exit);
    }

    protected virtual void OnDisable()
    {
        isOpen = false;
        Managers.UI.Pop_WindowExitStack();
    }

    public virtual void OnClick_Exit() { }

    public void OnClick_SoundSFX(int sfxTypeNum)
    {
        Managers.Sound.Play_SFX((FGDefine.ENUM_SFX_TYPE)sfxTypeNum);
    }
}
