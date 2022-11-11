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
        SpriteRenderer charSpriteRenderer = activeCharacter.GetComponent<SpriteRenderer>();
        Color color = charSpriteRenderer.color;

        yield return new WaitForSeconds(1f);

        while (color.a > 0.1f)
        {
            color.a -= 0.01f;
            charSpriteRenderer.color = color;

            yield return null;
        }

        color.a = 0f;
        charSpriteRenderer.color = color;
        activeCharacter.EndGame();
    }
}
