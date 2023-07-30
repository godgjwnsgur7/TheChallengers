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
    Explode = 2, // 폭발
}

public class FallAttackObject : GenerateAttackObject
{
    Animator anim;
    protected Rigidbody2D rigid2D;

    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] Vector2 shotPowerVec;
    
    ENUM_FALLOBJECTSTATE_TYPE currMyState = ENUM_FALLOBJECTSTATE_TYPE.Generate;

    Coroutine explodeCheckCoroutine;

    public override void OnDisable()
    {
        base.OnDisable();

        if (explodeCheckCoroutine != null)
            StopCoroutine(explodeCheckCoroutine);

        boxCollider.enabled = false;
    }

    public override void Init()
    {
        base.Init();

        if (anim == null)
            anim = GetComponent<Animator>();

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if(boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

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

    [BroadcastMethod]
    public override void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.Activate_AttackObject(_summonPosVec, _teamType, _reverseState);

        if(PhotonLogicHandler.IsMine(ViewID))
        {
            boxCollider.enabled = false;
            Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Generate);
        }
    }

    private void Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE fallObjectState)
    {
        if (!PhotonLogicHandler.IsMine(viewID))
            return;

        currMyState = fallObjectState;

        if(rigid2D == null)
        {
            Managers.Resource.Destroy(this.gameObject);
            return;
        }
        
        if (currMyState == ENUM_FALLOBJECTSTATE_TYPE.Fall)
        {
            Vector2 updateShotPowerVec = new Vector2(reverseState ? shotPowerVec.x * -1f : shotPowerVec.x, shotPowerVec.y);
            rigid2D.AddForce(updateShotPowerVec);
            explodeCheckCoroutine = StartCoroutine(IExplodeCheck());
        }
        else if (currMyState == ENUM_FALLOBJECTSTATE_TYPE.Explode)
        {
            rigid2D.velocity = Vector2.zero;
            boxCollider.enabled = false;
        }

        SetAnimTrigger(fallObjectState.ToString() + "Trigger");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ENUM_TAG_TYPE.Ground.ToString())
        {
            Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Explode);
        }
    }

    protected IEnumerator IExplodeCheck()
    {
        float currPosY = this.transform.position.y;

        yield return new WaitUntil(() =>
        (currPosY - 2f > transform.position.y) ||
        currMyState != ENUM_FALLOBJECTSTATE_TYPE.Fall);

        if(currMyState == ENUM_FALLOBJECTSTATE_TYPE.Fall)
            boxCollider.enabled = true;
        explodeCheckCoroutine = null;
    }

    public void AnimEvent_Falling()
    {
        Set_AnimTrigger(ENUM_FALLOBJECTSTATE_TYPE.Fall);
    }
}