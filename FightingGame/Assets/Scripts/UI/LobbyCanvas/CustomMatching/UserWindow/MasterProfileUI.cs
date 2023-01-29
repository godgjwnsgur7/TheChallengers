using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MasterProfileUI : BaseProfile
{
    public override void Init()
    {

        base.Init();

    }

    public override void Clear()
    {
        base.Clear();
    }

    public bool Get_IsSelectedChar() => currCharType != ENUM_CHARACTER_TYPE.Default;
}
