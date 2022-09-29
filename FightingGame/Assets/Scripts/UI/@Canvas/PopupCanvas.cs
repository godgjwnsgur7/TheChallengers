using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class PopupCanvas : MonoBehaviour
{
    [SerializeField] CharSelectPopup charSelectPopup;
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
        // 애들 활성화 상태 확인해서 전체를 끈다던가 이런거 생각중
    }

    public void Open_CharSelectPopup(Action<ENUM_CHARACTER_TYPE> _charCallBack)
    {
        if(charSelectPopup.isUsing)
        {
            Debug.Log("이미 캐릭터선택팝업창이 사용중입니다.");
            return;
        }

        charSelectPopup.Open(_charCallBack);
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
    public void Open_NotifyPopup(string _message, Action _checkCallBack = null)
    {
        if (notifyPopup.isUsing)
        {
            notifyPopup.Open_Again(_message, _checkCallBack);
            return;
        }

        notifyPopup.Open(_message, _checkCallBack);
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
