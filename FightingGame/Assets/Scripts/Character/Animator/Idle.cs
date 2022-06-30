using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : Idle
/// </summary>
public class Idle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 임시 처리
        animator.transform.gameObject.GetComponent<ActiveCharacter>().Idle();
    }
}