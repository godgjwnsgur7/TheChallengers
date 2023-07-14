using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScreen : UIElement
{
    [SerializeField] GameObject CreditAreaObject;
    [SerializeField] Text skipText;
    [SerializeField] Image backgroundImage;
    [SerializeField] Rigidbody2D rigid2D;

    Coroutine creditEffectCoroutine = null;

    readonly float startPosVecY = -1000f;
    readonly float endPosVecY = 5100f;

    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.Platform.ShowBanner();
    }

    protected override void OnDisable()
    {
        if (creditEffectCoroutine != null)
            StopCoroutine(creditEffectCoroutine);

        base.OnDisable();

        Managers.Platform.HideBanner();
    }

    public void Open()
    {
        if(rigid2D == null)
            rigid2D = CreditAreaObject.GetComponent<Rigidbody2D>();

        CreditAreaObject.transform.localPosition = new Vector3(0, startPosVecY, 0);
        backgroundImage.color = new Color(0, 0, 0, 0);
        skipText.color = new Color(skipText.color.r, skipText.color.g, skipText.color.b, 0);
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
            tempColor.a += Time.deltaTime / fadeTime;
            backgroundImage.color = tempColor;

            yield return null;
        }

        tempColor.a = 1f;
        backgroundImage.color = tempColor;
        Managers.UI.popupCanvas.DeactivePopupAll();

        // 크레딧 실행
        rigid2D.AddForce(new Vector2(0, 1f), ForceMode2D.Impulse);

        tempColor = skipText.color;
        
        // skip 버튼 On.
        while (tempColor.a < 1f)
        {
            tempColor.a += Time.deltaTime / fadeTime;
            skipText.color = tempColor;

            yield return null;
        }

        tempColor.a = 1f;
        skipText.color = tempColor;

        // 크레딧 종료 대기
        yield return new WaitUntil(() => CreditAreaObject.transform.localPosition.y > endPosVecY);
        rigid2D.velocity = Vector2.zero;

        // 페이드 인
        backgroundImage.color = new Color(0, 0, 0, 1);
        tempColor = backgroundImage.color;

        while (tempColor.a > 0f)
        {
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