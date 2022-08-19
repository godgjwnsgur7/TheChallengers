using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanel : UIElement
{
    [SerializeField] Button slideBtn;

    public override void Open(UIParam param = null)
    {
        base.Open(param);
    }

    public override void Close()
    {
        base.Close();
    }

    public void InteractableBtn()
    {
        slideBtn.interactable = !slideBtn.interactable;
    }
}
