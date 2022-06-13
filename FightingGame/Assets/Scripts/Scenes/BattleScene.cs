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
        Managers.Resource.GenerateInPool("GunBullet", 20);
        Managers.Resource.GenerateInPool("RifleBullet", 20);
        Managers.Resource.GenerateInPool("BowArrow", 20);
    }

    public override void Clear()
    {

    }
}