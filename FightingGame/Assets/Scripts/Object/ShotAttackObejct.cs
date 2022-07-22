using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAttackObejct : AttackObejct
{
    public Rigidbody2D rigid2D;

    public override void Init()
    {
        base.Init();
    
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public override void ActivatingAttackObject(GameObject _target, bool _reverseState)
    {
        base.ActivatingAttackObject(_target, _reverseState);

        // 날아가는 힘을 받아야 하는데, 어떻게 받을지 고민중
        float speed = 200.0f;

        if (_reverseState) speed *= -1f;

        rigid2D.AddForce(new Vector2(speed, 0));
    }

    private void A()
    {

    }
}
