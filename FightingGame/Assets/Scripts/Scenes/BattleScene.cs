using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        Managers.Resource.GenerateInPool("TestPrefab", 20); // 테스트 코드
    }

    public override void Clear()
    {

    }
}