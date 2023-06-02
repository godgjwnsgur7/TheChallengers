using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBasicKey : InputKey
{
    private void Start()
    {
        AlphaArea_TouchRestriction();
    }

    [SerializeField] protected Image iconImage;

    protected virtual void AlphaArea_TouchRestriction()
    {
        slotImage.alphaHitTestMinimumThreshold = 0.1f;
        iconImage.alphaHitTestMinimumThreshold = 0.1f;
    }

    public override void Set_Transparency(float _opacity)
    {
        Color changeColor = iconImage.color;
        changeColor.a = _opacity;
        iconImage.color = changeColor;

        base.Set_Transparency(_opacity);
    }
}
