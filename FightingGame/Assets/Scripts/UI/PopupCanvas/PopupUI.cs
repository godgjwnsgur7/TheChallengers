using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    private bool isPopupActive = false;
    public bool isUsing = false;

    private Coroutine openCoroutine;
    private Coroutine closeCoroutine;
    [SerializeField] GameObject popupWindow;

    public void Open_Effect()
    {
        // 열려있는데 여는건 의미 없다 생각해서 리턴
        if (isPopupActive)
            return;

        // 닫는 걸 중단하고 열기
        if (closeCoroutine != null)
            StopCoroutine(closeCoroutine);


        SetActive(true);

        openCoroutine = StartCoroutine(Open_Coroutine());
    }

    public void Close_Effect()
    {
        // 닫혀 있는데 닫는건 의미없다 생각해 리턴
        if (!isPopupActive)
            return;

        // 여는 걸 중단하고 닫기
        if (openCoroutine != null)
            StopCoroutine(openCoroutine);

        closeCoroutine = StartCoroutine(Close_Coroutine());
    }

    // 코루틴 안에서의 역할같은건 나중에 변할거니까 일단 안읽음

    IEnumerator Open_Coroutine()
    {
        float time = 0;
        Image popupImage = popupWindow.GetComponent<Image>();

        while (time < 1f)
        {
            popupImage.color = new Color(1, 1, 1, time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        ResetCoroutine();
    }

    IEnumerator Close_Coroutine()
    {
        float time = 0;

        while (time < 1f)
        {
            popupWindow.GetComponent<Image>().color = new Color(1, 1, 1, 1f - time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        ResetCoroutine();
        SetActive(false);
    }

    private void ResetCoroutine()
    {
        openCoroutine = null;
        closeCoroutine = null;
    }

    private void SetActive(bool isActive)
    {
        this.gameObject.SetActive(isActive);
        this.isPopupActive = isActive;
    }
}
