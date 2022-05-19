using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class ActiveCharacter : Character
{
    public PlayerAnimation playerAnim;

    public override void Init()
    {
        base.Init();
        playerAnim = GetComponent<PlayerAnimation>();
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);
        
        Debug.Log("Idle");
        // if (playerAnim.GetBool("isMove"))
            playerAnim.SetBool("isMove", false);
    }

    public override void Move(CharacterParam param)
    {
        base.Move(param);

        if (param == null) return;

        var moveParam = param as CharacterMoveParam;

        if (moveParam != null)
        {
            playerAnim.SetFloat("DirX", moveParam.inputVec.x);
            playerAnim.SetFloat("DirY", moveParam.inputVec.y);

            Debug.Log("Move");
            // if(!playerAnim.GetBool("isMove"))
            playerAnim.SetBool("isMove", true);
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