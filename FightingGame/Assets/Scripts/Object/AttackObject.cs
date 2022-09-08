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

        ENUM_SKILL_TYPE skill = (ENUM_SKILL_TYPE)Enum.Parse(typeof(ENUM_SKILL_TYPE), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)skill, out skillValue))
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
                CoroutineHelper.StartCoroutine(IRunTimeCheck(skillValue.runTime));
        }
        else
            CoroutineHelper.StartCoroutine(IRunTimeCheck(skillValue.runTime));
    }


    public void FollowingTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (!PhotonLogicHandler.IsMine(viewID))
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

            CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_SKILL_TYPE)skillValue.skillType, reverseState);
            
            /*
            if(PhotonLogicHandler.IsConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, CharacterAttackParam>
                    (enemyCharacter, enemyCharacter.Hit, attackParam);
            }
            else*/
                enemyCharacter.Hit(attackParam);

            DestroyMine();
        }
        else
        {
            Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    private IEnumerator IRunTimeCheck(float _runTime)
    {
        float realTime = 0.0f;

        while(realTime < _runTime && this.gameObject.activeSelf)
        {
            realTime += Time.deltaTime;
            
            if(attackObjectType != ENUM_ATTACKOBJECT_TYPE.Shot || targetTr != null)
                this.transform.position = targetTr.position;

            yield return null;
        }

        if(this.gameObject.activeSelf)
        {
            DestroyMine();
        }
    }
    
    public virtual void DestroyMine()
    {
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
