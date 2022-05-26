using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;

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

    public FirebaseUser user;
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

                if(result == DependencyStatus.Available)
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
    
    public void SignInWithEmailAndPassword(string email, string password, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
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

    // 구글, 페이스북 추가
    // 토큰과 플랫폼 키를 해싱하여 디비에 추가
}
