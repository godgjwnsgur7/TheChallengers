using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class DebugScene : BaseScene
{
    public override void Clear()
    {

    }

    public override void Update_BGM()
    {
        Managers.Sound.Play(ENUM_BGM_TYPE.TestBGM, ENUM_SOUND_TYPE.BGM);
    }
}
