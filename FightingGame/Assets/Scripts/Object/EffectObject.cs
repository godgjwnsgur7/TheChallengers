using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class EffectObject : Poolable
{
    public Transform targetTr = null;
    public bool reverseState;

    private Color[] effectColors = {Color.white}; // 일단 흰색만
    private SpriteRenderer effectRenderer;

    [SerializeField] Vector3 subPos;

    public override void Init()
    {
        base.Init();

        effectRenderer = this.GetComponent<SpriteRenderer>();

        if (Managers.Battle.isServerSyncState)
        {
            SyncTransformView(transform);
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingEffectObject(Vector2 _targetTr, bool _reverseState, int _effectTypeNum)
    {
        isUsing = true;
        reverseState = _reverseState;
        transform.position = _targetTr;

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

        // HitEffect 색 변경(임시)
        if (_effectTypeNum < (int)ENUM_EFFECTOBJECT_NAME.Knight_SmokeEffect_Jump)
            ChangeEffectColor();

        gameObject.SetActive(true);
    }

    public virtual void ChangeEffectColor()
    {
        int colorNum = UnityEngine.Random.Range(0, effectColors.Length);
        effectRenderer.color = effectColors[colorNum];
    }

    public void AnimEvent_DestoryMine()
    {
        if (!this.gameObject.activeSelf) return;

        isUsing = false;
        targetTr = null;

        Managers.Resource.Destroy(gameObject);
    }

    public void Set_Position(Transform _targetTr)
    {
        targetTr = _targetTr;
        this.transform.position = targetTr.position;
    }

    public void Set_Position(Vector2 _targetPos)
    {
        this.transform.position = _targetPos;
    }
}
