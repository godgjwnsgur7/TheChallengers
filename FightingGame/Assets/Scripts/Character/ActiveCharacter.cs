using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public ENUM_SKILL_TYPE[] skills = new ENUM_SKILL_TYPE[3];

    public AttackObejct attackObject;
    public StatusWindowUI statusWindowUI;

    Coroutine coroutine;

    public override void Init()
    {
        base.Init();

        if (PhotonLogicHandler.IsConnected)
        {
            if ((PhotonLogicHandler.IsMasterClient && teamType == ENUM_TEAM_TYPE.Blue)
                || (!PhotonLogicHandler.IsMasterClient && teamType == ENUM_TEAM_TYPE.Red))
                isControl = true;
            else
                isControl = false;
        }
        else
        {
            Debug.Log("확인");
            isControl = true;
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        // Animator
        if (anim == null)
            anim = GetComponent<Animator>();

        if (anim == null)
        {
            gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = Managers.Resource.GetAnimator(characterType);
        }

        if (teamType == ENUM_TEAM_TYPE.Blue)
            ReverseSprites(1.0f);

        if(isControl)
            StartCoroutine(IJumpStateCheck());

        if (PhotonLogicHandler.IsConnected)
        {
            var param = MakeSyncAnimParam();
            SyncAnimator(anim, param);
        }
    }

    /// <summary>
    /// 동기화할 파라미터 배열 만들기
    /// </summary>
    /// <returns></returns>

    private AnimatorSyncParam[] MakeSyncAnimParam()
	{
        AnimatorSyncParam[] syncParams = new AnimatorSyncParam[]
        {
            new AnimatorSyncParam("IsIdle", AnimParameterType.Bool),
            new AnimatorSyncParam("DirX", AnimParameterType.Float),
            new AnimatorSyncParam("IsMove", AnimParameterType.Bool),
            new AnimatorSyncParam("AttackTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("IsJump", AnimParameterType.Bool),
            new AnimatorSyncParam("JumpTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("SkillType", AnimParameterType.Int),
            new AnimatorSyncParam("SkillTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("IsHit", AnimParameterType.Bool),
            new AnimatorSyncParam("HitTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("HitState", AnimParameterType.Bool),
            new AnimatorSyncParam("DropTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("IsDrop", AnimParameterType.Bool),
            new AnimatorSyncParam("DieTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("IsDie", AnimParameterType.Bool),
        };

        return syncParams;
    }

    public override void Idle()
    {
        base.Idle();

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

        if (currState != ENUM_PLAYER_STATE.Idle &&
            currState != ENUM_PLAYER_STATE.Move)
            return;

        base.Jump();

        anim.SetTrigger("JumpTrigger");
        anim.SetBool("IsJump", true);
    }

    public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack || currState == ENUM_PLAYER_STATE.Skill)
            return;

        if (attackObject != null) attackObject = null;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("AttackTrigger");
        }
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
                if (superArmour)
                {
                    curHP -= _skillData.damage;
                    return;
                }

                base.Hit(param);
                anim.SetBool("IsHit", true);
                anim.SetTrigger("HitTrigger");
                curHP -= _skillData.damage;
                
                if(!statusWindowUI.Input_Damage(_skillData.damage)) // 캐릭터의 HP가 다 닳음
                {
                    Die();
                    return;
                }

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

    public override void Die()
    {
        base.Die();

        anim.SetBool("IsHit", false);

        anim.SetTrigger("DieTrigger");
        anim.SetBool("IsDie", true);
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

        if (PhotonLogicHandler.IsConnected)
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, bool>
                (this, Sync_ReverseState, _reverseState);
        else
            Sync_ReverseState(_reverseState);
    }

    public void Invincible()
    {
        invincibility = true;

        StartCoroutine(IInvincibleCheck(1f)); // 일단 무적시간을 고정값으로 부여 (임시)
    }

    /// <summary>
    /// 점프상태임을 감지하는 코루틴 (업데이트문이나 다름없는 상태임 일단)
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IJumpStateCheck()
    {
        bool _jumpState;
        
        while(true)
        {
            yield return null;

            Debug.DrawRay(rigid2D.position, Vector2.down * 1.1f, Color.green);
            _jumpState = !Physics2D.Raycast(rigid2D.position, Vector2.down, 1.1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));

            if(jumpState != _jumpState)
            {   
                jumpState = _jumpState;
                if(!anim.GetBool("IsJump") && currState != ENUM_PLAYER_STATE.Hit)
                    anim.SetTrigger("DropTrigger");
                anim.SetBool("IsJump", jumpState);
            }
        }
    }

    /// <summary>
    /// 경직 시간을 부여하기 위한 함수
    /// </summary>
    /// <param name="_hitTime"></param>
    /// <returns>경직시간</returns>
    protected IEnumerator IHitRunTimeCheck(float _hitTime)
    {
        float realTime = 0f;

        while (_hitTime >= realTime)
        {
            realTime += Time.deltaTime;

            if (jumpState && !hitCoroutine)
            {
                realTime += _hitTime;
                StartCoroutine(IRisingStateCheck());
            }

            yield return null;
        }

        if (!hitCoroutine) // 경직만 당한 상태
            anim.SetBool("IsHit", false);
    }

    /// <summary>
    /// 공중 히트 상태에서 바닥에 착지하는 것을 감지하는 함수
    /// </summary>
    /// <returns></returns>
    protected IEnumerator IRisingStateCheck()
    {
        hitCoroutine = true;

        while (jumpState)
        {
            yield return null;
        }

        Push_Rigid2D(Vector2.zero);
        Invincible();
        anim.SetBool("IsHit", false);
        hitCoroutine = false;
    }

    protected IEnumerator IInvincibleCheck(float _invincibleTime)
    {
        yield return new WaitForSeconds(_invincibleTime);

        invincibility = false;
    }

    protected void Summon_AttackObject(int _attackTypeNum)
    {
        if (!isControl) return;

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
            Debug.Log($"ENUM_SKILL_TYPE에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }
    }

    protected void SuperArmourState_On()
    {
        if (!isControl) return;

        superArmour = true;
    }

    protected void SuperArmourState_Off()
    {
        if (!isControl) return;

        superArmour = false;
    }

    protected void Checking_AttackState()
    {
        if (!isControl) return;

        if (!attackState)
            anim.SetBool("IsIdle", true);
    }

    protected void Move_Attack(float vecX)
    {
        if (!isControl) return;

        if (reverseState)
            vecX *= -1f;

        Push_Rigid2D(new Vector2(vecX, 0));
    }
}