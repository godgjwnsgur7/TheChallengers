using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Wizard : ActiveCharacter
{
    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Wizard;

        base.Init();

        if (PhotonLogicHandler.IsMine(viewID))
        {
            Debug.Log("컨트롤이 가능한 객체");
        }
        else
        {
            Debug.Log("컨트롤이 불가능한 객체");
        }
    }
}
