using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightBody에 붙어있는 애니메이션 Idle
/// </summary>
public class Idle : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 임시 처리
        animator.transform.parent.gameObject.GetComponent<ActiveCharacter>().Idle(); 
        
    }
}