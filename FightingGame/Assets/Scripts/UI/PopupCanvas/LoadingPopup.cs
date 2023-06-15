using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPopup : PopupUI
{
    [SerializeField] Text popupText;

    Coroutine messageEffectCoroutine;
    string message = string.Empty;

    public void Open(string _message)
    {
        if (messageEffectCoroutine != null)
            StopCoroutine(messageEffectCoroutine);

        message = _message;
        popupText.text = _message;
        this.gameObject.SetActive(true);

        messageEffectCoroutine = StartCoroutine(IMessageTextEffect());
    }

    public void Close()
    {
        message = string.Empty;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IMessageTextEffect()
    {
        int count = 1;
        string _message;

        while(isUsing)
        {
            yield return new WaitForSeconds(0.2f);

            _message = message;

            for (int i = 0; i < count; i++)
                _message += ".";

            popupText.text = _message;

            if (count == 4)
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
