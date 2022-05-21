using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 Body에 붙어있는 애니메이션 Idle
/// </summary>
public class FarAttack : StateMachineBehaviour
{
    public PlayerAnimation playerAnim = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playerAnim == null)
        {
            // 불안불안한데 일단 트라이 (임시)
            playerAnim = animator.gameObject.GetComponent<ActiveCharacter>().playerAnim;
        }

        // playerAnim = 
    }
}
