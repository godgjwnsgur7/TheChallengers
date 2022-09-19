using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupCanvas : MonoBehaviour
{
    [SerializeField] BlackOutPopup blackOut;
    [SerializeField] CountDownPopup countDownPopup;

    // 중복으로 요청 시에 리턴
    [SerializeField] SelectPopup selectPopup;
    [SerializeField] NotifyPopup notifyPopup;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        DontDestroyOnLoad(this);
    }

    public void Open_SelectPopup(Action _succeededCallBack, Action _failedCallBack, string _message)
    {
        if(notifyPopup.isUsing)
        {
            Debug.Log("이미 알림팝업창이 사용중입니다.");
            return;
        }

        selectPopup.Open(_succeededCallBack, _failedCallBack, _message);
    }

    public void Open_NotifyPopup(string _message, Action _succeededCallBack = null)
    {
        if (selectPopup.isUsing)
        {
            Debug.Log("이미 선택팝업창이 사용중입니다.");
            return;
        }

        notifyPopup.Open(_message, _succeededCallBack);
    }

    public void Open<T>()
    {
        if (typeof(T) == typeof(BlackOutPopup)) blackOut.Open();
        else if (typeof(T) == typeof(CountDownPopup)) countDownPopup.Open();
        else
        {
            Debug.Log("범위 벗어남");
            return;
        }
    }

    public void Close<T>()
    {
        if (typeof(T) == typeof(BlackOutPopup)) blackOut.Close();
        else if (typeof(T) == typeof(CountDownPopup)) countDownPopup.Close();
        else
        {
            Debug.Log("범위 벗어남");
            return;
        }
    }

}
