using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

// 낙하해서 충돌하면 이펙트 발생
public class FallAttackObject : AttackObject
{
    Animator anim;
    Rigidbody2D rigid2D;
    AttackObject attackObject;
    BoxCollider2D attackCollider;

    [SerializeField] protected Vector2 shotSpeed;
    [SerializeField] protected Vector3 subPos;

    bool isFirstHit = false;
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
        isFirstHit = false;
    }

    public void Shot_AttackObject() => rigid2D.AddForce(shotSpeed);

    [BroadcastMethod]
    public void Active_Trigger(string _parametorName)
    {
        anim.SetTrigger(_parametorName);
    }

    public void Check_GroundHit()
    {
        // AttackObject size와 Offset 값을 이용해 Ray의 위치를 정해야함
        Vector3 attackObj_HalfWidth = new Vector3(attackCollider.size.x / 2, 0, 0);
        float attackObj_HalfHeight = attackCollider.size.y;
        float attackObj_OffsetY = attackCollider.offset.y;

        // AttackObject 양 끝단 Ray 발사
        Debug.DrawRay(attackObject.transform.position + attackObj_HalfWidth * -1f, Vector2.down * (attackObj_HalfHeight + attackObj_OffsetY * -1f), Color.red);
        Debug.DrawRay(attackObject.transform.position + attackObj_HalfWidth, Vector2.down * (attackObj_HalfHeight + attackObj_OffsetY * -1f), Color.red);

        // 바닥 충돌 검사
        isFirstHit = Physics2D.Raycast(attackObject.transform.position + attackObj_HalfWidth, Vector2.down, attackObj_HalfHeight + attackObj_OffsetY * -1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString())) 
            || Physics2D.Raycast(attackObject.transform.position + attackObj_HalfWidth * -1f, Vector2.down, attackObj_HalfHeight + attackObj_OffsetY * -1f, LayerMask.GetMask(ENUM_LAYER_TYPE.Ground.ToString()));

        if (isFirstHit)
        {
            rigid2D.velocity = Vector2.zero;
            Active_Trigger("Hit");
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