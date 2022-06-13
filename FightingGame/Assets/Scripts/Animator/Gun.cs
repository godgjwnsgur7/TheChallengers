using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightWeapon에 붙어있는 애니메이션 Gun
/// </summary>
public class Gun : StateMachineBehaviour
{
    GameObject effect;
    TestBulletShot tbShot;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        effect = animator.transform.parent.Find("Effect").gameObject;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1)
        {
            // Effect Object SetActive True When Gun Animation Start
            effect.SetActive(true);

            // Bullet Shot
            tbShot = animator.transform.GetComponent<TestBulletShot>();
            tbShot.init("GunBullet");
        }
    }
}
