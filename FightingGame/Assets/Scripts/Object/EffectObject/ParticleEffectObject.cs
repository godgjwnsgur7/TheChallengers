using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ParticleEffectObject : EffectObject
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] Renderer paricleRenderer;

    Coroutine pariclePlayCoroutine;
    
    public override void Init()
    {
        base.Init();

        particle = GetComponent<ParticleSystem>();
    }

    public override void OnEnable()
    {
        paricleRenderer.sortingOrder = Managers.OrderLayer.Get_SequenceOrderLayer(ENUM_OBJECTLAYERLEVEL_TYPE.Untagged);

        base.OnEnable();
    }

    public override void OnDisable()
    {
        if (pariclePlayCoroutine != null)
            StopCoroutine(pariclePlayCoroutine);

        Managers.OrderLayer.Return_OrderLayer(ENUM_OBJECTLAYERLEVEL_TYPE.Untagged, paricleRenderer.sortingOrder);

        base.OnDisable();
    }


    [BroadcastMethod]
    public override void Activate_EffectObject(Vector2 _summonPosVec, ENUM_TEAM_TYPE _teamType, bool _reverseState)
    {
        base.Activate_EffectObject(_summonPosVec, _teamType, _reverseState);

        pariclePlayCoroutine = StartCoroutine(IPlayParicle());
    }

    protected IEnumerator IPlayParicle()
    {
        particle.Play();

        while(particle.isPlaying || particle.IsAlive())
        {
            yield return null;
        }

        pariclePlayCoroutine = null;
        DestoryMine();
    }
}
