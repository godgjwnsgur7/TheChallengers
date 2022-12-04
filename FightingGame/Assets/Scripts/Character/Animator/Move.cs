using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : StateMachineBehaviour
{
    Coroutine currCorutine;
    bool isMove = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currCorutine != null)
            CoroutineHelper.StopCoroutine(currCorutine);

        isMove = true;
        if (animator.gameObject.GetComponent<ActiveCharacter>().isControl)
            currCorutine = CoroutineHelper.StartCoroutine(IPlayerMoveSound());
        else
            currCorutine = CoroutineHelper.StartCoroutine(IEnemyMoveSound());
    }

    IEnumerator IPlayerMoveSound()
    {
        while (isMove)
        {
            Managers.Sound.Play(FGDefine.ENUM_SFX_TYPE.walk, ENUM_SOUND_TYPE.SFX_Player);
            yield return new WaitForSeconds(0.55f);
        }
    }

    IEnumerator IEnemyMoveSound()
    {
        while (isMove)
        {
            Managers.Sound.Play(FGDefine.ENUM_SFX_TYPE.walk, ENUM_SOUND_TYPE.SFX_Enemy);
            yield return new WaitForSeconds(0.55f);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isMove = false;
    }
}
