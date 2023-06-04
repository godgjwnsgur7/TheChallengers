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
        if (inputKeyManagement == null)
        {
            inputKeyManagement = Managers.UI.popupCanvas.Get_InputKeyManagement();
        }

        return inputKeyManagement;
    }

    public void Connect_InputKeyController(ENUM_CHARACTER_TYPE _charType, Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (inputKeyController == null)
        {
            inputKeyController = Managers.UI.popupCanvas.Get_InputKeyController();
        }

        inputKeyController.Open(_charType, _OnPointDownCallBack, _OnPointUpCallBack);
    }

    public void Connect_InputArrowKey(Action<float> _OnPointEnterCallBack)
    {
        if (inputKeyController == null)
        {
            inputKeyController = Managers.UI.popupCanvas.Get_InputKeyController();
        }

        inputKeyController.Connect_InputArrowKey(_OnPointEnterCallBack);
    }

    public void Deactive_InputKeyController()
    {
        if (inputKeyController == null)
        {
            inputKeyController = Managers.UI.popupCanvas.Get_InputKeyController();
        }

        inputKeyController.Close();
    }
    public void Destroy_InputKeyManagement() => Managers.Resource.Destroy(inputKeyManagement.gameObject);

    public void Notify_UseSkill(int skillNum) => inputKeyController.Notify_UseSkill(skillNum);

    public void Clear()
    {
        if(inputKeyController != null && inputKeyController.gameObject.activeSelf)
            inputKeyController.Close();

        if(inputKeyManagement != null && inputKeyManagement.gameObject.activeSelf)
            inputKeyManagement.Close();
    }
}