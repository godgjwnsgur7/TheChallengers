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

        // 디버그용
        playerAnim = GetComponent<PlayerAnimation>();
        playerAnim.SetInteger("WeaponType", (int)weaponType);

        // playerAnim = new PlayerAnimation();
        playerAnim.Init(characterType);
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        playerAnim.SetIdle();

        if (playerAnim.GetBool("isMove"))
            playerAnim.SetBool("isMove", false);
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
            playerAnim.SetVector(moveParam.inputVec, moveParam.isRun);

            if(!playerAnim.GetBool("isMove"))
                playerAnim.SetBool("isMove", true);
        }
    }

    public override void Attack(CharacterParam param)
    {
        if (weaponType == ENUM_WEAPON_TYPE.Null ||
            currState == ENUM_PLAYER_STATE.Hit)
            return;

        base.Attack(param);

        if ((int)weaponType > 3) // 원거리 무기일 경우 위탁
            playerAnim.SetSprite(weaponType);

        playerAnim.SetTrigger("AttackTrigger");
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
            playerAnim.SetTrigger("HitTrigger");
        }
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }
}