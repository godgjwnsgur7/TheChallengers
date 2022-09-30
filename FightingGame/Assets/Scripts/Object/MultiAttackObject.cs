using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MultiAttackObject : AttackObject
{
    protected AttackObject attackObject;

    public bool isConnected;

    [SerializeField] Vector3 subPos;

    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Multi;

        isConnected = PhotonLogicHandler.IsConnected;

        if (isConnected)
        {
            SyncTransformView(transform);
        }
    }

	[BroadcastMethod]
    public override void ActivatingAttackObject(ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        reverseState = _reverseState;
        teamType = _teamType;

        if (reverseState)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(subPos.x * -1.0f, subPos.y, 0);
        }
        else
        {
            transform.localEulerAngles = Vector3.zero;
            transform.position += subPos;
        }

        gameObject.SetActive(true);
    }

    protected void Summon_AttackObject(int _attackTypeNum)
    {
        bool isControl = true;
        
        if(isConnected)
            isControl = PhotonLogicHandler.IsMine(viewID);

        if (!isControl) return;

        attackObject = null;
        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)_attackTypeNum;

        if (isConnected)
            attackObject = Managers.Resource.InstantiateEveryone(attackObjectName.ToString(), Vector2.zero).GetComponent<AttackObject>();
        else
            attackObject = Managers.Resource.GetAttackObject(attackObjectName.ToString());

        if (attackObject != null)
        {
            attackObject.FollowingTarget(this.transform);

            if (isConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, ENUM_TEAM_TYPE, bool>
                    (attackObject, attackObject.ActivatingAttackObject, teamType, reverseState);
            }
            else
                attackObject.ActivatingAttackObject(teamType, reverseState);
        }
        else
        {
            Debug.Log($"ENUM_SKILL_TYPE에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }
    }
}
