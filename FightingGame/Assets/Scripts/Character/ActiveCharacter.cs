using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    public bool reverseState = false;

    public override void Init()
    {
        base.Init();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Animator
        anim = GetComponent<Animator>();
        if (anim == null) gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = Managers.Resource.GetAnimator(characterType);
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        if (anim.GetBool("IsMove"))
            anim.SetBool("IsMove", false);
    }

    public override void Move(CharacterParam param)
    {
        if (currState != ENUM_PLAYER_STATE.Idle && 
            currState != ENUM_PLAYER_STATE.Move)
            return;

        base.Move(param);

        if (param == null) return;

        var moveParam = param as CharacterMoveParam;

        if (moveParam != null)
        {
            if(!anim.GetBool("IsMove"))
                anim.SetBool("IsMove", true);

            SetAnimParamVector(moveParam.moveDir);
        }
    }

    public override void Jump()
    {
        if (currState == ENUM_PLAYER_STATE.Jump)
            return;

        base.Jump();


    }

    public override void Attack(CharacterParam param)
    {
        if(currState == ENUM_PLAYER_STATE.Attack)
            return;

        base.Attack(param);

        anim.SetTrigger("AttackTrigger");
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        base.Hit(param);

        if (param == null) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {

        }
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }

    public void SetAnimParamVector(float moveDir)
    {
        ReverseSprites(moveDir);

        anim.SetFloat("DirX", moveDir);
    }

    private void ReverseSprites(float vecX)
    {
        bool _reverseState = (vecX < 0.9f);

        if (reverseState == _reverseState)
            return;

        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }
}