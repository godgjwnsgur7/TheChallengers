using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        Managers.Resource.Instantiate("TestPrefab");
    }

    public override void Clear()
    {

    }
}