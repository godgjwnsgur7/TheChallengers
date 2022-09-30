using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;
    [SerializeField] GameObject popupWindow;
    private float time = 0;

    Coroutine currCoroutine;
    private bool isRunning = false;

    // 팝업 UI가 어떤 크기든 맞춰서 Popup Window의 등장 이펙트 사라지는 이펙트 구현
    // 등장하고 사라지는데에는 0.5초 ~ 1초 사이의 시간 안에 할 것.
    // 이쁘게 만들어주세엽 (우진)
    // + 활성화 시키는 시점도 여기서 제어할 예정

    private void OnEnable()
    {
        isRunning = false;
        currCoroutine = null;
        time = 0;
    }

    public void Open_Effect()
    {
        if (isRunning)
        {
            Debug.Log("실행중인 코루틴이 있습니다.");
            return;
        }

        this.gameObject.SetActive(true);

        currCoroutine = StartCoroutine(Open_Coroutine());
    }

    public void Close_Effect()
    {
        if (isRunning)
        {
            Debug.Log("실행중인 코루틴이 있습니다.");
            return;
        }

        currCoroutine = StartCoroutine(Close_Coroutine());
    }

    IEnumerator Open_Coroutine()
    {
        isRunning = true;

        while (time < 1f)
        { 
            popupWindow.GetComponent<Image>().color = new Color(1, 1, 1, time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        isRunning = false;
    }

    IEnumerator Close_Coroutine()
    {
        isRunning = true;

        while (time < 1f)
        {
            popupWindow.GetComponent<Image>().color = new Color(1, 1, 1, 1f - time / 1);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        isRunning = false;
        this.gameObject.SetActive(false);
    }
}
