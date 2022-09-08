using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ShotAttackObejct : AttackObject
{
    public Rigidbody2D rigid2D;
    
    public override void Init()
    {
        base.Init();

        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Shot;

        rigid2D = GetComponent<Rigidbody2D>();
        
        if (PhotonLogicHandler.IsConnected)
        {
            SyncPhysics(rigid2D);
        }
    }

    [BroadcastMethod]
    public override void ActivatingAttackObject(ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        targetTr = null;

        base.ActivatingAttackObject(_teamType, _reverseState);

        // 날아가는 힘을 받아야 하는데, 고민중

        float speed = 500.0f;

        if (_reverseState) speed *= -1f;

        rigid2D.AddForce(new Vector2(speed, 0));
    }
}
