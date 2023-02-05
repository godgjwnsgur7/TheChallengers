using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class FollowAttackObject : HitAttackObject
{
    Coroutine followTargetCoroutine = null;

    public override void Init()
    {
        ObjType = ENUM_SYNCOBJECT_TYPE.Follow;

        base.Init();
    }

    [BroadcastMethod]
    public override void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.Activate_AttackObject(_summonPosVec, _teamType, _reverseState);
        
    }

    public override void OnDisable()
    {
        if (followTargetCoroutine != null)
            StopCoroutine(followTargetCoroutine);

        base.OnDisable();
    }

    public void Set_TargetTransform(Transform _targetTr)
    {
        followTargetCoroutine = StartCoroutine(IFollowTarget(_targetTr));
    }

    private bool IsActivate()
    {
        Debug.Log(this.gameObject.activeSelf && isUsing);
        return this.gameObject.activeSelf && isUsing;
    }

    // 결국에는 FollowAttackObject일 경우에 소환주체의 Transform을 참조해야 함.
    private IEnumerator IFollowTarget(Transform _targetTr)
    {
        while(isUsing)
        {
            this.transform.position = _targetTr.position;
            yield return null;
        }

        followTargetCoroutine = null;
    }
}
