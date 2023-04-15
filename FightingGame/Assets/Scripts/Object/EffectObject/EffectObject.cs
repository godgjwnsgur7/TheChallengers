using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : SyncObject
{
    [SerializeField] Vector3 subPos;

    public override void Init()
    {
        base.Init();

    }

    [BroadcastMethod]
    public virtual void Activate_EffectObject(Vector2 _summonPosVec, bool _reverseState)
    {
        Set_PositionAngle(_summonPosVec, _reverseState);

        transform.position += new Vector3(reverseState ? subPos.x * -1 : subPos.x, subPos.y, 0);
        
        gameObject.SetActive(true);
    }

    // 이펙트오브젝트는 따로 사라지는 시점을 동기화처리 하지 않음.
    public void DestoryMine()
    {
        if (!isUsing) return;

        Managers.Resource.Destroy(gameObject);
    }

    public void AnimEvent_DestoryMine()
    {
        DestoryMine();
    }
}
