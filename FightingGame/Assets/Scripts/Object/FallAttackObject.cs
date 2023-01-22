using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

// 낙하해서 충돌하면 이펙트 발생
public class FallAttackObject : MultiAttackObject
{
    Animator anim;
    Rigidbody2D rigid2D;

    [SerializeField] protected Vector2 shotPos;
    [SerializeField] protected Vector3 subPos;

    bool reverseShot;

    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Fall;

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();

        base.Init();

        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)Enum.Parse(typeof(ENUM_ATTACKOBJECT_NAME), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)attackObjectName, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }
    }

    public override void ActivatingAttackObject(Vector2 _targetTr, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.ActivatingAttackObject(_targetTr, _teamType, _reverseState);

        if (reverseState)
        {
            reverseShot = reverseState;
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(subPos.x * -1.0f, subPos.y, 0);
        }
        else
        {
            reverseShot = reverseState;
            transform.localEulerAngles = Vector3.zero;
            transform.position += subPos;
        }

        gameObject.SetActive(true);

        isMine = true;

        if (isServerSyncState)
        {
            if (PhotonLogicHandler.IsMine(viewID))
                runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
            else
                isMine = false;
        }
        else
            runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
    }

    [BroadcastMethod]
    public void Move_AttackObject()
    {
        Vector2 vecPos = shotPos;

        if (reverseShot)
            vecPos.x *= -1f;

        rigid2D.AddForce(vecPos, ForceMode2D.Force);
    }

    [BroadcastMethod]
    public void Set_Bool(string _parametorName)
    {
        anim.SetBool(_parametorName, !anim.GetBool(_parametorName));
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

            Set_Bool("Hit");

            rigid2D.velocity = Vector3.zero;
        }
        else
        {
            // Debug.Log($"{gameObject.name}이 {collision.gameObject.name}을 감지했으나 Hit하지 못함");
        }
    }
}
