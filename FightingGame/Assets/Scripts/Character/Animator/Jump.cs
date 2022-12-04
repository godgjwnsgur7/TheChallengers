using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject.GetComponent<ActiveCharacter>().isControl)
            Managers.Sound.Play(FGDefine.ENUM_SFX_TYPE.walkbeep1, ENUM_SOUND_TYPE.SFX_Player);
        else
            Managers.Sound.Play(FGDefine.ENUM_SFX_TYPE.walkbeep1, ENUM_SOUND_TYPE.SFX_Enemy);
    }
}
