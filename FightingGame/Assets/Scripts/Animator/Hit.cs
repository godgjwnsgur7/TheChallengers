using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightWeapon에 붙어있는 애니메이션 Hit
/// </summary>
public class Hit : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.parent.Find("Effect").gameObject.activeSelf == true)
        {
            animator.transform.parent.Find("Effect").gameObject.SetActive(false);
        }
    }
}