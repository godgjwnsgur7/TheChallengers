using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 애니메이터 KnightWeapon에 붙어있는 애니메이션 Rifle
/// </summary>
public class Rifle : StateMachineBehaviour
{
    GameObject effect;
    TestBulletShot tbShot;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Effect Object SetActive True When Rifle Animation Start
        effect = animator.transform.parent.Find("Effect").gameObject;
        effect.SetActive(true);

        tbShot = animator.transform.GetComponent<TestBulletShot>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (layerIndex == 2) 
        {
            tbShot.init("rifleBullet");
        }
    }
}
