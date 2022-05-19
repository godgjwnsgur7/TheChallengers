using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class ActiveCharacter : Character
{
    public override void Init()
    {
        base.Init();

        
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

    }

    public override void Move(CharacterParam param)
    {
        base.Move(param);

        if (param == null) return;

        var moveParam = param as CharacterMoveParam;

        if (moveParam != null)
        {
        }
    }

    public override void Attack(CharacterParam param)
    {
        base.Attack(param);

        if (param == null) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
        }
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        base.Hit(param);


    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }
}