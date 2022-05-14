using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
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
    
    public void TryConnectAuth(Action OnConnectAuthSuccess = null, Action OnConnectAuthFail = null)
    {
        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWith(task =>
            {
                var result = task.Result;

                if(result == DependencyStatus.Available)
                {
                    InitFirebase();
                    OnConnectAuthSuccess?.Invoke();
                    Debug.Log("파이어베이스 인증 성공");
                }
                else
                {
                    OnConnectAuthFail?.Invoke();
                    Debug.LogError("파이어베이스 인증 실패");
                }
            });
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
        auth?.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWith(task =>
            {
                if(task.IsCompleted)
                {
                    OnSignInSuccess?.Invoke();
                    Debug.Log($"이메일 로그인 성공 : {task.Result.Email}");

                    InitFirebaseCurrentUser(task.Result);
                }
                else if(task.IsFaulted)
                {
                    OnSignInFailed?.Invoke();
                    Debug.LogError($"이메일 로그인 실패 : {task.Result.Email}");
                }
                else if(task.IsCanceled)
                {
                    OnSignCanceled?.Invoke();
                    Debug.LogWarning($"이메일 로그인 취소 : {task.Result.Email}");
                }
            });
    }

}
