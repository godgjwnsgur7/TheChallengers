using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PopupCanvas : MonoBehaviour
{
    // 중복으로 요청 시에 리턴
    [SerializeField] SelectPopup selectPopup;
    [SerializeField] NotifyPopup notifyPopup;
    [SerializeField] LoadingPopup loadingPopup;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        DontDestroyOnLoad(this);
    }

    public void Check_ActivePopup()
    {

    }

    /// <summary>
    /// 예, 아니오 선택창 Popup Window 
    /// 해당 버튼의 Action이 없을 경우 null
    /// </summary>
    public void Open_SelectPopup(Action _succeededCallBack, Action _failedCallBack, string _message)
    {
        if(selectPopup.isUsing)
        {
            Debug.Log("이미 선택팝업창이 사용중입니다.");
            return;
        }

        selectPopup.Open(_succeededCallBack, _failedCallBack, _message);
    }

    /// <summary>
    /// 알림창 Popup Window 
    /// 해당 버튼의 Action이 없을 경우 null
    /// 알림 팝업창은 중복해서 호출 시에 새로운 창으로 갱신됨
    /// </summary>
    public void Open_NotifyPopup(string _message, Action _succeededCallBack = null)
    {
        if (notifyPopup.isUsing)
        {
            notifyPopup.Open_Again(_message, _succeededCallBack);
            return;
        }

        notifyPopup.Open(_message, _succeededCallBack);
    }

    /// <summary>
    /// 로딩 팝업창 Popup Window
    /// 반드시 Close를 따로 호출해주어야 함
    /// </summary>
    public void Open_LoadingPopup()
    {
        if(loadingPopup.isUsing)
        {
            Debug.Log("이미 선택팝업창이 사용중입니다.");
            return;
        }

        loadingPopup.Open();
    }

    public void Close_LoadingPopup()
    {
        if (!loadingPopup.isUsing)
        {
            Debug.Log("팝업창이 사용중이지 않습니다.");
            return;
        }

        loadingPopup.Close();
    }
}
