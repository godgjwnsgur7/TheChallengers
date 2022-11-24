using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class InputSkillKey : InputKey
{
    float coolTime;

    public override void Init(Action<ENUM_INPUTKEY_NAME> _OnPointDownCallBack, Action<ENUM_INPUTKEY_NAME> _OnPointUpCallBack)
    {
        base.Init(_OnPointDownCallBack, _OnPointUpCallBack);

        Skill _skill = new Skill();
        Managers.Data.SkillDict.TryGetValue(inputKeyNum, out _skill);

        // coolTime = _skill.
    }
}
