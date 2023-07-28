using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyController : MonoBehaviour
{
    InputPanel inputPanel = null;

    public void Open(ENUM_CHARACTER_TYPE _charType, Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        Init(_charType, _OnPointDownCallBack, _OnPointUpCallBack);
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (inputPanel != null)
            Managers.Resource.Destroy(inputPanel.gameObject);

        gameObject.SetActive(false);
    }

    public void Init(ENUM_CHARACTER_TYPE _charType ,Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (inputPanel != null)
            Managers.Resource.Destroy(inputPanel.gameObject);
            
        inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
        
        inputPanel.Init(_OnPointDownCallBack, _OnPointUpCallBack);
        inputPanel.Set_InputSkillKeys(_charType);
    }
    
    public void Set_InputKeyPos()
    {
        inputPanel.Set_InputKeyPos();
    }

    public void Connect_InputArrowKey(Action<float> _OnPointEnterCallBack)
    {
        InputArrowKey inputArrowKey = inputPanel.transform.Find(ENUM_INPUTKEY_NAME.Direction.ToString()).GetComponent<InputArrowKey>();
        
        if(inputArrowKey == null)
        {
            Debug.LogError("inputArrowKey is Null!!");
            return;
        }

        inputArrowKey.Connect_Player(_OnPointEnterCallBack);
    }

    public void Notify_UseSkill(int skillNum) => inputPanel.Notify_UseSkill(skillNum);
}