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

    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        if (GetBool("isMove"))
            SetBool("isMove", false);
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

        if (GetInteger("WeaponType") <= 3) 
        {
            weaponSetting.enabledWeapon(dirVec);
        }
        else if (GetInteger("WeaponType") > 3)
        {
            weaponSetting.SetEffectPosition(dirVec);
        }
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        if (GetBool("isAttack"))
            SetBool("isAttack", false);

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

    public void SetWeapon(ENUM_WEAPON_TYPE _weaponType)
    {
        weaponType = _weaponType;
        SetInteger("WeaponType", (int)weaponType);

        if (GetInteger("WeaponType") <= 3)
        {
            weaponSetting.SetWeaponCollider2D(weaponType);
        }
        else if (GetInteger("WeaponType") > 3)
        {
            weaponSetting.SetWeaponEffect(weaponType);
        }
    }
}