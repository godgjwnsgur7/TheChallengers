using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIMgr
{
    public void Init()
    {
    }

    public T GetUI<T>() where T : BaseUI
    {

        return null;
    }

    public void OpenUI<T>(UIParam param = null) where T : BaseUI
    {
        T window = GetUI<T>();

        
    }

    public void CloseUI<T>() where T : BaseUI
    {
        T window = GetUI<T>();

    }
}
