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

    public Vector2 dirVec = Vector2.down;
    public ENUM_CHARACTER_TYPE characterType;
    public ENUM_WEAPON_TYPE weaponType = ENUM_WEAPON_TYPE.Null;
    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;


    public float jumpPower = 10f;

    public InteractableObject GetForwardObjectWithRay()
    {
        Vector2 CriteriaPos = rigid2D.position + new Vector2(0, 0.5f);

        Debug.DrawRay(CriteriaPos, dirVec * 1f, Color.green);
        RaycastHit2D rayHit;

        rayHit = Physics2D.Raycast(CriteriaPos, dirVec, 1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Interaction.ToString()));
        if (rayHit.collider != null)
            return rayHit.collider.gameObject.GetComponent<InteractableObject>();

        return null;
    }

    public override void Init()
    {
        base.Init();

        rigid2D = GetComponent<Rigidbody2D>();

        if(PhotonLogicHandler.IsMine(this))
        {
            Debug.Log("컨트롤이 가능한 객체");
        }   
        else
        {
            Debug.Log("컨트롤이 불가능한 객체");
        }
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

        rigid2D.velocity = new Vector2(moveParam.inputVec.x * 10f, rigid2D.velocity.y);
    }

    public virtual void Jump()
    {
        // currState = ENUM_PLAYER_STATE.Jump;

        rigid2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public virtual void Attack(CharacterParam param)
    {

        rigid2D.velocity = Vector2.zero;
        currState = ENUM_PLAYER_STATE.Attack;
    }

    public virtual void Expression(CharacterParam param)
    {
        rigid2D.velocity = Vector2.zero;
        // 감정표현
    }
    
    public virtual void Hit(CharacterParam param)
    {
        rigid2D.velocity = Vector2.zero;
        currState = ENUM_PLAYER_STATE.Hit;
    }

    public virtual void Die(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Die;
    }
}