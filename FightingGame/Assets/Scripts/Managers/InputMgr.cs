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
            inputKeyManagement = Managers.Resource.Instantiate("UI/@InputKeyManagement", Managers.UI.currCanvas.transform).GetComponent<InputKeyManagement>();

        return inputKeyManagement;
    }

    public void Connect_InputKeyController(ENUM_CHARACTER_TYPE _charType, Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (inputKeyManagement != null)
        {
            Managers.Resource.Destroy(inputKeyManagement.gameObject);
            inputKeyManagement = null;
        }

        if (inputKeyController == null)
        {
            inputKeyController = Managers.Resource.Instantiate("UI/InputKeyController", Managers.UI.currCanvas.transform).GetComponent<InputKeyController>();
            inputKeyController.transform.SetSiblingIndex(1); // 임시
        }

        inputKeyController.Init(_charType, _OnPointDownCallBack, _OnPointUpCallBack);
    }

    public void Connect_InputArrowKey(Action<float> _OnPointEnterCallBack)
    {
        if (inputKeyController == null)
            inputKeyController = Managers.Resource.Instantiate("UI/InputKeyController", Managers.UI.currCanvas.transform).GetComponent<InputKeyController>();

        inputKeyController.Connect_InputArrowKey(_OnPointEnterCallBack);
    }

    public void Destroy_InputKeyController()
    {
        if(inputKeyController != null)
        {
			Managers.Resource.Destroy(inputKeyController.gameObject);
		}
    }
    public void Destroy_InputKeyManagement() => Managers.Resource.Destroy(inputKeyManagement.gameObject);

    public void Notify_UseSkill(int skillNum) => inputKeyController.Notify_UseSkill(skillNum);

    public void Clear()
    {
        if (inputKeyController != null)
        {
            Managers.Resource.Destroy(inputKeyController.gameObject);
            inputKeyController = null;
        }

        if (inputKeyManagement != null)
        {
            Managers.Resource.Destroy(inputKeyManagement.gameObject);
            inputKeyManagement = null;
        }
    }
}