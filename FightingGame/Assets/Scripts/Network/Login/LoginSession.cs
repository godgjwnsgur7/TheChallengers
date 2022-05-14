using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 로그인 정보가 남아있는 세션, 한 번 인증한 후엔 더 이상 인증을 할 수 없도록 세션만 남기는 것을 추천
/// </summary>
public class LoginSession
{
    public static bool IsSessionValid
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

    ~LoginSession()
    {
        UnregisterAuth();
    }
}
