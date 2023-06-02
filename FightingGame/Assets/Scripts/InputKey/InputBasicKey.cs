using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBasicKey : InputKey
{
    [SerializeField] protected Image iconImage;

    public override void Set_Transparency(float _opacity)
    {
        Color changeColor = iconImage.color;
        changeColor.a = _opacity;
        iconImage.color = changeColor;

        base.Set_Transparency(_opacity);
    }
}
