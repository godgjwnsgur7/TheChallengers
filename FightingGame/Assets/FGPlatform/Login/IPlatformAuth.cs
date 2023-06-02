using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

using Google;
using System.Linq;

public enum ENUM_LOGIN_TYPE
{
    None = -1,
    Guest,// 게스트 계정 (디버깅 모드 포함)
    Google, // 구글 계정
}

namespace FGPlatform.Auth
{
    public class PlatformAuthFactory
    {
        public static IPlatformAuth Create()
        {
#if UNITY_ANDROID && GOOGLE_LOGIN_MODE
            return new PlatformGoogleAuth();
#else
            return new PlatformGuestAuth();
#endif
        }
    }


    public interface IPlatformAuth
    {
        public bool IsAuthValid
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

        public ENUM_LOGIN_TYPE CurrentLoginType
		{
            get;
		}

        public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null);
        public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null);
        public void SignOut();
        public void RegistStateChanged(EventHandler handler);
		public void UnregistStateChanged(EventHandler handler);

	}

    public class PlatformGuestAuth : IPlatformAuth
    {
        public bool IsAuthValid => isAuthValid;
        private bool isAuthValid = false;

        public bool IsLogin => isLogin;
        private bool isLogin = false;

        public string UserId
        {
            get
            {
                string host = System.Net.Dns.GetHostName();
                var entry = System.Net.Dns.GetHostEntry(host);
                var ipAddr = entry.AddressList;
                var address = ipAddr.FirstOrDefault();
                return address.ToString();
            }
        }

        public ENUM_LOGIN_TYPE CurrentLoginType => ENUM_LOGIN_TYPE.Guest;

        public void RegistStateChanged(EventHandler handler)
        {
            
        }

        public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
        {
            isLogin = true;
            OnSignInSuccess?.Invoke();
        }

        public void SignOut()
        {
            isLogin = false;
        }

        public bool TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
        {
            isAuthValid = true;
            OnConnectAuthSuccess?.Invoke();
            return true;
        }

        public void UnregistStateChanged(EventHandler handler)
        {
            
        }
    }

    /// <summary>
    /// 파이어베이스 및 구글 인증을 하는 데에 사용되는 도구
    /// </summary>

    public class PlatformGoogleAuth : IPlatformAuth
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

        public bool IsLogin
        {
            get
			{
                return IsAuthValid && user != null && UserId != string.Empty;
            }
        }

        public ENUM_LOGIN_TYPE CurrentLoginType => ENUM_LOGIN_TYPE.Google;

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

            InitAuth();
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
            UserId = string.Empty;
        }

        public void SignIn(Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
        {
            if (IsLogin) // 이미 로그인을 완료한 경우
			{
                Debug.LogError($"이미 로그인 상태입니다. {UserId}");
                return;
            }

            GoogleAuthenticate(OnGetToken: (Credential c) => { SignInByCredential(c, OnSignInSuccess, OnSignInFailed, OnSignCanceled); },
                         OnSignInSuccess, OnSignInFailed);
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

        public void RegistStateChanged(EventHandler handler)
        {
            if (auth == null)
            {
                Debug.LogError("파이어베이스 인증이 완료되지 않았는데 StateChanged 콜백을 등록합니다.");
                return;
            }

			auth.StateChanged -= handler;
			auth.StateChanged += handler;
        }

        public void UnregistStateChanged(EventHandler handler)
        {
			if (auth == null)
			{
				Debug.LogError("파이어베이스 인증이 완료되지 않았는데 StateChanged 콜백을 등록합니다.");
				return;
			}

			auth.StateChanged -= handler;
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
						Debug.Log($"이메일 로그인 실패 : {task.Result.UserId}");
						OnFailed?.Invoke();
                    }
                    else if (task.IsCompleted)
                    {
                        OnSuccess?.Invoke();

						Debug.Log($"이메일 로그인 성공 : {task.Result.UserId}");

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
			auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
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