using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private SpriteRenderer eRenderer;
    private void Awake()
    {
        eRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("EffectCoroutine");
    }

    IEnumerator EffectCoroutine()
    {
        float fadeValue = 1;
        while (fadeValue > 0f) 
        {
            fadeValue -= 0.1f;
            yield return new WaitForSeconds(0.01f);
            eRenderer.color = new Color(255, 255, 255, fadeValue); ;
        }

        gameObject.SetActive(false);
    }
}
