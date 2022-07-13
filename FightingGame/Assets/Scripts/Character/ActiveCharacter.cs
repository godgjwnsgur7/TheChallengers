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
        if (attackObject != null)
            attackObject = null;
        base.Attack(param);
    }

    public override void Hit(CharacterParam param)
    {
        if (param == null || invincibility) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            if(Managers.Data.SkillDict.TryGetValue((int)attackParam.skillType, out Skill _skillData))
            {
                base.Hit(param);
                anim.SetBool("IsHit", true);
                hp -= _skillData.damage;
                StopCoroutine("IHitRunTimeCheck"); // 문제가 있다 ㅎ;
                StartCoroutine(IHitRunTimeCheck(_skillData.runTime));
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

    private void ReverseSprites(float vecX)
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
        if (collision.gameObject.tag.ToString() == ENUM_TAG_TYPE.Ground.ToString())
            SetJumpState(false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.ToString() == ENUM_TAG_TYPE.Ground.ToString())
            SetJumpState(true);
    }

    protected IEnumerator IAttackDelayTimeCheck(CharacterAttackParam _attackParam)
    {
        float delayTime;
        if(Managers.Data.SkillDict.TryGetValue((int)_attackParam.skillType, out Skill skill))
        {
            delayTime = skill.delayTime;
        }
        else
        {
            Debug.Log("찾을 수 없음");
            yield break;
        }

        yield return new WaitForSeconds(delayTime);

        attackObject.transform.position = gameObject.transform.position;
        attackObject.ActivatingAttackObject(_attackParam, reverseState);
    }

    protected IEnumerator IHitRunTimeCheck(float _hitTime)
    {
        yield return new WaitForSeconds(_hitTime);

        if(currState == ENUM_PLAYER_STATE.Hit)
            anim.SetBool("IsHit", false);
    }
}