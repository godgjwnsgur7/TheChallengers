using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : StateMachineBehaviour
{
    // 중복으로 들어오는 걸 방지하기 위해

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HitState", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HitState", false);
    }
}
