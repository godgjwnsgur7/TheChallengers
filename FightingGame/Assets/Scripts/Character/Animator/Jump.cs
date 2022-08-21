using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : Jump
/// </summary>
public class Jump : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;
    Coroutine coroutine;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        coroutine = CoroutineHelper.StartCoroutine(ICheckCharFall());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        animator.SetBool("IsDrop", false);
        CoroutineHelper.StopCoroutine(coroutine);
    }

    private IEnumerator ICheckCharFall()
    {
        bool dropState = false;

        float curPosY;
        float charPosY = activeCharacter.transform.position.y;

        while (!dropState)
        {
            curPosY = activeCharacter.transform.position.y;
            dropState = (charPosY > curPosY) ? true : false;
            charPosY = curPosY;

            if (dropState)
            {
                activeCharacter.anim.SetBool("IsDrop", true);
            }

            yield return null;
        }
    }

}
