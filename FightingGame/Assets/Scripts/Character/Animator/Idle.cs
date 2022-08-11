using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : Idle
/// </summary>
public class Idle : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        activeCharacter.Idle();
        animator.SetBool("IsIdle", false);
    }
}