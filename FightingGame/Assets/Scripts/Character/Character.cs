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
    protected bool isInitialized = false;
    protected bool isServerSyncState = false;

    public override void Init()
    {
        base.Init();

        isInitialized = true;

        isServerSyncState = Managers.Network.isServerSyncState;

        Managers.Data.CharInfoDict.TryGetValue((int)characterType, out CharacterInfo characterInfo);
        MyCharInfo = characterInfo;

        currHP = MyCharInfo.maxHP;

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

        currState = ENUM_PLAYER_STATE.Idle;
    }

    public virtual void Move(CharacterParam param)
    {
        if (param == null || param is CharacterMoveParam == false)
            return;

        currState = ENUM_PLAYER_STATE.Move;

        var moveParam = param as CharacterMoveParam;

        rigid2D.velocity = new Vector2(moveParam.moveDir * MyCharInfo.moveSpeed, rigid2D.velocity.y);
    }

    public virtual void Jump()
    {
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

    [BroadcastMethod]
    public void Set_Sound()
    {
        AudioSource audioSource = this.transform.Find("Sound").GetComponent<AudioSource>();
        if (teamType == ENUM_TEAM_TYPE.Blue)
            Managers.Sound.Set_TeamAudioSource(audioSource, ENUM_SOUND_TYPE.SFX_BLUE);
        else if (teamType == ENUM_TEAM_TYPE.Red)
            Managers.Sound.Set_TeamAudioSource(audioSource, ENUM_SOUND_TYPE.SFX_RED);
        else
            return;
    }
}