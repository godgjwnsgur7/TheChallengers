using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.OurUtils;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;


public enum ENUM_LOGIN_TYPE
{
    Guest,// 게스트 계정 (디버깅 모드 포함)
    Google, // 구글 계정
    GooglePlayStore // 구글 플레이 스토어 계정 (플랫폼 서비스 이용)
}


public interface IPlatformAuth
{
    public bool IsAuthValid
    {
        get;
    }

    public string UserId
    {
        get;
    }
}

/// <summary>
/// 파이어베이스 인증을 하는 데에 사용되는 도구
/// </summary>

public class PlatformAuth : IPlatformAuth
{
    private FirebaseApp app;
    private FirebaseAuth auth;
    public bool IsAuthValid
    {
        get
        {
            return app != null && auth != null;
        }
    }

    private FirebaseUser user;
    public string UserId
    {
        get;
        private set;
    } = string.Empty;

    public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
    {
        if (IsAuthValid) // 이미 파이어베이스 인증을 끝낸 경우임
            return false;

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                var result = task.Result;

                if (result == DependencyStatus.Available)
                {
                    InitFirebase();
                    OnConnectAuthSuccess?.Invoke();
                    Debug.Log("파이어베이스 인증 성공");
                }
                else // 호출 시도 아직 안 해봄
                {
                    OnConnectAuthFail?.Invoke();
                    Debug.LogError("파이어베이스 인증 실패");
                }
            });

        
        return true;
    }

    private void InitFirebase()
    {
        app = FirebaseApp.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
    }

    private void InitFirebaseCurrentUser(FirebaseUser currentUser)
    {
        if (currentUser == null)
            return;

        user = currentUser;
        UserId = currentUser.UserId;

        LoginSession.RegisterAuth(this);
    }

    public void SignIn(ENUM_LOGIN_TYPE loginType, string email, string password, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        switch (loginType)
        {
            case ENUM_LOGIN_TYPE.Guest:
                SignInByGuest(email, password, OnSignInSuccess, OnSignInFailed, OnSignCanceled);
                break;

            case ENUM_LOGIN_TYPE.Google:
            case ENUM_LOGIN_TYPE.GooglePlayStore:
                SocialAuthenticate(
                    IsUsedPlatformService: loginType == ENUM_LOGIN_TYPE.GooglePlayStore,
                    OnGetToken: (Credential c) => { SignInByCredential(c, OnSignInSuccess, OnSignInFailed, OnSignCanceled); },
                    OnSignInSuccess, OnSignInFailed);
                break;
        }
    }

    public void SignOut()
    {
        auth?.SignOut();
    }
    
    private void SignInByGuest(string email, string password, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        // 현재 메인 스레드에서 Debug를 부르는 데에도 정상 작동하지 않는 이슈가 있음
        auth?.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsFaulted)
                {
                    OnSignInFailed?.Invoke();
                    Debug.LogError($"이메일 로그인 실패 : {task.Result.Email}");
                }
                else if(task.IsCanceled)
                {
                    OnSignCanceled?.Invoke();
                    Debug.LogWarning($"이메일 로그인 취소 : {task.Result.Email}");
                }
                else
                {
                    OnSignInSuccess?.Invoke();
                    Debug.Log($"이메일 로그인 성공 : {task.Result.Email}");
                    InitFirebaseCurrentUser(task.Result);
                }
            });
    }

    private void SocialAuthenticate(bool IsUsedPlatformService, Action<Credential> OnGetToken, Action OnSuccess, Action OnFailed = null)
    {
        var credential = GetUserCredential(IsUsedPlatformService);

        if(!IsUsedPlatformService)
        {
            OnGetToken?.Invoke(credential);
            return;
        }

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                OnSuccess?.Invoke();
                OnGetToken?.Invoke(credential);
            }
            else
            {
                OnFailed?.Invoke();
            }
        });
    }

    private Credential GetUserCredential(bool IsUsedPlatformService)
    {
        Credential credential = null;

        if(IsUsedPlatformService)
        {
            string autoCode = GetAutoCodeToken();
            credential = PlayGamesAuthProvider.GetCredential(autoCode);
        }
        else
        {
            string idToken = GetGoogleIDToken();
            string accessToken = GetGoogleAccessToken();
            credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        }

        return credential;
    }

    /// <summary>
    /// 현재 ID Token을 얻는 곳에 문제가 있어서 임시 방편, 이 곳을 수정하는 것이 일
    /// 구글은 자바 쪽 코드를 읽어야 하는 건가..? https://firebase.google.com/docs/auth/unity/google-signin?hl=ko
    /// 플레이 스토어 쪽 소셜 코드는 구글 플레이 콘솔과 앱이 연동이 되어 있어야 작동하므로, 추후 수정 https://firebase.google.com/docs/auth/unity/play-games?hl=ko
    /// </summary>
    /// <returns></returns>

    private string GetGoogleIDToken()
    {
        return "googleIdToken";
    }

    private string GetAutoCodeToken()
    {
        return "authCode";
    }

    private string GetGoogleAccessToken()
    {
        return null;
    }

    private void SignInByCredential(Credential credential, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => 
        {
            if (task.IsFaulted)
            {
                OnSignInFailed?.Invoke();
                Debug.LogError($"이메일 로그인 실패 : {task.Result.Email}");
            }
            else if (task.IsCanceled)
            {
                OnSignCanceled?.Invoke();
                Debug.LogWarning($"이메일 로그인 취소 : {task.Result.Email}");
            }
            else
            {
                OnSignInSuccess?.Invoke();
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("이메일 로그인 성공 : {0} ({1})", newUser.DisplayName, newUser.UserId);
                InitFirebaseCurrentUser(newUser);
            }
        });
    }
}
