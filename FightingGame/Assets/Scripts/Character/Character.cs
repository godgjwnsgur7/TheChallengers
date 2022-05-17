using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class Character : MonoBehaviour
{
    public Rigidbody2D rigid2D;
    public Collider Col
    {
        get;
        protected set;
    }

    public ENUM_CHARACTER_TYPE characterType;
    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;

    public virtual void Init()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        
    }

    public virtual void Idle(CharacterParam param = null)
    {
        currState = ENUM_PLAYER_STATE.Idle;
        rigid2D.velocity = Vector2.zero;
    }

    public virtual void Move(CharacterParam param)
    {
        if (param == null || param is CharacterMoveParam == false)
            return;

        currState = ENUM_PLAYER_STATE.Move;
        var moveParam = param as CharacterMoveParam;

        Vector3 direction = transform.up * moveParam.inputVec.y + transform.right * moveParam.inputVec.x ;
        transform.position += direction * moveParam.speed * Time.deltaTime;
    }
   
    public virtual void Attack(CharacterParam param)
    {
        if (param == null || param is CharacterAttackParam == false)
            return;

        currState = ENUM_PLAYER_STATE.Attack;
    }

    public virtual void Expression(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Expression;
    }

    public virtual void Die(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Die;
    }
}