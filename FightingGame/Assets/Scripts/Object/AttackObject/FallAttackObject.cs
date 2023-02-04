using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

// 해당 스크립트에서만 사용할 예정인 열거형
public enum ENUM_FALLOBJECTSTATE_TYPE
{
    Generate = 0, // 생성
    Fall = 1, // 낙하 (Loop)
    Explode = 2, // 충돌
}

// 낙하해서 충돌하면 이펙트 발생
public class FallAttackObject : GenerateAttackObject
{
    Animator anim;
    protected Rigidbody2D rigid2D;
    [SerializeField] Vector2 shotPowerVec;
    ENUM_FALLOBJECTSTATE_TYPE currMyState = ENUM_FALLOBJECTSTATE_TYPE.Generate;

    Coroutine groundHitCheckCoroutine = null;
    AttackObject attackObject = null;

    public override void Init()
    {
        base.Init();

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();

        var param = MakeSyncAnimParam();
        SyncAnimator(anim, param);

        if (isServerSyncState)
        {
            SyncPhysics(rigid2D);
        }
    }

    private AnimatorSyncParam[] MakeSyncAnimParam()
    {
        AnimatorSyncParam[] syncParams = new AnimatorSyncParam[]
        {
            new AnimatorSyncParam("GenerateTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("FallTrigger", AnimParameterType.Trigger),
            new AnimatorSyncParam("ExplodeTrigger", AnimParameterType.Trigger),
        };

        return syncParams;
    }

    public override void OnDisable()
    {
        if(groundHitCheckCoroutine != null)
            StopCoroutine(groundHitCheckCoroutine);

        base.OnDisable();
    }

    [BroadcastMethod]
    public override void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        currMyState = ENUM_FALLOBJECTSTATE_TYPE.Generate;

        base.Activate_AttackObject(_summonPosVec, _teamType, _reverseState);

        Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Generate);
    }

    public override void Summon_AttackObject(int _attackTypeNum)
    {
        if (!isMine) return;

        if(attackObject != null && attackObject.isUsing)
        {
            attackObject.Sync_DestroyMine();
        }

        base.Summon_AttackObject(_attackTypeNum);
    }

    private void Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE fallObjectState)
    {
        SetAnimTrigger(fallObjectState.ToString() + "Trigger");
        currMyState = fallObjectState;

        if(fallObjectState == ENUM_FALLOBJECTSTATE_TYPE.Fall)
        {
            Vector2 updateShotPowerVec = new Vector2(reverseState ? shotPowerVec.x * -1f : shotPowerVec.x, shotPowerVec.y);
            rigid2D.AddForce(updateShotPowerVec);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currMyState != ENUM_FALLOBJECTSTATE_TYPE.Fall)
            return;

        // 낙하상태에서 바닥과 닿았다.
        if(collision.tag == ENUM_TAG_TYPE.Ground.ToString())
        {
            Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Explode);
            rigid2D.velocity = Vector2.zero;
        }
    }

    public void AnimEvent_Falling()
    {
        Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Fall);
    }

    /* OnTriggerEnter2D로 대체. 일단 코드는 남겨놓자.
    public void Check_GroundHit()
    {
        // AttackObject size와 Offset 값을 이용해 Ray의 위치를 정해야함
        Vector3 attackObj_HalfWidth = new Vector3(attackCollider.size.x / 2, 0, 0);
        float attackObj_HalfHeight = attackCollider.size.y / 2;
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

            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<FallAttackObject, string>
                    (this, Active_Trigger, "BoomTrigger");
            }
            else
                Active_Trigger("BoomTrigger");

            Managers.Resource.Destroy(attackObject.gameObject);
        }
    }
    */
}