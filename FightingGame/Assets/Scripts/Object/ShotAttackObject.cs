using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class ShotAttackObject : AttackObject
{
    Skill skillValue;
    Coroutine runTimeCheckCoroutine = null;
    Rigidbody2D rigid2D;

    [SerializeField] Vector3 subPos;
    [SerializeField] Vector2 shotSpeed;

    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Shot;

        base.Init();

        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)Enum.Parse(typeof(ENUM_ATTACKOBJECT_NAME), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)attackObjectName, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if (isServerSyncState)
        {
            SyncPhysics(rigid2D);
        }
    }

    public override void ActivatingAttackObject(Vector2 _targetTr, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.ActivatingAttackObject(_targetTr, _teamType, _reverseState);

        shotSpeed.x = Mathf.Abs(shotSpeed.x);
        if (reverseState)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(subPos.x * -1.0f, subPos.y, 0);
            shotSpeed.x *= -1f;
        }
        else
        {
            transform.localEulerAngles = Vector3.zero;
            transform.position += subPos;
        }

        gameObject.SetActive(true);

        isMine = true;

        if (Managers.Network.isServerSyncState)
        {
            if (PhotonLogicHandler.IsMine(viewID))
                runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
            else
                isMine = false;
        }
        else
            runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));

        Shot_AttackObject();
    }

    public void Shot_AttackObject() => rigid2D.AddForce(shotSpeed);

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

            Destroy_Mine();
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    public IEnumerator IRunTimeCheck(float _runTime)
    {
        float realTime = 0.0f;

        while (realTime < _runTime && isUsing)
        {
            realTime += Time.deltaTime;

            if (attackObjectType == ENUM_ATTACKOBJECT_TYPE.Follow)
                this.transform.position = targetTr.position;

            yield return null;
        }

        runTimeCheckCoroutine = null;
        Destroy_Mine();
    }

    public void Destroy_Mine()
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

    public void Summon_EffectObject(int _effectTypeNum, Vector2 _targetTr)
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
                PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject, Vector2, bool, int>
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
}
