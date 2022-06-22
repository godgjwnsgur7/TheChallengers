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

        if (characterType == ENUM_CHARACTER_TYPE.Default)
            characterType = ENUM_CHARACTER_TYPE.Knight;

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Animator
        anim = GetComponent<Animator>();
        if (anim == null) gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = Managers.Resource.GetAnimator(characterType);
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        if (anim.GetBool("isMove"))
            anim.SetBool("isMove", false);
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
            if(!anim.GetBool("isMove"))
                anim.SetBool("isMove", true);

            dirVec = moveParam.inputVec.normalized;
            SetAnimParamVector(dirVec, moveParam.isRun);
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
        if (weaponType == ENUM_WEAPON_TYPE.Null)
            return;

        if(currState == ENUM_PLAYER_STATE.Attack||
            currState == ENUM_PLAYER_STATE.Hit)
            return;

        base.Attack(param);

        anim.SetBool("isAttack", true);

    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        if (anim.GetBool("isAttack"))
            anim.SetBool("isAttack", false);

        base.Hit(param);

        if (param == null) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetBool("isHit", true);
        }
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }

    public void SetAnimParamVector(Vector2 vec, bool isRun)
    {
        ReverseSprites(vec.x);

        float f = isRun ? 2.0f : 1.0f;

        anim.SetFloat("DirX", vec.x * f);
        anim.SetFloat("DirY", vec.y * f);
    }

    public void SetWeapon(ENUM_WEAPON_TYPE _weaponType)
    {
        weaponType = _weaponType;
        anim.SetInteger("WeaponType", (int)weaponType);

    }

    private void ReverseSprites(float vecX)
    {
        bool _reverseState = (vecX > 0.9f);

        if (reverseState == _reverseState)
            return;

        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }
}