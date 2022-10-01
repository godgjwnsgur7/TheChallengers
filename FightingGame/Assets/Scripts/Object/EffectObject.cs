using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class EffectObject : Poolable
{
    public Transform targetTr = null;
    public bool reverseState;

    [SerializeField] Vector3 subPos;

    public override void Init()
    {
        base.Init();

        if (PhotonLogicHandler.IsConnected)
        {
            SyncTransformView(transform);
        }
    }

    public virtual void ActivatingEffectObject(bool _reverseState)
    {
        isUsing = true;
        reverseState = _reverseState;

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

    public virtual void DestroyMine()
    {
        if (!this.gameObject.activeSelf) return;

        isUsing = false;
        targetTr = null;

        if (PhotonLogicHandler.IsConnected)
            PhotonLogicHandler.Instance.TryBroadcastMethod<EffectObject>(this, Sync_DestroyMine);
        else
            Managers.Resource.Destroy(gameObject);

    }

    [BroadcastMethod]
    public virtual void Sync_DestroyMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }

    public void FollowingTarget(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    // AttackObject를 참고하여 이펙트오브젝트를 만들 것.
    // 어택오브젝트와 이펙트오브젝트는 엄연히 다른 역할을 할 것.
    // 그래서 비슷한 구조를 띄나 결국 다르게 동작할 것임

    // 스크립트는 해당 스크립트로 하고, 만약 여러 스크립트가 필요 시에는
    // AttackObject를 분할해놓은 것처럼 할 것.
    // 분할 할 경우엔 ENUM_EFFECTOBJECT_TYPE으로 선언하고,
    // 모든 열거형은 해당 스크립트에 일단 모아놓을 것.!

    // 가져다 쓰는 건 준혁이 할 것.

    // Summon_AttackObject와 비슷하게 애니메이션 이벤트를 이용해서
    // Summon_EffectObject 같은 함수를 선언하여 사용할 것.
    // 관련해서 어택오브젝트나 액티브캐릭터 코드리딩 중에 질문이 있다면, 질문하면 됨
    // (우진)
}
