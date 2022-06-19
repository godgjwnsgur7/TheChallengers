using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightBody에 붙어있는 애니메이션 Hit
/// </summary>
public class Hit : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("isHit"))
            animator.transform.parent.gameObject.GetComponent<ActiveCharacter>().SetBool("isHit", false);
    }
}