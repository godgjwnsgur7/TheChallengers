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
    Fall = 4,
}

public class AttackObject : Poolable
{
    protected Skill skillValue;
    protected ENUM_TEAM_TYPE teamType;
    protected ENUM_ATTACKOBJECT_TYPE attackObjectType = ENUM_ATTACKOBJECT_TYPE.Default;

    protected Transform targetTr = null;
    protected bool reverseState;

    protected bool isMine = true;
    protected bool isServerSyncState = false;

    protected Coroutine runTimeCheckCoroutine = null;

    public override void Init()
    {
        base.Init();

        isServerSyncState = Managers.Battle.isServerSyncState;

        if (isServerSyncState)
        {
            SyncTransformView(transform);
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingAttackObject(Vector2 _targetTr, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        this.transform.position = _targetTr;
        reverseState = _reverseState;
        teamType = _teamType;
    }

    public void Set_TargetTransform(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isServerSyncState && isMine)
            return;

        ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

        if (enemyCharacter != null && skillValue != null)
        {
            if (enemyCharacter.teamType == teamType || enemyCharacter.invincibility)
                return;

            enemyCharacter.Hit(new CharacterAttackParam((ENUM_ATTACKOBJECT_NAME)skillValue.skillType, reverseState));

            // 이펙트 생성 (임시)
            int effectNum = UnityEngine.Random.Range(0, 3);
            Summon_EffectObject(effectNum, collision.transform.position);

            DestroyMine();
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    protected void Summon_EffectObject(int _effectTypeNum, Vector2 _targetTr)
    {
        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        EffectObject effectObject = null;

        if (isServerSyncState)
            effectObject = Managers.Resource.InstantiateEveryone(effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            Vector2 HitPosition = this.GetComponent<Collider2D>().bounds.ClosestPoint(_targetTr);
            effectObject.Set_Position(HitPosition);

            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject,Vector2, bool, int>
                    (effectObject, effectObject.ActivatingEffectObject, _targetTr, reverseState, _effectTypeNum);
            }
            else
                effectObject.ActivatingEffectObject(transform.position, reverseState, _effectTypeNum);
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
            
            if(attackObjectType == ENUM_ATTACKOBJECT_TYPE.Follow)
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

        if (runTimeCheckCoroutine != null)
            StopCoroutine(runTimeCheckCoroutine);
        
        if (isServerSyncState)
            PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject>(this, Sync_DestroyMine);
        else
            Managers.Resource.Destroy(gameObject);
    }

    [BroadcastMethod]
    public virtual void Sync_DestroyMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
