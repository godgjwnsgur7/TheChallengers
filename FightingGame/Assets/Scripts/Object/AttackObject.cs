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
    Fall = 4,
}

public class AttackObject : Poolable
{
    protected ENUM_TEAM_TYPE teamType;
    protected ENUM_ATTACKOBJECT_TYPE attackObjectType = ENUM_ATTACKOBJECT_TYPE.Default;

    protected Transform targetTr = null;
    protected bool reverseState;
    protected bool isServerSyncState = false;
    protected bool isMine = true;

    public override void Init()
    {
        base.Init();

        isServerSyncState = Managers.Network.isServerSyncState;

        if (isServerSyncState)
        {
            SyncTransformView(transform);
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingAttackObject(Vector2 _targetTr, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        this.transform.position = _targetTr;
        reverseState = _reverseState;
        teamType = _teamType;
    }

    public void Set_TargetTransform(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }
}
