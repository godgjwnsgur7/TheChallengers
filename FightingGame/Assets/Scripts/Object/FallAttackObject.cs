using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 낙하해서 충돌하면 이펙트 발생
public class FallAttackObject : AttackObject
{
    Animator anim;

    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Fall;

        base.Init();

    }
}
