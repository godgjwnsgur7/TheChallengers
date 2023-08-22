using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : PopupUI
{
    [SerializeField] Text loadingText;
    [SerializeField] Text tipDescriptionText;

    Coroutine messageEffectCoroutine;
    string message = "LOADING ";

    protected override void OnDisable()
    {
        base.OnDisable();

        if (messageEffectCoroutine != null)
            StopCoroutine(messageEffectCoroutine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
            Application.Quit();
    }

    public void Open()
    {
        if (messageEffectCoroutine != null)
            StopCoroutine(messageEffectCoroutine);

        loadingText.text = message;
        tipDescriptionText.text = "TIP. " + Managers.Data.Get_TipDescription();

        this.gameObject.SetActive(true);

        messageEffectCoroutine = StartCoroutine(IMessageTextEffect());
    }

    public void Close()
    {
        message = "LOADING ";
        loadingText.text = message;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IMessageTextEffect()
    {
        int count = 0;
        string _message;

        while(isUsing)
        {
            yield return new WaitForSeconds(0.2f);

            _message = message;

            for (int i = 0; i < count; i++)
                _message += ".";

            loadingText.text = _message;

            if (count == 3)
            {
                count = 0;
                yield return new WaitForSeconds(0.5f);
            }
            else
                count++;
        }

        messageEffectCoroutine = null;
    }
}
