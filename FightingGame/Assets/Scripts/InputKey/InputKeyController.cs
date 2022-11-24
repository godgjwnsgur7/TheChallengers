using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyController : MonoBehaviour
{
    public InputPanel inputPanel = null;

    public void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack, Action<ENUM_INPUTKEY_NAME> _OnDragCallBack)
    {
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(_OnPointDownCallBack, _OnPointUpCallBack, _OnDragCallBack);
        }
    }

}