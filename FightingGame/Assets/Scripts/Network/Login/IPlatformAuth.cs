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
using Google;

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

    private readonly string ClientID = "";

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

    public void SignIn(ENUM_LOGIN_TYPE loginType, string email = "", string password = "", Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        switch (loginType)
        {
            case ENUM_LOGIN_TYPE.Guest:
                SignInByGuest(email, password, OnSignInSuccess, OnSignInFailed, OnSignCanceled);
                break;

            case ENUM_LOGIN_TYPE.Google:
                GoogleAuthenticate(
                    OnGetToken: (Credential c) => { SignInByCredential(c, OnSignInSuccess, OnSignInFailed, OnSignCanceled); },
                    OnSignInSuccess, OnSignInFailed);
                break;

            case ENUM_LOGIN_TYPE.GooglePlayStore:
                GooglePlayStoreAuthenticate(
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

    private void GoogleAuthenticate(Action<Credential> OnGetToken, Action OnSuccess, Action OnFailed = null)
    {
        // 여기 클라이언트 ID의 정체를 아직 모름
        GoogleSignIn.Configuration = new GoogleSignInConfiguration { WebClientId = ClientID, RequestEmail = true, RequestIdToken = true };
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn()
            .ContinueWithOnMainThread((task) => 
            {
                if(task.IsFaulted)
                {
                    OnFailed?.Invoke();
                }
                else if(task.IsCompleted)
                {
                    OnSuccess?.Invoke();

                    var credential = GetUserCredential(false, task.Result.IdToken);
                    OnGetToken?.Invoke(credential);
                }
            });
    }

    private void GooglePlayStoreAuthenticate(Action<Credential> OnGetToken, Action OnSuccess, Action OnFailed = null)
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                OnSuccess?.Invoke();

                // 여기 오토 코드 수정해야 함
                var credential = GetUserCredential(true, "구글 플레이 전용 오토코드");
                OnGetToken?.Invoke(credential);
            }
            else
            {
                OnFailed?.Invoke();
            }
        });
    }

    private Credential GetUserCredential(bool IsUsedPlatformService, string token)
    {
        Credential credential = null;

        if(IsUsedPlatformService)
        {
            string autoCode = token;
            credential = PlayGamesAuthProvider.GetCredential(autoCode);
        }
        else
        {
            string idToken = token;
            string accessToken = GetGoogleAccessToken();
            credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
        }

        return credential;
    }

    private string GetGoogleAccessToken()
    {
        return null; // 무슨 의미가 있는 지 모름, 다들 null로 함zz
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
