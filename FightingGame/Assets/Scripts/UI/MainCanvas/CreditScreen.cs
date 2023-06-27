using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScreen : UIElement
{
    [SerializeField] GameObject CreditAreaObject;
    [SerializeField] Image backgroundImage;

    Coroutine creditEffectCoroutine = null;

    readonly float startPosVecY = -1000f;
    readonly float endPosVecY = 5100f;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        if (creditEffectCoroutine != null)
            StopCoroutine(creditEffectCoroutine);

        base.OnDisable();
    }

    public void Open()
    {
        CreditAreaObject.transform.localPosition = new Vector3(0, startPosVecY, 0);
        backgroundImage.color = new Color(0, 0, 0, 0);
        this.gameObject.SetActive(true);

        creditEffectCoroutine = StartCoroutine(ICreditMoveEffect());
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        if (creditEffectCoroutine != null)
            StopCoroutine(creditEffectCoroutine);

        this.gameObject.SetActive(false);
    }

    IEnumerator ICreditMoveEffect()
    {
        Color tempColor = backgroundImage.color;
        float fadeTime = 0.5f;
        
        // 페이드 아웃
        while (tempColor.a < 1f)
        {
            Debug.Log("1");

            tempColor.a += Time.deltaTime / fadeTime;
            backgroundImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 1f;
        backgroundImage.color = tempColor;
        Managers.UI.popupCanvas.DeactivePopupAll();

        float effectTime = 60;
        float runTime = 0f;
        float currCreditPosY;

        // 크레딧 실행
        while (CreditAreaObject.transform.localPosition.y != endPosVecY)
        {
            Debug.Log("2");

            runTime += Time.deltaTime;
            
            currCreditPosY = Mathf.Lerp(startPosVecY, endPosVecY, runTime / effectTime);
            CreditAreaObject.transform.localPosition = new Vector3(0, currCreditPosY, 0);

            yield return null;
        }

        // 페이드 인
        backgroundImage.color = new Color(0, 0, 0, 1);
        tempColor = backgroundImage.color;

        while (tempColor.a > 0f)
        {
            Debug.Log("3");

            tempColor.a -= Time.deltaTime / fadeTime;
            backgroundImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 0f;
        backgroundImage.color = tempColor;
        creditEffectCoroutine = null;

        OnClick_Exit();
    }
}