using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class Character : MonoBehaviourPhoton
{
    protected Rigidbody2D rigid2D;
    public Collider Col
    {
        get;
        protected set;
    }

    public CharacterInfo MyCharInfo
    {
        get;
        protected set;
    }

    public float currHP
    {
        get;
        protected set;
    }

    public ENUM_CHARACTER_TYPE characterType
    {
        get;
        protected set;
    }

    public ENUM_TEAM_TYPE teamType
    {
        get;
        protected set;
    }

    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;

    public bool reverseState = false;
    public bool isControl = false;

    public bool jumpState = false;
    public bool invincibility = false;
    public bool superArmour = false;

    protected float inputArrowDir = 0.0f;
    private bool isInitialized = false;
    protected bool isServerSyncState = false;

    public override void Init()
    {
        base.Init();

        if (isInitialized)
            return;

        isInitialized = true;

        isServerSyncState = Managers.Network.isServerSyncState;

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if(isServerSyncState)
        {
            SyncPhysics(rigid2D);
            SyncTransformView(transform);
        }
    }
    
    protected override void OnMineSerializeView(PhotonWriteStream stream)
    {
        stream.Write(currHP);
        stream.Write(currState);
        stream.Write(teamType);
        stream.Write(jumpState);
        stream.Write(invincibility);
        stream.Write(superArmour);

        base.OnMineSerializeView(stream);
    }

    protected override void OnOtherSerializeView(PhotonReadStream stream)
    {
        currHP = stream.Read<float>();
        currState = stream.Read<ENUM_PLAYER_STATE>();
        teamType = stream.Read<ENUM_TEAM_TYPE>();
        jumpState = stream.Read<bool>();
        invincibility = stream.Read<bool>();
        superArmour = stream.Read<bool>();

        base.OnOtherSerializeView(stream);
    }

    public virtual void Idle()
    {
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);

        if(!jumpState)
            currState = ENUM_PLAYER_STATE.Idle;
        else if(currState != ENUM_PLAYER_STATE.Jump)
            currState = ENUM_PLAYER_STATE.Jump;
    }

    public virtual void Move(CharacterParam param)
    {
        if(currState != ENUM_PLAYER_STATE.Jump)
            currState = ENUM_PLAYER_STATE.Move;

        var moveParam = param as CharacterMoveParam;

        rigid2D.velocity = new Vector2(moveParam.moveDir * MyCharInfo.moveSpeed, rigid2D.velocity.y);
    }

    public virtual void Jump()
    {
        currState = ENUM_PLAYER_STATE.Jump;

        rigid2D.AddForce(Vector2.up * MyCharInfo.jumpPower, ForceMode2D.Impulse);
    }

    public virtual void Dash()
    {
        currState = ENUM_PLAYER_STATE.Dash;

        rigid2D.velocity = Vector2.zero;
        rigid2D.AddForce(Vector2.right * MyCharInfo.jumpPower * (reverseState? -1.0f : 1.0f), ForceMode2D.Impulse);
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

    public virtual void Die()
    {
        currState = ENUM_PLAYER_STATE.Die;

        invincibility = true;
    }

    protected void Push_Rigid2D(Vector2 vec)
    {
        if(rigid2D == null)
        {
            Debug.Log("rigid2D is Null");
            return;
        }

        rigid2D.velocity = new Vector2(0, rigid2D.velocity.y); // 받고있는 힘 초기화
        rigid2D.AddForce(vec, ForceMode2D.Impulse);
    }

    public void ValueClear_Rigid() => rigid2D.velocity = Vector2.zero;
}