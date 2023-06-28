using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class MainCanvas : BaseCanvas
{
    [SerializeField] GuestLoginWindow guestLoginWindow;
    [SerializeField] FirstLoginWindowUI firstLoginWindow;
    [SerializeField] CreditScreen creditScreen;

    [SerializeField] Text blinkText;

    Coroutine textEffectCoroutine = null;
    bool textEffectLock = false;
    bool overlapLock = false;
    
    private void Start()
    {
		Managers.Platform.RegistAuthChanged(null, PhotonLogicHandler.Instance.TryDisconnectToMaster);
		Managers.Platform.Initialize();

        textEffectCoroutine = StartCoroutine(IBlinkEffectToText());
    }

    private void OnEnable()
    {
        overlapLock = false;
    }

    private void OnDisable()
    {
        if (textEffectCoroutine != null)
            StopCoroutine(textEffectCoroutine);
    }

    private void Try_MasterServer()
    {
        Managers.UI.popupCanvas.Open_LoadingPopup();
        PhotonLogicHandler.Instance.TryConnectToMaster(
            GoTo_Lobby, Managers.Network.DisconnectMaster_CallBack);
    }

    private void GoTo_Lobby()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_JoinGameLobby);
        Managers.UI.popupCanvas.Close_LoadingPopup();
        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
    }

    private void Login_Failed()
    {
        overlapLock = false;
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Notify);
        Managers.UI.popupCanvas.Open_NotifyPopup("로그인에 실패했습니다.\n다시 시도해주세요");
        Managers.UI.popupCanvas.Close_LoadingPopup();
    }

    public void Set_MyNickname(string myNickName)
    {
        PhotonLogicHandler.CurrentMyNickname = myNickName;
        Try_MasterServer();
    }

    public void OnClick_LoginAndMasterServer()
    {
        if (overlapLock)
            return;

        overlapLock = true;

        Managers.UI.popupCanvas.Open_LoadingPopup();
        Managers.Platform.Login(
        _OnSignInSuccess: () =>
        {
            string id = Managers.Platform.GetUserID();
            Debug.Log($"회원번호 : {id} 으로 로그인 완료");
        },
        _OnCheckFirstUser: (bool isFirstLogin) =>
        {
            if (isFirstLogin)
            {
                firstLoginWindow.Open(Set_MyNickname);
            }
            else
            {
                Try_MasterServer();
            }
        }, _OnSignInFailed: () => { Login_Failed(); });
    }

    public void OnClick_Credit()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Light);
        creditScreen.Open();
    }

    protected IEnumerator IBlinkEffectToText()
    {
        textEffectLock = true;
        Color color = new Color(1, 1, 1, 1);
        float runTIme;
        float duration = 1.0f;

        while (textEffectLock)
        {
            runTIme = 0.0f;
            color.a = 1.0f;
            blinkText.color = color;
            while (runTIme < duration)
            {
                runTIme += Time.deltaTime;
                color.a = Mathf.Lerp(1.0f, 0.5f, runTIme / duration);
                blinkText.color = color;
                yield return null;
            }

            runTIme = 0.0f;
            color.a = 0.5f;
            blinkText.color = color;
            while (runTIme < duration)
            {
                runTIme += Time.deltaTime;
                color.a = Mathf.Lerp(0.5f, 1.0f, runTIme / duration);
                blinkText.color = color;
                yield return null;
            }
        }

        textEffectLock = false;
        textEffectCoroutine = null;
    }
}
