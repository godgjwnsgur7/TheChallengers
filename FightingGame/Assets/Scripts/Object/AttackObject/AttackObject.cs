using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class AttackObject : SyncObject
{
    protected ENUM_TEAM_TYPE teamType;
    protected bool isMine = true; // 내가 소환한 객체인가?

    public override void Init()
    {
        base.Init();
    }

    [BroadcastMethod]
    public virtual void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        teamType = _teamType;

        Set_PositionAngle(_summonPosVec, _reverseState);

        gameObject.SetActive(true);
    }

    public void Sync_DestroyMine()
    {
        if (!isUsing) return;

        if (isServerSyncState)
            PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject>(this, DestroyMine);
        else
            Managers.Resource.Destroy(gameObject);
    }

    [BroadcastMethod]
    public void DestroyMine()
    {
        if (!isUsing) return;

        Managers.Resource.Destroy(this.gameObject);
    }
}
