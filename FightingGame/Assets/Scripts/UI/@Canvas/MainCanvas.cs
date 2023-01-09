using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class MainCanvas : BaseCanvas
{
    [SerializeField] GuestLoginWindow guestLoginWindow;
    [SerializeField] FirstLoginWindowUI firstLoginWindow;
    [SerializeField] Button mainButton;

    [SerializeField] Text loginText;
    [SerializeField] Text gameStartText;

    Coroutine textEffectCoroutine = null;

    float fadeEffectSpeed = 0.5f;
    bool textEffectLock = false;
    bool overlapLock = false;

    private void Start()
    {
        Managers.Platform.Initialize();
        Set_LoginEnvironment();
    }

    public void Set_OverlabLock(bool _value) => overlapLock = _value;

    private void Set_LoginEnvironment()
    {
        textEffectCoroutine = StartCoroutine(ITextEffect_FadeOut(loginText));
        mainButton.onClick.AddListener(OnClick_Login);
    }

    private void Set_GameStartEnvironment()
    {
        // Remove Login
        textEffectLock = true;
        StopCoroutine(textEffectCoroutine);
        loginText.gameObject.SetActive(false);
        mainButton.onClick.RemoveListener(OnClick_Login);

        // Add GameStart
        textEffectLock = false;
        gameStartText.gameObject.SetActive(true);
        mainButton.onClick.AddListener(OnClick_Start);
        textEffectCoroutine = StartCoroutine(ITextEffect_FadeOut(gameStartText));
    }

    public void Try_GuestLogin()
    {
        if (!guestLoginWindow.Check_InputField())
            return;

        Managers.Platform.Login(ENUM_LOGIN_TYPE.Guest, () =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");

            if (string.IsNullOrEmpty(PhotonLogicHandler.CurrentMyNickname))
                firstLoginWindow.Open(Set_NickNameCallBack);
            else
                Set_GameStartEnvironment();

            OnClick_Deactivate(guestLoginWindow.gameObject);
        },
        _OnCheckFirstUser: (bool isFirstLogin) =>
        {
            if (isFirstLogin)
            {
                firstLoginWindow.Open(Set_NickNameCallBack);
            }
        }, email: guestLoginWindow.Get_EmailText(), password: guestLoginWindow.Get_PasswordText());
    }

    public void Try_GuestLoginA()
    {
        Managers.Platform.Login(ENUM_LOGIN_TYPE.Guest, () =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");

            PhotonLogicHandler.CurrentMyNickname = "godgjwnsgur";

            Set_GameStartEnvironment();
            OnClick_Deactivate(guestLoginWindow.gameObject);

        }, null, null, email: "godgjwnsgur7@gmail.com", password: "123456");
    }

    public void Try_GuestLoginB()
    {
        Managers.Platform.Login(ENUM_LOGIN_TYPE.Guest, () =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");

            PhotonLogicHandler.CurrentMyNickname = "sorikun";

            Set_GameStartEnvironment();
            OnClick_Deactivate(guestLoginWindow.gameObject);

        }, null, null, email: "psh50zmfhtm@gmail.com", password: "123456");
    }

    public void Set_NickNameCallBack(string nickname)
    {
        PhotonLogicHandler.CurrentMyNickname = nickname;
        Set_GameStartEnvironment();

        firstLoginWindow.Close();
    }

    private void Try_GoogleLogin()
    {
        string loginID;

        Managers.Platform.Login(ENUM_LOGIN_TYPE.Google,
        _OnSignInSuccess: () =>
        {
            loginID = Managers.Platform.GetUserID();

            if (loginID == string.Empty)
            {
                Managers.UI.popupCanvas.Close_LoadingPopup();
                Managers.UI.popupCanvas.Open_NotifyPopup("로그인에 실패했습니다. 다시 시도해주세요.");
                return;
            }

            Debug.Log($"회원번호 : {loginID} 으로 로그인 완료");
            Set_GameStartEnvironment();
        },
        _OnCheckFirstUser: (bool isFirstLogin) =>
        {
            if (isFirstLogin)
            {
                firstLoginWindow.Open(Set_NickNameCallBack);
            }
        });   
    }

    private void Try_ConnectMasterServerAndStart()
    {
        bool isSuccess = PhotonLogicHandler.Instance.TryConnectToMaster(
            () => Try_JoinLobby(), null);

        if(!isSuccess)
        {
            Managers.UI.popupCanvas.Close_LoadingPopup();
            Managers.UI.popupCanvas.Open_NotifyPopup("서버 접속에 실패했습니다. 다시 시도해주세요.");
        }
            
    }

    private void Try_JoinLobby()
    {
        bool isSuccess = PhotonLogicHandler.Instance.TryJoinLobby(ENUM_MATCH_TYPE.RANDOM, GoTo_LobbyScene);

        if (!isSuccess)
        {
            Managers.UI.popupCanvas.Close_LoadingPopup();
            Managers.UI.popupCanvas.Open_NotifyPopup("서버 접속에 실패했습니다. 다시 시도해주세요.");
        }
    }

    private void GoTo_LobbyScene()
    {
        Managers.UI.popupCanvas.Close_LoadingPopup();
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
    }

    public void OnClick_Login()
    {
        if (overlapLock)
            return;

        overlapLock = true;

        //Try_GoogleLogin();

        OnClick_Activate(guestLoginWindow.gameObject);
    }

    public void OnClick_Start()
    {
        Managers.UI.popupCanvas.Open_LoadingPopup();

        Try_ConnectMasterServerAndStart();
    }

    protected IEnumerator ITextEffect_FadeOut(Text effectTarget)
    {
        effectTarget.color = new Color(1f, 1f, 1f, 1f);

        Color tempColor = effectTarget.color;

        while (tempColor.a > 0.5f && !textEffectLock)
        {
            tempColor.a -= Time.deltaTime * fadeEffectSpeed;
            effectTarget.color = tempColor;

            yield return null;
        }

        textEffectCoroutine = null;
        if(!textEffectLock)
            textEffectCoroutine = StartCoroutine(ITextEffect_FadeIn(effectTarget));
    }

    protected IEnumerator ITextEffect_FadeIn(Text effectTarget)
    {
        effectTarget.color = new Color(1f, 1f, 1f, 0.5f);

        Color tempColor = effectTarget.color;

        while (tempColor.a < 0.95f && !textEffectLock)
        {
            tempColor.a += Time.deltaTime * fadeEffectSpeed;
            effectTarget.color = tempColor;

            yield return null;
        }

        textEffectCoroutine = null;
        if (!textEffectLock)
            textEffectCoroutine = StartCoroutine(ITextEffect_FadeOut(effectTarget));
    }
}
