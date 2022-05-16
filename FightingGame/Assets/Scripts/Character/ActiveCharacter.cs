using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class ActiveCharacter : Character
{
    public Animator anim;

    public override void Init()
    {
        base.Init();
        anim = GetComponent<Animator>();
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        anim.SetFloat("DirX", 0.0f);
        anim.SetFloat("DirY", 0.0f);
    }

    public override void Move(CharacterParam param)
    {
        base.Move(param);

        if (param == null) return;

        var moveParam = param as CharacterMoveParam;

        if (moveParam != null)
        {
            anim.SetFloat("DirX", moveParam.inputVec.x);
            anim.SetFloat("DirY", moveParam.inputVec.y);
        }
    }

    public override void Attack(CharacterParam param)
    {
        base.Attack(param);

        if (param == null) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("Attack");
        }
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }
}