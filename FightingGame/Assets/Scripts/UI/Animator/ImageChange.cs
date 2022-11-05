using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : StateMachineBehaviour
{
    [SerializeField] Sprite changeSprite;

    Image myImageComponent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myImageComponent == null)
            myImageComponent = animator.GetComponent<Image>();

        myImageComponent.sprite = changeSprite;
    }
}
