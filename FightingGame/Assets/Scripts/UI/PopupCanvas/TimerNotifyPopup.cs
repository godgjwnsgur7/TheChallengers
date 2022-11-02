using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerNotifyPopup : PopupUI
{
    [SerializeField] Text popupText;

    public void Open(string _message, float _runTime = 2.0f)
    {
        popupText.text = _message;

        this.gameObject.SetActive(true);

        StartCoroutine(ICloseTimeCheck(_runTime));
    }
    public void Open_Again(string _message, float _runTime = 2.0f)
    {
        this.gameObject.SetActive(false);

        Open(_message, _runTime);
    }

    private void Close()
    {
        this.gameObject.SetActive(false);

        popupText.text = null;
    }

    protected IEnumerator ICloseTimeCheck(float runTime)
    {
        yield return new WaitForSeconds(runTime);
        Close();
    }

}
