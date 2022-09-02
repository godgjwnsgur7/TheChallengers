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

public class AttackObejct : Poolable
{
    public Skill skillValue;
    public ENUM_TEAM_TYPE teamType;
    public ENUM_ATTACKOBJECT_TYPE attackObjectType;
    
    public bool reverseState;

    public override void Init()
    {
        base.Init();
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Default;
        ENUM_SKILL_TYPE skill = (ENUM_SKILL_TYPE)Enum.Parse(typeof(ENUM_SKILL_TYPE), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)skill, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingAttackObject(ENUM_TEAM_TYPE _teamType,bool _reverseState)
    {
        isUsing = true;

        reverseState = _reverseState;
        teamType = _teamType;

        transform.localEulerAngles = reverseState ? new Vector3(0, 180, 0) : Vector3.zero;

        gameObject.SetActive(true);

        CoroutineHelper.StartCoroutine(IAttackRunTimeCheck(skillValue.runTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveCharacter enemyCharacter = collision.GetComponent<ActiveCharacter>();

        // 충돌한 객체가 액티브캐릭터가 아니라면 파괴 (ShotAttackObject)
        if (enemyCharacter == null && attackObjectType == ENUM_ATTACKOBJECT_TYPE.Shot)
        {
            Managers.Resource.Destroy(gameObject);
            return;
        }

        if (enemyCharacter.teamType == teamType || enemyCharacter.invincibility)
            return;

        if (enemyCharacter != null && skillValue != null)
        {
            CharacterAttackParam attackParam = new CharacterAttackParam((ENUM_SKILL_TYPE)skillValue.skillType, reverseState);

            if(PhotonLogicHandler.IsConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter, CharacterAttackParam>
                    (enemyCharacter, enemyCharacter.Hit, attackParam);
            }
            else
                enemyCharacter.Hit(attackParam);

            isUsing = false;
            Managers.Resource.Destroy(gameObject);
        }
        else
        {
            Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }

    private IEnumerator IAttackRunTimeCheck(float _runTime)
    {
        yield return new WaitForSeconds(_runTime);
        
        if(this.gameObject.activeSelf)
        {
            isUsing = false;
            Managers.Resource.Destroy(gameObject);
        }
    }
}
