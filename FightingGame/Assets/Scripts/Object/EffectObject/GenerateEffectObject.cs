using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class GenerateEffectObject : EffectObject
{
    protected EffectObject effectObject = null;

    public override void Init()
    {
        base.Init();
    }

    [BroadcastMethod]
    public override void Activate_EffectObject(Vector2 _summonPosVec, bool _reverseState)
    {
        Vector2 updateSummonPosVec = new Vector2(
            _reverseState ? (_summonPosVec.x - subPosVec.x) : (_summonPosVec.x + subPosVec.x)
            , (_summonPosVec.y + subPosVec.y));

        base.Activate_EffectObject(updateSummonPosVec, _reverseState);
    }

    // Animation Event
    public void Summon_EffectObject(int _effectTypeNum)
    {
        ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)_effectTypeNum;

        EffectObject effectObject = null;

        if (isServerSyncState)
            effectObject = Managers.Resource.InstantiateEveryone("EffectObjects/" + effectObjectName.ToString(), Vector2.zero).GetComponent<EffectObject>();
        else
            effectObject = Managers.Resource.GetEffectObject(effectObjectName.ToString());

        if (effectObject != null)
        {
            if (isServerSyncState)
            {
                PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject, Vector2, bool>
                    (effectObject, effectObject.Activate_EffectObject, transform.position, reverseState);
            }
            else
                effectObject.Activate_EffectObject(transform.position, reverseState);
        }
        else
        {
            Debug.Log($"ENUM_EFFECTOBJECT_NAME에서 해당 번호를 찾을 수 없음 : {effectObjectName}");
        }
    }
}
