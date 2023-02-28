using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class GenerateAttackObject : AttackObject
{
    [SerializeField] Vector2 subPosVec;

    protected AttackObject attackObject = null;

    public override void Init()
    {
        base.Init();
    }

    [BroadcastMethod]
    public override void Activate_AttackObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        Vector2 updateSummonPosVec = new Vector2(
            _reverseState ? (_summonPosVec.x - subPosVec.x) : (_summonPosVec.x + subPosVec.x)
            , (_summonPosVec.y + subPosVec.y));

        base.Activate_AttackObject(updateSummonPosVec, _teamType, _reverseState);
    }

    // Animation Event
    public void Summon_AttackObject(int _attackTypeNum)
    {
        if (attackObject != null && attackObject.isUsing)
        {
            attackObject.Sync_DestroyMine();
        }

        attackObject = null;

        if (isServerSyncState)
            attackObject = Managers.Resource.InstantiateEveryone(((ENUM_ATTACKOBJECT_NAME)_attackTypeNum).ToString(), transform.position).GetComponent<AttackObject>();
        else
        {
            attackObject = Managers.Resource.GetAttackObject(((ENUM_ATTACKOBJECT_NAME)_attackTypeNum).ToString());
            attackObject.transform.position = transform.position;   
        }
        
        if (attackObject != null)
        {
            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, Vector2, ENUM_TEAM_TYPE, bool>
                    (attackObject, attackObject.Activate_AttackObject, transform.position, teamType, reverseState);
            }
            else
            {
                attackObject.Activate_AttackObject(transform.position, teamType, reverseState);
            }

            if (attackObject.ObjType == ENUM_SYNCOBJECT_TYPE.Follow)
            {
                attackObject.GetComponent<FollowAttackObject>().Set_TargetTransform(transform);
            }
        }
        else
        {
            Debug.Log($"ENUM_SKILL_TYPE에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }
    }

    public void AnimEvent_DestoryMine()
    {
        if (!isUsing) return;

        Managers.Resource.Destroy(this.gameObject);
    }
}
