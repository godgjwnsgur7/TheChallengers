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
    TestBulletShot tbshot;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        // Effect Object SetActive True When Gun Animation Start
        effect = animator.transform.parent.Find("Effect").gameObject;
        effect.SetActive(true);

        tbshot = animator.transform.GetComponent<TestBulletShot>();
        tbshot.init("gunBullet");
    }
}
