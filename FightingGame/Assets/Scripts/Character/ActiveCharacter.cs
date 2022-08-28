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

    Coroutine stunTimeCoroutine;
    Coroutine landCoroutine;

    public float stunTime;
    public float curStunTime;

    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 캐릭터를 초기화 시도하였습니다.");
            return;
        }

        base.Init();

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

        if (PhotonLogicHandler.IsConnected)
        {
            var param = MakeSyncAnimParam();
            SyncAnimator(anim, param);
        }

        isInitialized = true;
    }

    public void Set_Character()
    {
        if (PhotonLogicHandler.IsConnected)
        {
            if (PhotonLogicHandler.IsMine(viewID))
            {
                isControl = true;
            }
            else
            {
                isControl = false;
            }
        }
        else
        {
            isControl = true;
        }

        if (teamType == ENUM_TEAM_TYPE.Red)
            ReverseSprites(-1.0f);

        if (isControl)
            StartCoroutine(IJumpStateCheck());
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
        if (jumpState) return;

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

    [BroadcastMethod]
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

                anim.SetBool("IsHit", true);
                anim.SetTrigger("HitTrigger");

                base.Hit(param);
                
                Vector2 getPowerDir = new Vector2(_skillData.pushingPower, _skillData.risingPower);

                if (attackParam.reverseState)
                    getPowerDir.x *= -1.0f;

                ReverseSprites(getPowerDir.x * -1.0f);

                if (jumpState && _skillData.risingPower == 0.0f)
                {
                    // 추후에 수치 조정 방식 변경이 필요할 듯 (임시)
                    getPowerDir.y = _skillData.pushingPower * 2;
                    getPowerDir.x = getPowerDir.normalized.x;
                }

                Push_Rigid2D(getPowerDir);

                curHP -= _skillData.damage;
                
                if(!statusWindowUI.Input_Damage(_skillData.damage)) // 캐릭터의 HP가 다 닳음
                {
                    Die();
                    return;
                }

                if(jumpState && landCoroutine == null)
                {
                    landCoroutine = StartCoroutine(ILandingCheck());
                }
                else if(stunTimeCoroutine == null && landCoroutine == null)
                {
                    curStunTime = 0.0f;
                    stunTime = _skillData.stunTime;
                    stunTimeCoroutine = StartCoroutine(IStunTimeCheck());
                }
                else if (stunTimeCoroutine != null)
                {
                    curStunTime = 0.0f;
                    stunTime = _skillData.stunTime;
                }
            }
            else
            {
                Debug.Log($"{attackParam.attackType}을 SkillDict에서 찾을 수 없습니다.");
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
            Debug.DrawRay(rigid2D.position, Vector2.down * 1.1f, Color.green);
            _jumpState = !Physics2D.Raycast(rigid2D.position, Vector2.down, 1.1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));

            if(jumpState != _jumpState)
            {   
                jumpState = _jumpState;
                if(!anim.GetBool("IsJump") && currState != ENUM_PLAYER_STATE.Hit)
                    anim.SetTrigger("DropTrigger");
                anim.SetBool("IsJump", jumpState);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 경직 시간을 부여하기 위한 함수
    /// </summary>
    /// <param name="_hitTime"></param>
    /// <returns>경직시간</returns>
    protected IEnumerator IStunTimeCheck()
    {
        while (stunTime >= curStunTime)
        {
            curStunTime += Time.deltaTime;

            if (jumpState && landCoroutine == null)
            {
                landCoroutine = StartCoroutine(ILandingCheck());
                StopCoroutine(stunTimeCoroutine);
                stunTimeCoroutine = null;
            }

            yield return null;
        }

        stunTimeCoroutine = null;
        anim.SetBool("IsHit", false);
    }

    /// <summary>
    /// 공중 히트 상태에서 바닥에 착지하는 것을 감지하는 함수
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ILandingCheck()
    {
        while (jumpState)
        {
            yield return null;
        }

        Invincible();
        landCoroutine = null;
        Push_Rigid2D(Vector2.zero);
        anim.SetBool("IsHit", false);
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

        bool isConnected = PhotonLogicHandler.IsConnected;
        
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