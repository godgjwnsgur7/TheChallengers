using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    private SpriteRenderer eRenderer;
    // 해당하는 스크립트가 가지고 있는 오브젝트가
    // SetActive(true)가 되면 실행됨.

    private void Awake()
    {
        eRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 코루틴 들어가 주면서~
        StartCoroutine("EffectCoroutine");
    }

    // 시점 제어가 가능, 반복 호출시키는 시점 제어 가능.
    // 비동기 처리처럼 보이나, 사실상 비동기가 아님
    IEnumerator EffectCoroutine()
    {
        // 셋팅 해줘야겠지
        float fadeValue = 1;
        while (fadeValue > 0f) 
        {
            fadeValue -= 0.1f;
            yield return new WaitForSeconds(0.01f);
            eRenderer.color = new Color(255, 255, 255, fadeValue); ;
        }

        // 탈출시에 실행
        // 초기화 시켜야겠지 다시
        gameObject.SetActive(false);
    }
}
