using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class ShotAttackObject : HitAttackObject
{
    Rigidbody2D rigid2D;

    [SerializeField] Vector3 subPosVec;
    [SerializeField] Vector2 shotPower;

    public override void Init()
    {
        base.Init();

        if (rigid2D == null)
            rigid2D = GetComponent<Rigidbody2D>();

        if (isServerSyncState)
        {
            SyncPhysics(rigid2D);
        }
    }

    [BroadcastMethod]
    public override void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        Vector2 updateSummonPosVec = new Vector2(
            _reverseState ? (_summonPosVec.x - subPosVec.x) : (_summonPosVec.x + subPosVec.x)
            , (_summonPosVec.y + subPosVec.y));

        base.Activate_AttackObject(updateSummonPosVec, _teamType, _reverseState);

        rigid2D.AddForce(new Vector2(_reverseState ? shotPower.x * -1.0f : shotPower.x, shotPower.y));
    }
}
