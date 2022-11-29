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

    public StatusWindowUI statusWindowUI;
    public AttackObject attackObject;

    Coroutine stunTimeCoroutine;
    Coroutine landCoroutine;
    Coroutine dropCoroutine;

    public float stunTime;
    public float currStunTime;

    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 캐릭터를 초기화 시도하였습니다.");
            return;
        }

        base.Init();

        // Setting Pool
        Skills_Pooling(characterType);
        Effects_Pooling();

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

        var param = MakeSyncAnimParam();
        SyncAnimator(anim, param);
      
        isInitialized = true;
    }

    public void Set_Character()
    {
        if (PhotonLogicHandler.IsConnected)
        {
            isControl = PhotonLogicHandler.IsMine(viewID);
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter>
                (this, Connect_MyStatusUI);
        }
        else
        {
            isControl = true;
            Connect_MyStatusUI();
            Managers.Battle.Set_MyCharacterType(characterType);
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
            new AnimatorSyncParam("DirX", AnimParameterType.Float),
            new AnimatorSyncParam("IsMove", AnimParameterType.Bool),
            new AnimatorSyncParam("MoveState", AnimParameterType.Bool),
            new AnimatorSyncParam("AttackState", AnimParameterType.Bool),
            new AnimatorSyncParam("IsJump", AnimParameterType.Bool),
            new AnimatorSyncParam("SkillType", AnimParameterType.Int),
            new AnimatorSyncParam("IsHit", AnimParameterType.Bool),
            new AnimatorSyncParam("HitState", AnimParameterType.Bool),
            new AnimatorSyncParam("IsDrop", AnimParameterType.Bool),
            new AnimatorSyncParam("IsDie", AnimParameterType.Bool),

            new AnimatorSyncParam("AttackTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("JumpTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("HitTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("DropTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("DieTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("SkillTrigger", AnimParameterType.Trigger),
        };

        return syncParams;
    }

    // Polling Helper 를 따로 팔지 고민중
    private void Skills_Pooling(ENUM_CHARACTER_TYPE charType)
    {
        switch (charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack1", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack2", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack3", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_1", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_2", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_3", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_JumpAttack", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_1", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_2", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_3", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Knight_ThrowSkillObject", 3);
                
                break;
            case ENUM_CHARACTER_TYPE.Wizard:
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_Attack1", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_Attack2", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThrowAttackObject", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThrowJumpAttackObject", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderAttackObject", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderAttackObject_1", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderAttackObject_2", 3);
                Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderAttackObject_3", 3);
                break;
            default:
                Debug.Log($"Failed to SkillObject : {charType}");
                break;
        }
    }

    private void Effects_Pooling()
    {
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect1", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect2", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect3", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Jump", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Landing", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Move", 5);
        Managers.Resource.GenerateInPool("EffectObjects/Wizard_ThunderEffect", 5);
    }

    public override void Idle()
    {
        if (jumpState) return;

        base.Idle();

        if (anim.GetBool("IsMove"))
            SetAnimBool("IsMove", false);
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
                SetAnimBool("IsMove", true);

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

        dropCoroutine = StartCoroutine(ICharDropStateCheck());

        SetAnimBool("IsJump", true);
        SetAnimTrigger("JumpTrigger");
    }

    public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack || currState == ENUM_PLAYER_STATE.Skill)
            return;

        if (attackObject != null)
            attackObject = null;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            SetAnimTrigger("AttackTrigger");
        }
    }

    public override void Skill(CharacterParam param)
    {
        if (attackObject != null)
            attackObject = null;

        if (currState == ENUM_PLAYER_STATE.Skill || jumpState)
            return;

        base.Skill(param);

        var skillParam = param as CharacterSkillParam;

        if (skillParam != null)
        {
            Managers.Input.Notify_UseSkill(skillParam.skillNum);
            SetAnimInt("SkillType", skillParam.skillNum);
            SetAnimTrigger("SkillTrigger");
        }
    }

    [BroadcastMethod]
    public override void Hit(CharacterParam param)
    {
        if (param == null || invincibility) return;

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            if(Managers.Data.SkillDict.TryGetValue((int)attackParam.attackTypeName, out Skill _skillData))
            {
                if (superArmour)
                {
                    Update_CurrHP(_skillData.damage);
                    return;
                }

                SetAnimBool("IsHit", true);
                SetAnimTrigger("HitTrigger");

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

                Update_CurrHP(_skillData.damage);

                if (currHP <= 0.0f)
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
                    currStunTime = 0.0f;
                    stunTime = _skillData.stunTime;
                    stunTimeCoroutine = StartCoroutine(IStunTimeCheck());
                }
                else if (stunTimeCoroutine != null)
                {
                    currStunTime = 0.0f;
                    stunTime = _skillData.stunTime;
                }
            }
            else
            {
                Debug.Log($"{attackParam.attackTypeName}을 SkillDict에서 찾을 수 없습니다.");
            }
        }
    }

    [BroadcastMethod]
    public void Connect_MyStatusUI()
    {
        BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        if(battleCanvas == null)
        {
            TrainingCanvas trainingCanvas = Managers.UI.currCanvas.GetComponent<TrainingCanvas>();

            statusWindowUI = trainingCanvas.Get_StatusWindowUI(teamType);
        }
        else
        {
            statusWindowUI = battleCanvas.Get_StatusWindowUI(teamType);
        }

        statusWindowUI.Set_StatusWindowUI(characterType, currHP);
    }

    public void Update_CurrHP(float _damage)
    {
        float _currHP = currHP - _damage;

        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, float>
                (this, Sync_CurrHP, _currHP, ENUM_RPC_TARGET.All);
        }
        else
        {
            Sync_CurrHP(_currHP);
        }
    }

    [BroadcastMethod]
    public void Sync_CurrHP(float _currHP)
    {
        currHP = _currHP;
        statusWindowUI.Update_CurrHP(_currHP);
    }
    
    public override void Die()
    {
        base.Die();

        SetAnimBool("IsHit", false);

        SetAnimTrigger("DieTrigger");
        SetAnimBool("IsDie", true);
    }

    public void Input_MoveKey(bool _moveKey)
    {
        SetAnimBool("MoveState", _moveKey);
    }

    public void SetAnimParamVector(float _moveDir)
    {
        ReverseSprites(_moveDir);

        SetAnimFloat("DirX", _moveDir);
    }

    public void Change_AttackState(bool _attackState)
    {
        if (anim.GetBool("AttackState") == _attackState)
            return;

        SetAnimBool("AttackState", _attackState);
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

    [BroadcastMethod]
    public void Sync_ReverseState(bool _reverseState)
    {
        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    public void EndGame()
    {
        if(Managers.Scene.CurrSceneType == ENUM_SCENE_TYPE.Training)
        {
            Destroy(this);
            return;
        }

        if (PhotonLogicHandler.IsConnected)
        {
            if (!Managers.Battle.isGamePlayingState)
                return;

            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, ENUM_TEAM_TYPE>
                (this, Sync_EndGame, teamType);
        }
    }
    
    [BroadcastMethod] public void Sync_EndGame(ENUM_TEAM_TYPE losingTeam) => Managers.Battle.EndGame(losingTeam);    
    [BroadcastMethod] public void Receive_EnemyChar() => Managers.Battle.Set_EnemyChar(this);

    public void Invincible()
    {
        invincibility = true;

        StartCoroutine(IInvincibleCheck(1f)); // 일단 무적시간을 고정값으로 부여 (임시)
    }

    #region IEnumerator ( Courotine )
    /// <summary>
    /// 점프상태임을 감지 (업데이트문이나 다름없는 상태)
    /// </summary>
    protected IEnumerator IJumpStateCheck()
    {
        bool LandingState;
        
        while(true)
        {
            Debug.DrawRay(rigid2D.position, Vector2.down * 1.1f, Color.green);
            LandingState = Physics2D.Raycast(rigid2D.position, Vector2.down, 1.1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));

            if (LandingState)
                SetAnimBool("IsDrop", false);

            if (jumpState == LandingState)
            {
                jumpState = !jumpState;
                
                if (dropCoroutine == null &&
                    (currState != ENUM_PLAYER_STATE.Hit && currState != ENUM_PLAYER_STATE.Skill))
                {
                    SetAnimBool("IsDrop", true);
                    SetAnimTrigger("DropTrigger");
                }
                SetAnimBool("IsJump", jumpState);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 경직 시간을 부여, 경직 시간은 호출 전에 세팅
    /// </summary>
    protected IEnumerator IStunTimeCheck()
    {
        while (stunTime >= currStunTime)
        {
            currStunTime += Time.deltaTime;

            if (jumpState && landCoroutine == null)
            {
                landCoroutine = StartCoroutine(ILandingCheck());
                StopCoroutine(stunTimeCoroutine);
                stunTimeCoroutine = null;
            }

            yield return null;
        }

        stunTimeCoroutine = null;
        SetAnimBool("IsHit", false);
    }

    /// <summary>
    /// 공중 히트 상태에서 바닥에 착지하는 것을 감지
    /// </summary>
    protected IEnumerator ILandingCheck()
    {
        while (jumpState)
        {
            yield return null;
        }

        Invincible();
        landCoroutine = null;
        Push_Rigid2D(Vector2.zero);
        SetAnimBool("IsHit", false);
    }

    /// <summary>
    /// 점프 키를 눌렀을 때 호출되며, 낙하상태로 들어감을 감지
    /// </summary>
    private IEnumerator ICharDropStateCheck()
    {
        bool dropState = false;

        float charPosY = transform.position.y;
         
        while (!dropState)
        {
            dropState = charPosY > transform.position.y;

            if(!dropState)
                charPosY = transform.position.y;

            yield return null;
        }

        dropCoroutine = null;
        SetAnimBool("IsDrop", true);
    }

    protected IEnumerator IInvincibleCheck(float _invincibleTime)
    {
        yield return new WaitForSeconds(_invincibleTime);

        invincibility = false;
    }
    #endregion
    #region Animation Event Function
    protected void Summon_AttackObject(int _attackTypeNum)
    {
        if (!isControl) return;

        attackObject = null;
        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)_attackTypeNum;

        bool isConnected = PhotonLogicHandler.IsConnected;

        if (isConnected)
            attackObject = Managers.Resource.InstantiateEveryone(attackObjectName.ToString(), Vector2.zero).GetComponent<AttackObject>();
        else
            attackObject = Managers.Resource.GetAttackObject(attackObjectName.ToString());

        if (attackObject != null)
        {
            attackObject.FollowingTarget(this.transform);

            if(isConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, ENUM_TEAM_TYPE, bool>
                    (attackObject, attackObject.ActivatingAttackObject, teamType, reverseState);
            }
            else
                attackObject.ActivatingAttackObject(teamType, reverseState);
        }
        else
        {
            Debug.Log($"ENUM_ATTACKOBJECT_NAME에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }
    }

    protected void Summon_EffectObject(int _effectTypeNum)
    {
        if (!isControl) return;

        attackObject = null;
        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        bool isConnected = PhotonLogicHandler.IsConnected;

        EffectObject effectObject = null;

        if (isConnected)
            effectObject = Managers.Resource.InstantiateEveryone(effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            effectObject.Set_Position(this.transform);

            if (isConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject, bool, int>
                    (effectObject, effectObject.ActivatingEffectObject, reverseState, _effectTypeNum);
            }
            else
                effectObject.ActivatingEffectObject(reverseState, _effectTypeNum);
        }
        else
        {
            Debug.Log($"ENUM_EFFECTOBJECT_NAME에서 해당 번호를 찾을 수 없음 : {_effectTypeNum}");
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

    protected void AnimEvent_Move(float vecX)
    {
        if (!isControl) return;

        if (reverseState)
            vecX *= -1f;

        Push_Rigid2D(new Vector2(vecX, 0));
    }

    #endregion
}