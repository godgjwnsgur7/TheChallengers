using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class Character : MonoBehaviourPhoton
{
    public Rigidbody2D rigid2D;
    public Collider Col
    {
        get;
        protected set;
    }


    [SerializeField] protected float hp;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    public ENUM_CHARACTER_TYPE characterType;
    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;
    public ENUM_TEAM_TYPE teamType;

    // 테스트 편의성을 위해 public
    public bool reverseState = false;
    public bool jumpState = false;
    public bool invincibility = false;
    public bool attackState = false;
    public bool hitCoroutine = false;

    public override void Init()
    {
        base.Init();

        rigid2D = GetComponent<Rigidbody2D>();

        SyncPhysics(rigid2D);
        SyncTransformView(transform);

    }

    protected override void OnMasterSerializeView(PhotonWriteStream stream)
    {
        base.OnMasterSerializeView(stream);

        stream.Write(characterType);
        Debug.Log($"{characterType} Write 성공");
    }

    protected override void OnSlaveSerializeView(PhotonReadStream stream)
    {
        base.OnSlaveSerializeView(stream);

        characterType = (ENUM_CHARACTER_TYPE)stream.Read();
        Debug.Log($"{characterType} Read 성공");
    }

    public virtual void Idle(CharacterParam param = null)
    {
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
        currState = ENUM_PLAYER_STATE.Idle;


    }

    public virtual void Move(CharacterParam param)
    {
        if (param == null || param is CharacterMoveParam == false)
            return;

        currState = ENUM_PLAYER_STATE.Move;
        var moveParam = param as CharacterMoveParam;

        rigid2D.velocity = new Vector2(moveParam.moveDir * moveSpeed, rigid2D.velocity.y);
    }

    public virtual void Jump()
    {
        rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);

    }

    public virtual void Attack(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Attack;
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
    }

    public virtual void Skill(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Skill;
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
    }

    public virtual void Hit(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Hit;
        rigid2D.velocity = Vector2.zero;
    }

    public virtual void Die(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Die;
        invincibility = true;
    }

}