using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : StateMachineBehaviour
{
    ActiveCharacter activeCharacter;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (activeCharacter == null)
            activeCharacter = animator.transform.gameObject.GetComponent<ActiveCharacter>();

        if (!activeCharacter.isControl) return;

        activeCharacter.currState = FGDefine.ENUM_PLAYER_STATE.Move;
    }
}
