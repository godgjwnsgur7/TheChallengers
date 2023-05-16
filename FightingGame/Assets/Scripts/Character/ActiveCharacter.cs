using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public partial class ActiveCharacter : Character
{
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;

    protected AttackObject attackObject;
    Action<float> OnHit;

    Coroutine stunTimeCoroutine;
    Coroutine landCoroutine;
    Coroutine dropCoroutine;
    Coroutine invincibleCoroutine;
    Coroutine hitImmunityCoroutine;
    Coroutine dashCoroutine;

    public float stunTime;
    public float currStunTime;

    public override void Init()
    {
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

        var param = MakeSyncAnimParam();
        SyncAnimator(anim, param);

        spriteRenderer.sortingOrder = Managers.OrderLayer.Get_CharacterOrderLayer();

        if (isServerSyncState)
        {
            isControl = PhotonLogicHandler.IsMine(viewID);
            Skills_Pooling();

            if(isControl)
                gameObject.AddComponent<AudioListener>();

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            Managers.Sound.Set_SFXSoundSetting(audioSource);
        }
        else
        {
            isControl = true;
        }
        
    }

    public virtual void Skills_Pooling()
    {
        // Public Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect1}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect2}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_AttackedEffect3}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect1}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Basic_SkillAttackedEffect2}", 3);
    }

    public void Set_Character(ENUM_TEAM_TYPE _teamType)
    {
        teamType = _teamType;

        if (isServerSyncState)
        {
            isControl = PhotonLogicHandler.IsMine(viewID);
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, ENUM_TEAM_TYPE>
                (this, Connect_MyStatusUI, teamType);
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter>
                (this, Set_CharacterToEnemyClient, ENUM_RPC_TARGET.OTHER);

            spriteRenderer.sortingOrder += 1;
        }
        else
        {
            isControl = true;
            Connect_MyStatusUI(_teamType);
            if(teamType == ENUM_TEAM_TYPE.Blue)
            {
                spriteRenderer.sortingOrder += 1;
                gameObject.AddComponent<AudioListener>();
            }

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
            Managers.Sound.Set_SFXSoundSetting(audioSource);
        }

        if (teamType == ENUM_TEAM_TYPE.Red)
            ReverseSprites(-1.0f);

        if (isControl)
            StartCoroutine(IJumpStateCheck());
    }

    /// <summary>
    /// 상대 클라이언트에서 수행시킬 함수
    /// </summary>
    [BroadcastMethod]
    public void Set_CharacterToEnemyClient()
    {
        Managers.Battle.Char_ReferenceRegistration(this);
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
            new AnimatorSyncParam("IsDash", AnimParameterType.Bool),

            new AnimatorSyncParam("AttackTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("JumpTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("HitTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("DropTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("DieTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("SkillTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("DashTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("ImmunityTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("TestTrigger", AnimParameterType.Trigger),
        };

        return syncParams;
    }

    public override void Idle()
    {
        if (jumpState && currState != ENUM_PLAYER_STATE.Jump)
        {
            SetAnimBool("IsDrop", true);
            SetAnimTrigger("DropTrigger");
        }

        base.Idle();

        if (anim.GetBool("IsMove"))
            SetAnimBool("IsMove", false);
    }

    public override void Move(CharacterParam param)
    {
        if (currState != ENUM_PLAYER_STATE.Idle && 
            currState != ENUM_PLAYER_STATE.Move &&
            currState != ENUM_PLAYER_STATE.Jump)
            return;

        if (param == null || param is CharacterMoveParam == false)
            return;

        base.Move(param);

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
        if (jumpState)
            return;

        if (currState != ENUM_PLAYER_STATE.Idle &&
            currState != ENUM_PLAYER_STATE.Move)
            return;

        base.Jump();

        dropCoroutine = StartCoroutine(ICharDropStateCheck());

        SetAnimBool("IsJump", true);
        SetAnimTrigger("JumpTrigger");
    }

    public override void Dash()
    {
        if (jumpState || currState == ENUM_PLAYER_STATE.Dash)
            return;

        if (currState != ENUM_PLAYER_STATE.Idle &&
            currState != ENUM_PLAYER_STATE.Move &&
            currState != ENUM_PLAYER_STATE.Attack)
            return;

        if (dashCoroutine != null)
            return;

        if (attackObject != null && attackObject.ObjType == ENUM_SYNCOBJECT_TYPE.Follow)
            attackObject.Sync_DestroyMine();

        base.Dash();

        ResetAnimTrigger("AttackTrigger");

        dashCoroutine = StartCoroutine(IDashTimeCheck(Managers.Data.gameInfo.dashSkillTime));
    }

    public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack || currState == ENUM_PLAYER_STATE.Skill
            || currState == ENUM_PLAYER_STATE.Dash)
            return;

        if (attackObject != null)
            attackObject = null;

        base.Attack(param);

        SetAnimTrigger("TestTrigger");

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            SetAnimTrigger("AttackTrigger");
        }
    }

    public override void Skill(CharacterParam param)
    {  
        if (jumpState || currState == ENUM_PLAYER_STATE.Skill
            || currState == ENUM_PLAYER_STATE.Dash)
            return;

        if (attackObject != null)
            attackObject = null;

        base.Skill(param);

        ResetAnimTrigger("TestTrigger");

        var skillParam = param as CharacterSkillParam;

        if (skillParam != null)
        {
            Managers.Input.Notify_UseSkill(skillParam.skillNum);
            SetAnimInt("SkillType", skillParam.skillNum);
            SetAnimTrigger("SkillTrigger");
        }
    }

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

                if (hitImmunityCoroutine == null)
                    hitImmunityCoroutine = StartCoroutine(IHitImmunityCheck(currHP));

                Vector2 getPowerDir = new Vector2(_skillData.pushingPower, _skillData.risingPower);

                if (attackParam.reverseState)
                    getPowerDir.x *= -1.0f;

                ReverseSprites(getPowerDir.x * -1.0f);

                if (jumpState && _skillData.risingPower == 0.0f)
                {
                    // 추후에 수치 조정 방식 변경이 필요할 듯 (임시)
                    getPowerDir.y = Math.Abs(_skillData.pushingPower) * 2;
                    getPowerDir.x = getPowerDir.normalized.x;
                }

                Push_Rigid2D(getPowerDir);

                Update_CurrHP(_skillData.damage);

                if (currHP <= 0.0f)
                {
                    Die();
                    return;
                }

                if (jumpState && landCoroutine == null)
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
    public void Connect_MyStatusUI(ENUM_TEAM_TYPE _teamType)
    {
        Type canvasType = Managers.UI.currCanvas.GetType();

        if (canvasType == typeof(BattleCanvas))
        {
            BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();
            OnHit = battleCanvas.Get_StatusWindowCallBack(_teamType, characterType);
        }
        else if (canvasType == typeof(TrainingCanvas))
        {
            TrainingCanvas trainingCanvas = Managers.UI.currCanvas.GetComponent<TrainingCanvas>();
            OnHit = trainingCanvas.Get_StatusWindowCallBack(_teamType, characterType);
        }
    } 

    public void Update_CurrHP(float _damage)
    {
#if TEST_MODE
        _damage = int.MaxValue;
#endif
        float _currHP = currHP - _damage;

        if(_currHP <= 0 && isControl)
            Die();

        if(isServerSyncState)
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
        OnHit?.Invoke(currHP);
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

        if (isServerSyncState)
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, bool>
                (this, Sync_ReverseState, _reverseState);
        else
            Sync_ReverseState(_reverseState);
    }

    public void TransparentState(float color_a)
    {
        if (isServerSyncState)
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, float>
                (this, Sync_TransparentState, color_a);
        else
            Sync_TransparentState(color_a);
    }

    [BroadcastMethod]
    public void Sync_ReverseState(bool _reverseState)
    {
        spriteRenderer.flipX = _reverseState;
        reverseState = _reverseState;
    }

    [BroadcastMethod]
    public void Sync_TransparentState(float color_a)
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, color_a);
    }

    public void EndGame()
    {
        if(Managers.Scene.CurrSceneType == ENUM_SCENE_TYPE.Training)
        {
            Managers.Resource.Destroy(gameObject);
            return;
        }

        if (isServerSyncState && isControl)
        {   
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, ENUM_TEAM_TYPE>
                (this, Sync_EndGame, teamType);
        }
    }
    
    [BroadcastMethod] public void Sync_EndGame(ENUM_TEAM_TYPE losingTeam) => Managers.Battle.EndGame(losingTeam);    
 
    public void Invincible()
    {
        if(invincibleCoroutine == null)
            invincibleCoroutine = StartCoroutine(IInvincibleCheck(Managers.Data.gameInfo.invincibleTime)); // 일단 무적시간을 고정값으로 부여 (임시)
    }

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
                
                if ((dropCoroutine == null && currState != ENUM_PLAYER_STATE.Attack)&&
                    (currState != ENUM_PLAYER_STATE.Hit && currState != ENUM_PLAYER_STATE.Skill))
                {
                    currState = ENUM_PLAYER_STATE.Jump;
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
    /// 히트 보정 체크
    /// </summary>
    protected IEnumerator IHitImmunityCheck(float _currHP)
    {
        float maxComboHP = _currHP - Managers.Data.gameInfo.maxComboDamage;

        while (true)
        {
            if (currState != ENUM_PLAYER_STATE.Hit)
            {
                yield return new WaitForSeconds(0.4f);
                
                if(currState != ENUM_PLAYER_STATE.Hit)
                {
                    hitImmunityCoroutine = null;
                    yield break;
                }
            }

            if (maxComboHP >= currHP)
            {
                Invincible();
                if (!jumpState)
                {
                    Push_Rigid2D(new Vector2(Managers.Data.gameInfo.hitImmunityPower * -1.0f, 0));
                    SetAnimTrigger("ImmunityTrigger");
                }
                hitImmunityCoroutine = null;
                yield break;
            }
            yield return null;
        }
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

    private IEnumerator IDashTimeCheck(float _DashTime)
    {
        Managers.Input.Notify_UseSkill(0);
        SetAnimBool("IsDash", true);
        SetAnimTrigger("DashTrigger");

        yield return new WaitForSeconds(_DashTime);

        SetAnimBool("IsDash", false);
        dashCoroutine = null;
    }

    protected IEnumerator IInvincibleCheck(float _invincibleTime)
    {
        invincibility = true;
        TransparentState(0.8f);

        while (currState == ENUM_PLAYER_STATE.Hit)
            yield return null;

        yield return new WaitForSeconds(_invincibleTime);

        if(currState != ENUM_PLAYER_STATE.Die)
            TransparentState(1f);
    
        invincibility = false;
        invincibleCoroutine = null;
    }

    protected IEnumerator IWaitActivate_FollowAttackObject(Action<FollowAttackObject> activatedCallBack, FollowAttackObject followAttackObject)
    {
        yield return new WaitUntil(() => followAttackObject.gameObject.activeSelf == true);

        activatedCallBack(followAttackObject);
    }
    public void Set_TargetTransform(FollowAttackObject followAttackObject)
    {
        followAttackObject.Set_TargetTransform(this.transform);
    }

    #region Animation Event Function
    protected void Summon_AttackObject(int _attackTypeNum)
    {
        if (!isControl) return;

        attackObject = null;
        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)_attackTypeNum;

        if (isServerSyncState)
            attackObject = Managers.Resource.InstantiateEveryone("AttackObjects/" + attackObjectName.ToString(), Vector2.zero).GetComponent<AttackObject>();
        else
            attackObject = Managers.Resource.GetAttackObject("AttackObjects/" + attackObjectName.ToString());

        if (attackObject != null)
        {
            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, Vector2, ENUM_TEAM_TYPE, bool>
                    (attackObject, attackObject.Activate_AttackObject, transform.position, teamType, reverseState);
            }
            else
            {
                attackObject.Activate_AttackObject(transform.position, teamType, reverseState);
            }

            if (attackObject.ObjType == ENUM_SYNCOBJECT_TYPE.Follow)
            {
                FollowAttackObject followAttackObject = (FollowAttackObject)attackObject;
                StartCoroutine(IWaitActivate_FollowAttackObject(Set_TargetTransform, followAttackObject));
            }
        }
        else
        {
            Debug.Log($"ENUM_ATTACKOBJECT_NAME에서 해당 번호를 찾을 수 없음 : {attackObjectName}");
        }
    }

    protected void Summon_EffectObject(int _effectTypeNum)
    {
        if (!isControl) return;

        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        EffectObject effectObject = null;

        if (isServerSyncState)
            effectObject = Managers.Resource.InstantiateEveryone("EffectObjects/"+effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject, Vector2, bool>
                    (effectObject, effectObject.Activate_EffectObject, transform.position, reverseState);
            }
            else
                effectObject.Activate_EffectObject(transform.position, reverseState);
        }
        else
        {
            Debug.Log($"ENUM_EFFECTOBJECT_NAME에서 해당 번호를 찾을 수 없음 : {effectObjectName}");
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
        if (!isControl || currState == ENUM_PLAYER_STATE.Dash) return;

        if (reverseState)
            vecX *= -1f;

        Push_Rigid2D(new Vector2(vecX, 0));
    }

    protected void AnimEvent_MoveToInputArrow(float vecX)
    {
        if (!isControl || currState == ENUM_PLAYER_STATE.Dash) return;

        if (reverseState)
            vecX *= -1f;

        if (inputArrowDir == 0.0f)
        {
            Push_Rigid2D(new Vector2(vecX, 0));
            return;
        }

        bool inputArrowReverseState = inputArrowDir < 0.0f;

        if (reverseState == inputArrowReverseState)
            Push_Rigid2D(new Vector2(vecX * 1.5f, 0));
        else
            Push_Rigid2D(Vector2.zero);
    }

    protected void AnimEvent_PlaySFX(int sfxTypeNum)
    {
        if (audioSource == null || Managers.Sound.Get_SFXSoundMuteState())
            return;

        AudioClipVolume audioClipVolume = Managers.Sound.Get_AudioClipVolume((ENUM_SFX_TYPE)sfxTypeNum);

        float listenerPosX = Managers.Sound.Get_AudioListenerWorldPosX();
        float currDistance = transform.position.x - listenerPosX; // 거리

        if (Math.Abs(currDistance) > 3)
            audioSource.panStereo = currDistance / 10.0f;
        else
            audioSource.panStereo = 0;

        audioSource.volume = audioClipVolume.volume;
        audioSource.PlayOneShot(audioClipVolume.audioClip);
    }

    #endregion

    public void Set_inputArrowDir(float _inputArrowDir)
    {
        inputArrowDir = _inputArrowDir;
    }
}