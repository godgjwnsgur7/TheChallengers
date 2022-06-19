using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("isAttack"))
        {
            animator.transform.parent.gameObject.GetComponent<ActiveCharacter>().SetBool("isAttack", false);
        }
    }
}
