using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class InputMgr
{
    InputKeyController inputKeyController = null;
    InputKeyManagement inputKeyManagement = null;
   
    public InputKeyManagement Get_InputKeyManagement()
    {
        if (inputKeyController != null)
        {
            Managers.Resource.Destroy(inputKeyController.gameObject);
            inputKeyController = null;
        }

        if (inputKeyManagement == null)
        {
            inputKeyManagement = Managers.Resource.Instantiate("UI/InputKeyManagement", Managers.UI.currCanvas.transform).GetComponent<InputKeyManagement>();
        }

        return inputKeyManagement;
    }

    public void Connect_InputKeyController(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (inputKeyManagement != null)
        {
            Managers.Resource.Destroy(inputKeyManagement.gameObject);
            inputKeyManagement = null;
        }

        if (inputKeyController == null)
            inputKeyController = Managers.Resource.Instantiate("UI/InputKeyController", Managers.UI.currCanvas.transform).GetComponent<InputKeyController>();

        inputKeyController.Init(_OnPointDownCallBack, _OnPointUpCallBack);
        
    }

    public void Clear()
    {
        inputKeyController = null;
        inputKeyManagement = null;
    }
}