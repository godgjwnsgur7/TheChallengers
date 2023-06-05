using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class InputKeyArea : InputBasicKey
{
    [SerializeField] Image areaImage;

    public override void EventTrigger_PointerDown()
    {
        areaImage.color = new Color(1, 1, 1, 1);
        OnPointDownCallBack?.Invoke((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public override void EventTrigger_PointerUp()
    {
        OnPointUpCallBack?.Invoke((ENUM_INPUTKEY_NAME)inputKeyNum);
    }

    public void Reset_InputKeyData(InputKey inputKey)
    {

    }

    public void Deactive_AreaImage()
    {
        areaImage.color = new Color(1, 1, 1, 0);
    }
}
