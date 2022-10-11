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

        if (PhotonLogicHandler.IsConnected)
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

        if(PhotonLogicHandler.IsConnected)
        {
            if (PhotonLogicHandler.IsMine(viewID))
            {
                StartCoroutine(IRunTimeCheck(skillValue.runTime));
            }
        }
        else
            StartCoroutine(IRunTimeCheck(skillValue.runTime));
    }

    public void FollowingTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (!PhotonLogicHandler.IsMine(viewID) || collision.gameObject.GetComponent<AttackObject>() != null)
            return;

        ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

            // 충돌한 객체가 액티브캐릭터가 아니라면 파괴 (ShotAttackObject)
        if (enemyCharacter == null && attackObjectType == ENUM_ATTACKOBJECT_TYPE.Shot)
        {
            DestroyMine();
            return;
        }

        if (enemyCharacter != null && skillValue != null)
        {
            if (enemyCharacter.teamType == teamType || enemyCharacter.invincibility)
                return;

            CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_ATTACKOBJECT_NAME)skillValue.skillType, reverseState);
            
            
            if(PhotonLogicHandler.IsConnected)
                PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, CharacterAttackParam>
                    (enemyCharacter, enemyCharacter.Hit, attackParam, ENUM_RPC_TARGET.OTHER);
            else
                enemyCharacter.Hit(attackParam);

            // 이펙트 생성 (임시)
            int effectNum = UnityEngine.Random.Range(0, 3);
            Summon_EffectObject(effectNum, collision.transform);

            DestroyMine();
        }
        else
        {
            Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    protected void Summon_EffectObject(int _effectTypeNum, Transform _targetTransform)
    {
        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        bool isConnected = PhotonLogicHandler.IsConnected;

        EffectObject effectObject = null;

        if (isConnected)
            effectObject = Managers.Resource.InstantiateEveryone(effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            Vector2 HitPosition = this.GetComponent<Collider2D>().bounds.ClosestPoint(_targetTransform.position);
            effectObject.Set_Position(HitPosition);

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

    protected IEnumerator IRunTimeCheck(float _runTime)
    {
        float realTime = 0.0f;

        while(realTime < _runTime && this.gameObject.activeSelf)
        {
            realTime += Time.deltaTime;
            
            if(attackObjectType != ENUM_ATTACKOBJECT_TYPE.Shot || targetTr != null)
                this.transform.position = targetTr.position;

            yield return null;
        }

        DestroyMine();
    }
    
    public virtual void DestroyMine()
    {
        if (!this.gameObject.activeSelf) return;

        isUsing = false;
        targetTr = null;

        if (PhotonLogicHandler.IsConnected)
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
