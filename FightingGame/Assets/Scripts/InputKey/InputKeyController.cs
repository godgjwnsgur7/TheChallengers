using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class InputKeyController : MonoBehaviour
{
    InputPanel inputPanel = null;

    public void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        if (inputPanel == null)
        {
            inputPanel = Managers.Resource.Instantiate("UI/InputPanel", this.transform).GetComponent<InputPanel>();
            inputPanel.Init(_OnPointDownCallBack, _OnPointUpCallBack);

            inputPanel.Set_InputSkillKeys(Managers.Battle.Get_MyCharacterType());
        }
    }

    public void Notify_UseSkill(int skillNum) => inputPanel.Notify_UseSkill(skillNum);
}