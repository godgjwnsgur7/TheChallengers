using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : Landing
/// </summary>
public class Landing : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Landing 상태의 역할과 Hit 상태의 역할이 같으므로 히트로 만들고, Idle 스테이트에서 재설정 될 것
        ActiveCharacter activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();
        activeCharacter.rigid2D.velocity = Vector2.zero;
        activeCharacter.currState = FGDefine.ENUM_PLAYER_STATE.Hit;
    }
}