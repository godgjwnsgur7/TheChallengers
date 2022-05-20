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

        // playerAnim = new PlayerAnimation();

        // 디버그용
        playerAnim = GetComponent<PlayerAnimation>();
        playerAnim.SetInteger("WeaponType", 2); // 검 들고 시작
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);
        
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

        playerAnim.SetTrigger("AttackTrigger");
    }

    public override void Expression(CharacterParam param)
    {
        base.Expression(param);


    }

    public override void Hit(CharacterParam param)
    {
        base.Hit(param);

        playerAnim.SetTrigger("HitTrigger");
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }
}