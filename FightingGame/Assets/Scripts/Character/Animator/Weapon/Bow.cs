using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightWeapon에 붙어있는 애니메이션 Bow
/// </summary>
public class Bow : StateMachineBehaviour
{
    //TestBulletShot tbShot;

    int count;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count = 1;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count++;
        Debug.Log(count);

        if (count == 20)
        {
            animator.transform.parent.gameObject.GetComponent<ActiveCharacter>().Shot();
            if (animator.GetBool("isAttack"))
            {
                animator.transform.parent.gameObject.GetComponent<ActiveCharacter>().SetBool("isAttack", false);
            }
        }
    }
}
