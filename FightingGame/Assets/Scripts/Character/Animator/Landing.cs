using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Core : Jump - Landing
/// </summary>
public class Landing : StateMachineBehaviour
{
    private ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        activeCharacter.rigid2D.velocity = Vector2.zero;
        activeCharacter.currState = FGDefine.ENUM_PLAYER_STATE.Idle;
    }

}