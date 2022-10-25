using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ShotAttackObject : AttackObject
{
    Rigidbody2D rigid2D;
    [SerializeField] Vector3 subPos;
    [SerializeField] Vector2 shotSpeed;

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
        reverseState = _reverseState;
        teamType = _teamType;
        Vector2 shotVec = shotSpeed;

        if (reverseState)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.position += new Vector3(subPos.x * -1.0f, subPos.y, 0);
            shotVec.x *= -1f; 
        }
        else
        {
            transform.localEulerAngles = Vector3.zero;
            transform.position += subPos;
        }

        gameObject.SetActive(true);

        if (PhotonLogicHandler.IsConnected)
            if (PhotonLogicHandler.IsMine(viewID))
                CoroutineHelper.StartCoroutine(IRunTimeCheck(skillValue.runTime));
        else
            CoroutineHelper.StartCoroutine(IRunTimeCheck(skillValue.runTime));

        rigid2D.AddForce(shotVec);
    }
}
