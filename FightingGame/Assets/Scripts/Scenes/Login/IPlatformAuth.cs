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
}

public class PlatformAuth : IPlatformAuth
{
    private FirebaseApp app;
    public bool IsAuthValid
    {
        get
        {
            return app != null && auth != null;
        }
    }
    
    private FirebaseAuth auth;
    public static FirebaseUser user;

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
    
    public void SignInWithEmailAndPassword(string email, string password, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
    {
        auth?.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWith(task =>
            {
                if(task.IsCompleted)
                {
                    OnSignInSuccess?.Invoke();
                    Debug.Log("이메일 로그인 성공");
                }
                else if(task.IsFaulted)
                {
                    OnSignInFailed?.Invoke();
                    Debug.LogError($"이메일 로그인 실패 : {task.Result.Email}");
                }
                else if(task.IsCanceled)
                {
                    OnSignCanceled?.Invoke();
                    Debug.LogError($"이메일 로그인 취소 : {task.Result.Email}");
                }
            });
    }

}
