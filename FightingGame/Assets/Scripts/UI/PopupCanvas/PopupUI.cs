using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUI : MonoBehaviour
{
    public bool isUsing = false;
    [SerializeField] GameObject popupWindow;
    private float time = 0; // 왜 얘는 나와서 공용으로 사용함?

    Coroutine currCoroutine; // 얘 왜 미사용하는데 사용..?
    private bool isRunning = false;

    // 변수명 순서가 조금 불편하네 ㅋ 타입끼리 묶여있으면 좋겠네

    private void OnEnable()
    {
        // 여기에서 활성화될때마다 초기화하는거보다 Close할때 초기화가 낫지않나.?

        isRunning = false;
        currCoroutine = null;
        time = 0;
    }

    public void Open_Effect()
    {
        if (isRunning)
        {
            Debug.Log("실행중인 코루틴이 있습니다.");
            // 실행중인 코루틴이 있으면 여기서 리턴을 시켜버리는건 옳지 않을듯
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
            // 실행중인 코루틴이 있으면 여기서 리턴을 시켜버리는건 옳지 않을듯
            return;
        }

        currCoroutine = StartCoroutine(Close_Coroutine());
    }

    // 코루틴 안에서의 역할같은건 나중에 변할거니까 일단 안읽음

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
