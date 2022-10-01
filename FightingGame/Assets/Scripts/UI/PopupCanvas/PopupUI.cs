using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;

    private Coroutine openCoroutine;
    private Coroutine closeCoroutine;
    [SerializeField] GameObject popupWindow;

    public void Open_Effect()
    {
        if (this.gameObject.activeSelf)
            return;

        if (closeCoroutine != null)
            StopCoroutine(closeCoroutine);


        this.gameObject.SetActive(true);

        openCoroutine = StartCoroutine(Open_Coroutine());
    }

    public void Close_Effect()
    {
        if (!this.gameObject.activeSelf)
            return;

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

        openCoroutine = null;
    }

    IEnumerator Close_Coroutine()
    {
        float time = 0;
        Image popupImage = popupWindow.GetComponent<Image>();

        while (time < 1f)
        {
            popupImage.color = new Color(1, 1, 1, time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        closeCoroutine = null;
        this.gameObject.SetActive(false);
    }
}
