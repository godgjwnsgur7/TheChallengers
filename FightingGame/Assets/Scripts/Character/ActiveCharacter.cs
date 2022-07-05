using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    [SerializeField] protected Skill[] skills = new Skill[3];

    public AttackObejct attackObject;

    public override void Init()
    {
        base.Init();

        spriteRenderer = GetComponent<SpriteRenderer>();

        // Animator
        anim = GetComponent<Animator>();
        if (anim == null) gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = Managers.Resource.GetAnimator(characterType);
    }

    public override void Idle(CharacterParam param = null)
    {
        base.Idle(param);

        if (anim.GetBool("IsMove"))
            anim.SetBool("IsMove", false);
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
            if(!anim.GetBool("IsMove"))
                anim.SetBool("IsMove", true);

            SetAnimParamVector(moveParam.moveDir);
        }
    }

    public override void Jump()
    {
        if (jumpState || currState == ENUM_PLAYER_STATE.Attack)
            return;

        base.Jump();

        anim.SetTrigger("JumpTrigger");
    }

    public override void Attack(CharacterParam param)
    {
        if(currState == ENUM_PLAYER_STATE.Attack)
            return;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("AttackTrigger");

            attackObject = Managers.Resource.Instantiate("AttackObejcts/AttackObjectSample").GetComponent<AttackObejct>();
            // attackObject.ActivatingAttackObject(attackParam);                
            
        }

       
    }

    public override void Hit(CharacterParam param)
    {

        if (param == null || invincibility) return;

        base.Hit(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("HitTrigger");

            hp -= attackParam.damage;
        }
    }

    public override void Die(CharacterParam param = null)
    {
        base.Die(param);


    }

    public void SetAnimParamVector(float _moveDir)
    {
        ReverseSprites(_moveDir);

        anim.SetFloat("DirX", _moveDir);
    }

    private void ReverseSprites(float vecX)
    {
        bool _reverseState = (vecX < 0.9f);

        if (reverseState == _reverseState)
            return;

        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    // 요기 밑에를 바꿀거야 알겠찌
    // 코루틴으로 돌릴거야 돌린 다음...
    // 코루틴에서 레이를 쏠거야 몰라 그럴거야 알겠어?
    // 만약 하강상태면 바로 넘어가겠지 알아서 ㅋ 몰라 싀발 꺼져
    // 코루틴에서 확인할 것 -> 

    public void SetJumpState(bool _jumpState)
    {
        if (jumpState == _jumpState) return;

        jumpState = _jumpState;
        anim.SetBool("IsJump", jumpState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ENUM_TAG_TYPE.Ground.ToString())
            SetJumpState(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ENUM_TAG_TYPE.Ground.ToString())
            SetJumpState(true);
    }
}