using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public ENUM_SKILL_TYPE[] skills = new ENUM_SKILL_TYPE[3];

    public AttackObejct attackObject;

    Coroutine coroutine;

    public override void Init()
    {
        base.Init();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(teamType == ENUM_TEAM_TYPE.Blue)
            spriteRenderer.flipX = true;

        // Animator
        anim = GetComponent<Animator>();
        if (anim == null) gameObject.AddComponent<Animator>();
        anim.runtimeAnimatorController = Managers.Resource.GetAnimator(characterType);

        SyncAnimator(anim);
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

            if(!GroundCheckWithRay())
            {
                anim.SetTrigger("DropTrigger");
            }
        }
    }

    public override void Jump()
    {
        if (jumpState || currState == ENUM_PLAYER_STATE.Attack)
            return;

        if (currState != ENUM_PLAYER_STATE.Idle &&
            currState != ENUM_PLAYER_STATE.Move)
            return;

        base.Jump();

        anim.SetTrigger("JumpTrigger");
    }

    public override void Attack(CharacterParam param)
    {
        if (attackObject != null)
            attackObject = null;
        base.Attack(param);
    }

    public override void Skill(CharacterParam param)
    {
        if (attackObject != null)
            attackObject = null;

        base.Skill(param);
    }

    public override void Hit(CharacterParam param)
    {
        if (param == null || invincibility) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            if(Managers.Data.SkillDict.TryGetValue((int)attackParam.attackType, out Skill _skillData))
            {
                base.Hit(param);
                anim.SetBool("IsHit", true);
                anim.SetTrigger("HitTrigger");
                hp -= _skillData.damage;
                if(hitCoroutine)
                {
                    StopCoroutine(coroutine);
                }
                else
                {
                    coroutine = StartCoroutine(IHitRunTimeCheck(_skillData.stunTime));
                }
            }
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

    public void ReverseSprites(float vecX)
    {
        bool _reverseState = (vecX < 0.9f);

        if (reverseState == _reverseState)
            return;

        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    public void SetJumpState(bool _jumpState)
    {
        if (jumpState == _jumpState) return;

        jumpState = _jumpState;
        anim.SetBool("IsJump", jumpState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.ToString() == ENUM_TAG_TYPE.Ground.ToString()
            && GroundCheckWithRay())
        {
            SetJumpState(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.ToString() == ENUM_TAG_TYPE.Ground.ToString())
        {
            SetJumpState(true);
        }
    }

    public bool GroundCheckWithRay()
    {
        Debug.DrawRay(rigid2D.position, Vector2.down * 1.1f, Color.green);

        return Physics2D.Raycast(rigid2D.position, Vector2.down, 1.1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));
    }

    public void Invincible()
    {
        invincibility = true;

        StartCoroutine(IInvincibleCheck(1f)); // 일단 무적시간을 고정값으로 부여 (임시)
    }

    /// <summary>
    /// 경직 시간을 부여하기 위한 함수
    /// </summary>
    /// <param name="_hitTime"></param>
    /// <returns>경직시간</returns>
    protected IEnumerator IHitRunTimeCheck(float _hitTime)
    {
        yield return new WaitForSeconds(_hitTime);

        if (!hitCoroutine)
            anim.SetBool("IsHit", false);
    }

    /// <summary>
    /// 공중 히트 상태에서 바닥에 착지하는 것을 감지하는 함수
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IRisingStateCheck()
    {
        hitCoroutine = true;

        while(!jumpState)
        {
            yield return null;
        }

        // 다운 상태로 바닥에 닿은 상태에 호출
        Set_Rigid2D(Vector2.zero);
        Invincible();
        anim.SetBool("IsHit", false);
        jumpState = false;
        hitCoroutine = false;
    }

    protected IEnumerator IInvincibleCheck(float _invincibleTime)
    {
        yield return new WaitForSeconds(_invincibleTime);

        invincibility = false;
    }

    public void Summon_AttackObject(int _attackTypeNum)
    {
        attackObject = null;
        ENUM_SKILL_TYPE attackType = (ENUM_SKILL_TYPE)_attackTypeNum;
        attackObject = Managers.Resource.GetAttackObject(attackType.ToString());

        if (attackObject != null)
        {
            attackObject.transform.position = transform.position;
            attackObject.ActivatingAttackObject(teamType, reverseState);
        }
        else
        {
            Debug.Log("찾을 수 없음");
        }
    }

    public void Checking_AttackState()
    {
        if (!attackState)
            anim.SetBool("IsIdle", true);
    }

}