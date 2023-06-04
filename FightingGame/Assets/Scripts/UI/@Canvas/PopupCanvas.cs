using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

/// <summary>
/// 최초 한번만 생성되고, 게임 종료까지 파괴되지 않는 캔버스
/// </summary>
public class PopupCanvas : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] InputKeyController inputKeyController;
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] SettingWindow settingWindow;
    [SerializeField] CharSelectPopup charSelectPopup;
    [SerializeField] SelectPopup selectPopup;
    [SerializeField] NotifyPopup notifyPopup;
    [SerializeField] LoadingPopup loadingPopup;
    [SerializeField] ErrorPopup errorPopup;
    [SerializeField] FadeEffectPopup fadeEffectPopup;
    [SerializeField] TimerNotifyPopup timerNotifyPopup;
    [SerializeField] GameObject touchProtection;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        DontDestroyOnLoad(this);
    }

    public InputKeyManagement Get_InputKeyManagement()
    {
        return inputKeyManagement;
    }

    public InputKeyController Get_InputKeyController()
    {
        return inputKeyController;
    }

    public void Open_SettingWindow()
    {
        settingWindow.Open();
    }

    /// <summary>
    /// True : 완전히 어두워진 상태
    /// </summary>
    public bool Get_FadeState()
    {
        return fadeEffectPopup.Get_FadeState();
    }

    /// <summary>
    /// 에러코드와 메세지를 출력하는 Popup Window
    /// </summary>
    public void Open_ErrorPopup(short _returnCode, string _message)
    {
        if(errorPopup.isUsing)
        {
            Debug.Log("이미 에러팝업창이 떠있습니다.");
            return;
        }

        errorPopup.Open(_returnCode, _message);
    }

    /// <summary>
    /// 캐릭터 선택창 Popup Window
    /// </summary>
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
            _failedCallBack?.Invoke();
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
            _checkCallBack?.Invoke();
            notifyPopup.Open_Again(_message, _checkCallBack);
            return;
        }

        notifyPopup.Open(_message, _checkCallBack);
    }

    /// <summary>
    /// 일정 시간 뒤에 자동으로 사라지는 알림창 Popup Window 
    /// 알림 팝업창은 중복해서 호출 시에 새로운 창으로 갱신됨
    /// </summary>
    public void Open_TimeNotifyPopup(string _message, float _runTime, Action _timeOutCallBack = null)
    {
        if (timerNotifyPopup.isUsing)
        {
            _timeOutCallBack?.Invoke();
            timerNotifyPopup.Open_Again(_message, _runTime);
            return;
        }

        timerNotifyPopup.Open(_message, _runTime);
    }

    /// <summary>
    /// 반드시 FadeIn을 따로 호출해주어야 함
    /// 마스터 클라이언트에 의해 끌려가는 경우가 있기 때문
    /// </summary>
    public void Play_FadeOutEffect(Action _fadeOutCallBack = null)
    {
        if (fadeEffectPopup.isUsing)
        {
            Debug.Log("fadeEffect is Using!!");
            _fadeOutCallBack?.Invoke();
			return;
        }

        fadeEffectPopup.Play_FadeOutEffect(_fadeOutCallBack);
    }

    /// <summary>
    /// 페이드아웃 상태가 아니면, 페이드인 기능은 수행하지 않음.
    /// 콜백은 실행됨
    /// </summary>
    public void Play_FadeInEffect(Action _fadeInCallBack = null)
    {
        if(!fadeEffectPopup.gameObject.activeSelf)
        {
            Debug.Log("페이드아웃 상태가 아닌데, 페이드인이 들어옴");
            _fadeInCallBack?.Invoke();
            return;
        }

        fadeEffectPopup.Play_FadeInEffect(_fadeInCallBack);
    }

    /// <summary>
    /// 페이드아웃 -> 콜백실행 -> 페이드인
    /// </summary>
    public void Play_FadeOutInEffect(Action _fadeOutInCallBack)
    {
        if (fadeEffectPopup.isUsing)
        {
            Debug.Log("fadeEffect is Using!!");
            _fadeOutInCallBack?.Invoke();
            return;
        }

        fadeEffectPopup.Play_FadeOutInEffect(_fadeOutInCallBack);
    }

    /// <summary>
    /// 로딩 팝업창 Popup Window
    /// 반드시 Close를 따로 호출해주어야 함
    /// </summary>
    public void Open_LoadingPopup(string _message)
    {
        loadingPopup.Open(_message);
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
