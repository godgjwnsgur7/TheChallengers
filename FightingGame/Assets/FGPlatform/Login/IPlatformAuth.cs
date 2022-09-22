using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

using Google;
public enum ENUM_LOGIN_TYPE
{
    Guest,// 게스트 계정 (디버깅 모드 포함)
    Google, // 구글 계정
}

namespace FGPlatform.Auth
{
    public interface IPlatformAuth
    {
        public bool IsAuthValid
        {
            get;
        }
        public bool IsAuthPlatform
        {
            get;
        }
        public bool IsLogin
		{
            get;
		}

        public string UserId
        {
            get;
        }

        public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null);
        public void SignIn(ENUM_LOGIN_TYPE loginType, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null, string email = "", string password = "");
        public void SignOut();
    }

    /// <summary>
    /// 파이어베이스 및 구글 인증을 하는 데에 사용되는 도구
    /// </summary>

    public class PlatformAuth : IPlatformAuth
    {
        private FirebaseApp app = null;
        private FirebaseAuth auth = null;

        
        public bool IsAuthValid
        {
            get
            {
                return app != null && auth != null;
            }
        }

        public bool IsAuthPlatform
        {
            get
            {
                return IsAuthValid && googleModule != null;
            }
        }

        public bool IsLogin
        {
            get
			{
                return IsAuthValid && user != null && UserId != string.Empty;
            }
            
        }

        private FirebaseUser user = null;
        public string UserId
        {
            get;
            private set;
        } = string.Empty;

        private GoogleSignIn googleModule = null;
        private readonly string ClientID = "834296008969-ha5c3bfbqjqfh21jo08nggjho53s9tt0.apps.googleusercontent.com";

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
                        InitAuth();
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

        private void InitAuth()
        {
            app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        }

        private void SetFirebaseCurrentUser(FirebaseUser currentUser)
        {
            if (currentUser == null)
                return;

            user = currentUser;
            UserId = currentUser.UserId;
        }

        private void UnsetFirebaseCurrentUser()
		{
            user = null;
            UserId = null;
        }

        public void SignIn(ENUM_LOGIN_TYPE loginType, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null, string email = "", string password = "")
        {
            if (IsLogin) // 이미 로그인을 완료한 경우
			{
                Debug.LogError($"이미 로그인 상태입니다. {UserId}");
                return;
            }
                
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
            }
        }

        public void SignOut()
        {
            if (!IsLogin)
			{
                Debug.LogError("이미 로그아웃 상태입니다.");
                return;
            }

            googleModule?.SignOut();
            auth?.SignOut();

            Debug.LogError($"{UserId} 유저가 로그아웃하였습니다.");
            UnsetFirebaseCurrentUser();
        }

        private void SignInByGuest(string email, string password, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
        {
            // 현재 메인 스레드에서 Debug를 부르는 데에도 정상 작동하지 않는 이슈가 있음
            auth?.SignInWithEmailAndPasswordAsync(email, password)
                .ContinueWithOnMainThread(task =>
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
                        Debug.Log($"이메일 로그인 성공 : {task.Result.Email}");
                        SetFirebaseCurrentUser(task.Result);

                        OnSignInSuccess?.Invoke();
                    }
                });
        }

        private void GoogleAuthenticate(Action<Credential> OnGetToken, Action OnSuccess, Action OnFailed = null)
        {
            if (googleModule == null)
            {
                GoogleSignIn.Configuration = new GoogleSignInConfiguration { WebClientId = ClientID, RequestEmail = true, RequestIdToken = true };
                GoogleSignIn.Configuration.UseGameSignIn = false;
                GoogleSignIn.Configuration.RequestIdToken = true;
                googleModule = GoogleSignIn.DefaultInstance;
            }

            googleModule.SignIn()
                .ContinueWithOnMainThread((task) =>
                {
                    if (task.IsFaulted)
                    {
                        OnFailed?.Invoke();
                    }
                    else if (task.IsCompleted)
                    {
                        OnSuccess?.Invoke();

                        var credential = GetUserCredential(task.Result.IdToken);
                        OnGetToken?.Invoke(credential);
                    }
                });
        }

        private Credential GetUserCredential(string token)
        {
            string idToken = token;
            string accessToken = GetGoogleAccessToken();
            return GoogleAuthProvider.GetCredential(idToken, accessToken);
        }

        private string GetGoogleAccessToken()
        {
            return null; // 무슨 의미가 있는 지 모름, 다들 null로 함... 문제 생길 시 수정
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
                    SetFirebaseCurrentUser(newUser);
                }
            });
        }
    }
}