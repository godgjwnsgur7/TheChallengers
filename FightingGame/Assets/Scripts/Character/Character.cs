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

    private float moveDir = 0f;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;
    public ENUM_CHARACTER_TYPE characterType;
    public ENUM_PLAYER_STATE currState = ENUM_PLAYER_STATE.Idle;

    // 테스트 편의성을 위해 public
    public bool reverseState = false;
    public bool jumpState = false;


    public void GroundCheckWithRay()
    {
        if (rigid2D == null)
            return;


        Debug.DrawRay(rigid2D.position, Vector2.down * 1.1f, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(rigid2D.position, Vector2.down, 1.1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));
        
        if(rayHit.collider != null)
        {
            Debug.Log(rayHit.distance);
            
        }
        
    }

    private void Update()
    {
        GroundCheckWithRay();
    }

    public override void Init()
    {
        base.Init();

        rigid2D = GetComponent<Rigidbody2D>();

        if (PhotonLogicHandler.IsMine(this))
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

    public virtual void Skill()
    {

    }

    public virtual void Hit(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Hit;
        rigid2D.velocity = new Vector2(0f, rigid2D.velocity.y);
    }

    public virtual void Die(CharacterParam param)
    {
        currState = ENUM_PLAYER_STATE.Die;
    }

    public void Test()
    {
        Debug.DrawRay(rigid2D.position, Vector2.down, Color.green);
    }
}