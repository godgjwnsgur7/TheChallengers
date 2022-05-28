using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISession
{
    bool IsValid
    {
        get;
    }
}

/// <summary>
/// 로그인 정보가 남아있는 세션, 한 번 인증한 후엔 더 이상 인증을 할 수 없도록 세션만 남기는 것을 추천
/// </summary>
public class LoginSession : IDisposable, ISession
{
    private bool disposedValue;

    public bool IsValid
    {
        get
        {
            if(authInfo != null)
                return authInfo.IsAuthValid;

            return false;
        }
    }

    public static IPlatformAuth authInfo
    {
        get;
        private set;
    }

    public static void RegisterAuth(IPlatformAuth iAuth)
    {
        authInfo = iAuth;
    }

    public static void UnregisterAuth()
    {
        authInfo = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                UnregisterAuth();
            }

            // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
            // TODO: 큰 필드를 null로 설정합니다.
            disposedValue = true;
        }
    }

    // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
    // ~LoginSession()
    // {
    //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
