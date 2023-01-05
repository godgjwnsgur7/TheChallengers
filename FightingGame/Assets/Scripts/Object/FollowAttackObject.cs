using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class FollowAttackObject : AttackObject
{
    public override void Init()
    {
        attackObjectType = ENUM_ATTACKOBJECT_TYPE.Follow;

        base.Init();

        ENUM_ATTACKOBJECT_NAME attackObjectName = (ENUM_ATTACKOBJECT_NAME)Enum.Parse(typeof(ENUM_ATTACKOBJECT_NAME), gameObject.name.ToString());
        if (!Managers.Data.SkillDict.TryGetValue((int)attackObjectName, out skillValue))
        {
            Debug.Log($"{gameObject.name} 를 초기화하지 못했습니다.");
        }
    }

    [BroadcastMethod]
    public override void ActivatingAttackObject(ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.ActivatingAttackObject(_teamType, _reverseState);

        transform.localEulerAngles = reverseState ? new Vector3(0, 180, 0) : Vector3.zero;

        gameObject.SetActive(true);

        isMine = true;

        if (isServerSyncState)
        {
            if (PhotonLogicHandler.IsMine(viewID))
                runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
            else
                isMine = false;
        }
        else
            runTimeCheckCoroutine = StartCoroutine(IRunTimeCheck(skillValue.runTime));
    }



}
