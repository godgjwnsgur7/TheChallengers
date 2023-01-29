using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingScene : BaseScene
{
    public override void Clear()
    {
        base.Clear();
    }

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Training;
        
        base.Init();
    }
}
