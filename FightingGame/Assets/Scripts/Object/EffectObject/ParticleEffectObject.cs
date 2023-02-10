using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectObject : EffectObject
{
    ParticleSystem particle;
    ParticleSystem.Particle[] particlesElements;

    Coroutine pariclePlayCoroutine;

    public override void OnDisable()
    {
        if (pariclePlayCoroutine != null)
            StopCoroutine(pariclePlayCoroutine);

        base.OnDisable();
    }

    public override void Init()
    {
        base.Init();

        particle = GetComponent<ParticleSystem>();
        particlesElements = new ParticleSystem.Particle[particle.main.maxParticles];
    }

    [BroadcastMethod]
    public override void Activate_EffectObject(Vector2 _summonPosVec, bool _reverseState)
    {
        base.Activate_EffectObject(_summonPosVec, _reverseState);

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
