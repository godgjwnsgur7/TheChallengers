using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class MultiAttackObject : AttackObject
{
    protected AttackObject attackObject;

    public bool isConnected;

    // 이미지 센터가 맞지 않아 임시로 로직처리
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

    public override void ActivatingAttackObject(SyncAttackObjectParam attackObjectParam)
    {
        reverseState = attackObjectParam.reverseState;
        teamType = attackObjectParam.teamType;

        if(reverseState)
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
        attackObject = null;
        ENUM_SKILL_TYPE attackType = (ENUM_SKILL_TYPE)_attackTypeNum;

        if (isConnected)
            attackObject = Managers.Resource.InstantiateEveryone(attackType.ToString(), Vector2.zero).GetComponent<AttackObject>();
        else
            attackObject = Managers.Resource.GetAttackObject(attackType.ToString());

        if (attackObject != null)
        {
            attackObject.transform.position = transform.position;
            
            SyncAttackObjectParam syncAttackObjectParam = new SyncAttackObjectParam(teamType, reverseState, this.transform);
            if (isConnected)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<AttackObject, SyncAttackObjectParam>
                    (attackObject, attackObject.ActivatingAttackObject, syncAttackObjectParam);
            }
            else
                attackObject.ActivatingAttackObject(syncAttackObjectParam);
        }
        else
        {
            Debug.Log($"ENUM_SKILL_TYPE에서 해당 번호를 찾을 수 없음 : {_attackTypeNum}");
        }
    }
}
