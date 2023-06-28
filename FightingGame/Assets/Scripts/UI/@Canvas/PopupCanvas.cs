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
    Canvas canvas = null;
    CanvasScaler scaler = null;
    bool isInit = false;

    [Header("Set In Editor")]
    [SerializeField] InputKeyController inputKeyController;
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] SettingWindow settingWindow;
    [SerializeField] MapSelectPopup mapSelectPopup;
    [SerializeField] CharSelectPopup charSelectPopup;
    [SerializeField] SelectPopup selectPopup;
    [SerializeField] NotifyPopup notifyPopup;
    [SerializeField] LoadingPopup loadingPopup;
    [SerializeField] ErrorPopup errorPopup;
    [SerializeField] FadeEffectPopup fadeEffectPopup;
    [SerializeField] TimerNotifyPopup timerNotifyPopup;
    [SerializeField] GameObject touchProtection;

    public bool isFadeObjActiveState
    {
        get { return fadeEffectPopup.gameObject.activeSelf; }
    }

    public void Init()
    {
        scaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<Canvas>();

        SetCanvas();
        SetCanvasScaler();
        SetResolution();

        if (isInit)
            return;
        
        isInit = true;
        DontDestroyOnLoad(this);
    }

    private void SetCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 1100;
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1.0f;
    }

    public void SetResolution()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

        OnPreCull();
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    public InputKeyManagement Get_InputKeyManagement()
    {
        return inputKeyManagement;
    }

    public InputKeyController Get_InputKeyController()
    {
        return inputKeyController;
    }

    /// <summary>
    /// 떠 있는 팝업을 모두 종료 // 페이드 효과, 로딩 제외
    /// </summary>
    public void DeactivePopupAll()
    {
        if (inputKeyController.gameObject.activeSelf) inputKeyController.Close();
        if (inputKeyManagement.gameObject.activeSelf) inputKeyManagement.Close();
        if (settingWindow.gameObject.activeSelf) settingWindow.Close();

        if (mapSelectPopup.isUsing) mapSelectPopup.OnClick_Exit();
        if (charSelectPopup.isUsing) charSelectPopup.OnClick_Exit();
        if (selectPopup.isUsing) selectPopup.OnClick_Exit();
        if (notifyPopup.isUsing) notifyPopup.OnClick_Exit();
        if (loadingPopup.isUsing) loadingPopup.OnClick_Exit();
        if (errorPopup.isUsing) errorPopup.OnClick_Exit();
        if (timerNotifyPopup.isUsing) timerNotifyPopup.OnClick_Exit();        
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

    public void Open_MapSelectPopup(Action<ENUM_MAP_TYPE> _mapCallBack)
    {
        if (mapSelectPopup.isUsing)
        {
            Debug.Log("이미 맵 선택창이 사용중입니다.");
            return;
        }

        mapSelectPopup.Open(_mapCallBack);
    }

    /// <summary>
    /// 캐릭터 선택창 Popup Window
    /// </summary>
    public void Open_CharSelectPopup(Action<ENUM_CHARACTER_TYPE> _charCallBack, bool isMine)
    {
        if(charSelectPopup.isUsing)
        {
            Debug.Log("이미 캐릭터선택팝업창이 사용중입니다.");
            return;
        }

        charSelectPopup.Open(_charCallBack, isMine);
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
    public void Open_LoadingPopup()
    {
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
