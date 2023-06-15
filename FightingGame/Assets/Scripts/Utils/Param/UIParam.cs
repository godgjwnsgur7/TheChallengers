using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class UIParam { }

public class WindowExitUIParam : UIParam
{
    public Action onWindowExit;
    public GameObject windowObject;
    public string windowNameStr;

    public WindowExitUIParam(Action _onWindowExit, GameObject _windowObject)
    {
        onWindowExit = _onWindowExit;
        windowObject = _windowObject;
        windowNameStr = _windowObject.name;
    }
}
