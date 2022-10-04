using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using UnityEngine.UI;

public class MatchingWindowUI : MonoBehaviour
{
    [SerializeField] Text stopwatchText;

    Coroutine timerCoroutine;

    bool isStopwatchLock = false;

    public void OnClick_Matching()
    {
        Managers.UI.popupCanvas.Open_CharSelectPopup(MathingStart);
    }

    public void MathingStart(ENUM_CHARACTER_TYPE charType)
    {
        this.gameObject.SetActive(true);
        isStopwatchLock = false;
        timerCoroutine = StartCoroutine(IStopwatch());

        // 매칭 돌리기
    }

    public void OnClick_Exit()
    {
        StopCoroutine(timerCoroutine);
        isStopwatchLock = true;
        this.gameObject.SetActive(false);
    }

    protected IEnumerator IStopwatch()
    {
        float startTime = Time.time;
        float elapsedSecondTime;
        int minute = 0;

        while (!isStopwatchLock)
        {
            elapsedSecondTime = Time.time - startTime;

            if(elapsedSecondTime >= 60.0f)
            {
                minute += 1;
                startTime += 60.0f;
            }

            stopwatchText.text = string.Format("{0:00}:{1:00.00}", minute, elapsedSecondTime);

            yield return null;
        }
    }

}
