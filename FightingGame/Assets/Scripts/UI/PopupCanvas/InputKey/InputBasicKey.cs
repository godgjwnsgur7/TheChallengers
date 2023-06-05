using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class InputBasicKey : InputKey
{
    private void Start()
    {
        AlphaArea_TouchRestriction();
    }

    [SerializeField] protected Image iconImage;

    protected virtual void AlphaArea_TouchRestriction()
    {
        if (inputKeyNum == 0)
            return;

        slotImage.alphaHitTestMinimumThreshold = 0.1f;
        iconImage.alphaHitTestMinimumThreshold = 0.1f;
    }

    public void ChangeSet_IconImage(string fileName)
    {
        if (iconImage != null)
            iconImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/InputKey/{fileName}");
    }

    public override void Set_Transparency(float _opacity)
    {
        if(iconImage != null)
        {
            Color changeColor = iconImage.color;
            changeColor.a = _opacity * 1.3f;
            iconImage.color = changeColor;
        }

        base.Set_Transparency(_opacity);
    }
}
