using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : 
/// </summary>
public class StandHitImmunity: StateMachineBehaviour
{
    private ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        // 구현해야 함 (임시)
    }

}
