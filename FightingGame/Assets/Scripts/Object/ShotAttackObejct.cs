using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ShotAttackObejct : AttackObejct
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
            SyncTransformView(transform);
        }
    }

    public override void ActivatingAttackObject(SyncAttackObjectParam attackObjectParam)
    {
        base.ActivatingAttackObject(attackObjectParam);

        // 날아가는 힘을 받아야 하는데, 고민중

        float speed = 500.0f;

        if (attackObjectParam.reverseState) speed *= -1f;

        rigid2D.AddForce(new Vector2(speed, 0));
    }
}
