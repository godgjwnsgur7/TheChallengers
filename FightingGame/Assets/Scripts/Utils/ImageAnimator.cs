using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField] Image currImage;
    [SerializeField] Sprite[] animationImages;

    Coroutine playAnimationCoroutine;

    private void OnEnable()
    {
        playAnimationCoroutine = StartCoroutine(IPlayAnimation());
    }

    private void OnDisable()
    {
        if (playAnimationCoroutine != null)
            StopCoroutine(playAnimationCoroutine);
    }

    private IEnumerator IPlayAnimation()
    {
        while(currImage != null)
        {
            for(int i = 0; i < animationImages.Length; i++)
            {
                currImage.sprite = animationImages[i];
                yield return new WaitForSeconds(0.05f);
            }
        }

        playAnimationCoroutine = null;
    }
}
