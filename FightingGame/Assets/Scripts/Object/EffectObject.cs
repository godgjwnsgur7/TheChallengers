using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class EffectObject : Poolable
{
    public Transform targetTr = null;
    public bool reverseState;

    public Color[] effectColors;
    private SpriteRenderer effectRenderer;

    [SerializeField] Vector3 subPos;

    public override void Init()
    {
        base.Init();

        effectRenderer = this.GetComponent<SpriteRenderer>();

        if (PhotonLogicHandler.IsConnected)
        {
            SyncTransformView(transform);
        }
    }

    [BroadcastMethod]
    public virtual void ActivatingEffectObject(bool _reverseState, int _effectTypeNum)
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
