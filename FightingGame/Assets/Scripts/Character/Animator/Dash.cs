using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        activeCharacter.SetAnimBool("IsDash", false);
    }
}
