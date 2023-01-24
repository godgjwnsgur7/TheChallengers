using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

// 낙하해서 충돌하면 이펙트 발생
public class FallAttackObject : AttackObject, IFallAttackObject
{
    Animator anim;
    Rigidbody2D rigid2D;
    AttackObject attackObject;
    BoxCollider2D attackCollider;
    bool isFirstHit = false;

    [SerializeField] protected Vector2 shotSpeed;
    [SerializeField] protected Vector3 subPos;

    Coroutine FollowingObjectCoroutine;

    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Fall;

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();

        base.Init();
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
    }

    public void Shot_AttackObject() => rigid2D.AddForce(shotSpeed);

    [BroadcastMethod]
    public void Reverse_Bool(string _parametorName)
    {
        anim.SetBool(_parametorName, !anim.GetBool(_parametorName));
    }

    public void Check_GroundHit()
    {
        Debug.DrawRay(attackObject.transform.position, transform.up * ((-attackCollider.size.y / 2) + attackCollider.offset.y), Color.red);
        if (Physics2D.Raycast(attackObject.transform.position, Vector2.down, attackCollider.size.y/ 2 + Mathf.Abs(attackCollider.offset.y), LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString())))
        {
            isFirstHit = true;
            rigid2D.velocity = Vector2.zero;
            Reverse_Bool("Hit");
            Managers.Resource.Destroy(attackObject.gameObject);
        }
    }   

    public void Summon_AttackObject(int _attackTypeNum)
    {
        bool isControl = true;

        if (isServerSyncState)
            isControl = PhotonLogicHandler.IsMine(viewID);

        if (!isControl) return;

        attackObject = null;
        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)_attackTypeNum;

        if (isServerSyncState)
            attackObject = Managers.Resource.InstantiateEveryone(attackObjectName.ToString(), Vector2.zero).GetComponent<AttackObject>();
        else
            attackObject = Managers.Resource.GetAttackObject(attackObjectName.ToString());

        attackCollider = attackObject.GetComponent<BoxCollider2D>();

        if (attackObject != null)
        {
            attackObject.Set_TargetTransform(this.transform);

            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, Vector2, ENUM_TEAM_TYPE, bool>
                    (attackObject, attackObject.ActivatingAttackObject, transform.position, teamType, reverseState);
            }
            else
                attackObject.ActivatingAttackObject(transform.position, teamType, reverseState);
        }
        else
        {
            Debug.Log($"ENUM_SKILL_TYPE에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }

        if (FollowingObjectCoroutine != null)
            StopCoroutine(FollowingObjectCoroutine);

        FollowingObjectCoroutine = StartCoroutine(Folling_AttackObejct());
    }

    public void AnimEvent_DestoryMine()
    {
        if (FollowingObjectCoroutine != null)
            StopCoroutine(FollowingObjectCoroutine);

        isFirstHit = false;

        Managers.Resource.Destroy(this.gameObject);
    }

    public IEnumerator Folling_AttackObejct()
    {
        while (attackObject != null)
        {
            attackObject.transform.position = this.transform.position;
            if(!isFirstHit)
                Check_GroundHit();
            yield return null;
        }
    }
}