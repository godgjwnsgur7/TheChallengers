using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    public Skill skill;

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        Managers.Data.SkillDict.TryGetValue(0, out skill);


    }

    public override void Clear()
    {

    }
}