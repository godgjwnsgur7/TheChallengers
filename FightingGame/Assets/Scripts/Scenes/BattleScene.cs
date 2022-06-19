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
        Managers.Resource.GenerateInPool("Bullet/Gun", 20);
        Managers.Resource.GenerateInPool("Bullet/Rifle", 20);
        Managers.Resource.GenerateInPool("Bullet/Bow", 20);
    }

    public override void Clear()
    {

    }
}