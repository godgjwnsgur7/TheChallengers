using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class TrainingScene : BaseScene
{
    [SerializeField] ButtonPanel buttonPanel;

    public override void Clear()
    {
        base.Clear();
    }

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Training;
        
        base.Init();

        buttonPanel.Init();
    }

    public override void Play_BGM()
    {
        Managers.Sound.Play_BGM(ENUM_BGM_TYPE.Test);
    }
}
