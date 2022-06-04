using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    public override void Init()
    {
        base.Init();

        if (characterType == ENUM_CHARACTER_TYPE.Default)
            characterType = ENUM_CHARACTER_TYPE.Knight;

        SetObjectInfo(characterType);
        SetSpriteOrderLayer(Vector2.zero);

        // 디버그용
        SetInteger("WeaponType", (int)weaponType);
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        if (GetBool("isMove"))
            SetBool("isMove", false);

        if (GetBool("isAttack")) 
            SetBool("isAttack", false);

        if (GetBool("isHit"))
            SetBool("isHit", false);
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
            SetVector(moveParam.inputVec, moveParam.isRun);

            if(!GetBool("isMove"))
                SetBool("isMove", true);

            dirVec = moveParam.inputVec.normalized;
        }
    }

    public override void Attack(CharacterParam param)
    {
        if (weaponType == ENUM_WEAPON_TYPE.Null)
            return;

        if(currState == ENUM_PLAYER_STATE.Attack||
            currState == ENUM_PLAYER_STATE.Hit)
            return;

        base.Attack(param);

        SetBool("isAttack", true);
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        base.Hit(param);

        if (param == null) return;

        var hitParam = param as CharacterHitParam;

        if (hitParam != null)
        {
            SetBool("isHit", true);
        }
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }
}