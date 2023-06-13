using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
