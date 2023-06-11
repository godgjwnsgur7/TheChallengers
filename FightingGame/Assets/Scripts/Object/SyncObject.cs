using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public enum ENUM_SYNCOBJECT_TYPE
{
    Default = 0,
    Follow = 1,
    Fall = 2,
}

public class SyncObject : Poolable
{
    public ENUM_SYNCOBJECT_TYPE ObjType
    {
        protected set;
        get;
    }

    protected ENUM_TEAM_TYPE teamType;

    protected Vector2 summonPosVec;
    protected bool reverseState = false;

    public override void Init()
    {
        base.Init();

        if(isServerSyncState)
        {
            SyncTransformView(transform);
        }
    }

    protected virtual void Set_PositionAngle(Vector2 _summonPosVec, bool _reverseState)
    {
        transform.position = _summonPosVec;
        reverseState = _reverseState;

        transform.localEulerAngles = reverseState ? new Vector3(0, 180, 0) : Vector3.zero;
    }

    [BroadcastMethod]
    public void PlaySFX_HitSound(int sfxTypeNum, Vector3 hitPosVec)
    {
        Debug.Log($"{sfxTypeNum} : {this.gameObject.name} 이 히트 ㅇㅇ");
        Managers.Sound.Play_SFX((ENUM_SFX_TYPE)sfxTypeNum, teamType, hitPosVec);
    }

    protected void AnimEvent_PlaySFX(int sfxTypeNum)
    {
        Managers.Sound.Play_SFX((ENUM_SFX_TYPE)sfxTypeNum, teamType, transform.position);
    }

    protected void AnimEvent_PlaySFXFollowingSound(int sfxTypeNum)
    {
        Managers.Sound.PlaySFX_FollowingSound((ENUM_SFX_TYPE)sfxTypeNum, teamType, transform.position, transform);
    }
}
