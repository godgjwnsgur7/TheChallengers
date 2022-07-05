using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Knight : ActiveCharacter
{

    public override void Init()
    {
        base.Init();

        characterType = ENUM_CHARACTER_TYPE.Knight;
    }
}
