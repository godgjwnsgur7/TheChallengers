using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIParam { }

public class UIPopupParam : UIParam
{
    public Vector2 pos;

    public UIPopupParam(Vector2 _pos)
    {
        pos = _pos;
    }
}