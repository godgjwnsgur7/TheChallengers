using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class DeadLanding : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        CoroutineHelper.StartCoroutine(IDeadEffect());
    }

    private IEnumerator IDeadEffect()
    {
        if (activeCharacter == null)
            yield break;

        SpriteRenderer charSpriteRenderer = activeCharacter.GetComponent<SpriteRenderer>();
        
        if (charSpriteRenderer == null)
            yield break;

        ENUM_SFX_TYPE sfxType = (ENUM_SFX_TYPE)Enum.Parse(typeof(ENUM_SFX_TYPE), $"{activeCharacter.characterType}_Die{UnityEngine.Random.Range(1, 3)}");
        Managers.Sound.Play_SFX(sfxType);

        Color color = charSpriteRenderer.color;

        yield return new WaitForSeconds(1.5f);

        while (color.a > 0.1f && charSpriteRenderer != null)
        {
            color.a -= 0.01f;
            charSpriteRenderer.color = color;

            yield return null;
        }

        color.a = 0f;
        if(charSpriteRenderer != null)
            charSpriteRenderer.color = color;
        activeCharacter.EndGame();
    }
}
