using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class UIParam { }

public class PopupUIParam : UIParam
{
    public Vector2 pos;

    public PopupUIParam(Vector2 _pos)
    {
        pos = _pos;
    }
}

public class BattleCanvasUIParam : UIParam
{


    public BattleCanvasUIParam()
    {

    }
}
