using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBasicKey : InputKey
{
    private void Start()
    {
        iconImage.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    [SerializeField] protected Image iconImage;

    public override void Set_Transparency(float _opacity)
    {
        Color changeColor = iconImage.color;
        changeColor.a = _opacity * 1.5f;
        iconImage.color = changeColor;

        base.Set_Transparency(_opacity);
    }
}
