using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightWeapon에 붙어있는 애니메이션 Bow
/// </summary>
public class Bow : StateMachineBehaviour
{
    TestBulletShot tbShot;

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1)
        {
            tbShot = animator.transform.GetComponent<TestBulletShot>();
            tbShot.init("BowArrow");
        }
    }
}
