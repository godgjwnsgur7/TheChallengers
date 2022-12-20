using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public enum ENUM_ATTACKOBJECT_TYPE
{
    Default = 0,
    Shot = 1,
    Multi = 2,
    Follow = 3,
}

public class AttackObject : Poolable
{
    public Skill skillValue;
    public ENUM_TEAM_TYPE teamType;
    public ENUM_ATTACKOBJECT_TYPE attackObjectType = ENUM_ATTACKOBJECT_TYPE.Default;

    public Transform targetTr = null;
    public bool reverseState;

    protected Coroutine runTimeCheckCoroutine = null;

    public override void OnDisable()
    {
        isUsing = false;

        if (runTimeCheckCoroutine != null)
            StopCoroutine(runTimeCheckCoroutine);

        base.OnDisable();
    }

    public override void Init()
    {
        base.Init();
        if (attackObjectType == ENUM_ATTACKOBJECT_TYPE.Default)
            attackObjectType = ENUM_ATTACKOBJECT_TYPE.Follow;

        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)Enum.Parse(typeof(ENUM_ATTACKOBJECT_NAME), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)attackObjectName, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }

        if (Managers.Battle.isServerSyncState)
        {
            SyncTransformView(transform);
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingAttackObject(ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        isUsing = true;

        reverseState = _reverseState;
        teamType = _teamType;

        transform.localEulerAngles = reverseState ? new Vector3(0, 180, 0) : Vector3.zero;

        gameObject.SetActive(true);

        if (Managers.Battle.isServerSyncState)
        {
            if (PhotonLogicHandler.IsMine(viewID))
                runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
        }
        else
            runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
    }

    public void FollowingTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Managers.Battle.isServerSyncState && PhotonLogicHandler.IsMine(viewID))
            return;

        /* 음.... 이게 맞나 싶긴 하네    
        if (attackObjectType == ENUM_ATTACKOBJECT_TYPE.Shot && collision.tag == ENUM_TAG_TYPE.Ground.ToString())
        {
            DestroyMine();
            return;
        }
        */

        ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

        if (enemyCharacter != null && skillValue != null)
        {
            if (enemyCharacter.teamType == teamType || enemyCharacter.invincibility)
                return;

            enemyCharacter.Hit(new CharacterAttackParam((ENUM_ATTACKOBJECT_NAME)skillValue.skillType, reverseState));

            // 이펙트 생성 (임시)
            int effectNum = UnityEngine.Random.Range(0, 3);
            Summon_EffectObject(effectNum, collision.transform);

            DestroyMine();
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    protected void Summon_EffectObject(int _effectTypeNum, Transform _targetTransform)
    {
        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        bool isServerSyncState = Managers.Battle.isServerSyncState;

        EffectObject effectObject = null;

        if (isServerSyncState)
            effectObject = Managers.Resource.InstantiateEveryone(effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            Vector2 HitPosition = this.GetComponent<Collider2D>().bounds.ClosestPoint(_targetTransform.position);
            effectObject.Set_Position(HitPosition);

            if (isServerSyncState)
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

    protected IEnumerator IRunTimeCheck(float _runTime)
    {
        float realTime = 0.0f;

        while(realTime < _runTime && isUsing)
        {
            realTime += Time.deltaTime;
            
            if(attackObjectType != ENUM_ATTACKOBJECT_TYPE.Shot || targetTr != null)
                this.transform.position = targetTr.position;

            yield return null;
        }

        runTimeCheckCoroutine = null;
        DestroyMine();
    }
        
    public void DestroyMine()
    {
        if (!isUsing) return;

        targetTr = null;

        if (Managers.Battle.isServerSyncState)
            PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject>(this, Sync_DestroyMine);
        else
            Managers.Resource.Destroy(gameObject);
    }

    [BroadcastMethod]
    public virtual void Sync_DestroyMine()
    {
        isUsing = false;

        Managers.Resource.Destroy(this.gameObject);
    }

    public void AnimEvent_DestoryMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
