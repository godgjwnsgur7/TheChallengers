using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (activeCharacter == null || !activeCharacter.isControl) return;

        activeCharacter.SetAnimBool("HitState", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (activeCharacter == null || !activeCharacter.isControl) return;

        activeCharacter.SetAnimBool("HitState", false);
    }
}
