using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class Character : MonoBehaviour
{
    private Rigidbody rigid;
    public Collider Col
    {
        get;
        protected set;
    }

    private ENUM_CHARACTER_TYPE characterType;
    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;

    public virtual void Init()
    {
        rigid = GetComponent<Rigidbody>();
        
    }

    public virtual void Idle(CharacterParam param = null)
    {
        currState = ENUM_PLAYER_STATE.Idle;
        rigid.velocity = Vector2.zero;
    }

    public virtual void Move(CharacterParam param)
    {
        if (param == null ||
            param is CharacterMoveParam == false)
            return;

        var moveParam = param as CharacterMoveParam;

        currState = moveParam.isRun ? ENUM_PLAYER_STATE.Run : ENUM_PLAYER_STATE.Move;

        Vector2 direction = transform.forward * moveParam.inputVec.y + transform.right * moveParam.inputVec.x;
        rigid.velocity = direction;
    }
}