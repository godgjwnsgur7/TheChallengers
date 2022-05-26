using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// (ID 토큰, 플랫폼) - 닉네임 - 승점 - 커피 구매 갯수
/// 너무 적어서 디비를 나눌 필요도 없음, UserInfo로 통일~
/// </summary>

public enum DB_CATEGORY
{
    Nickname,
    VictoryPoint,
    PurchaseCoffee
}

public class DBSession : IDisposable, ISession
{
    private bool disposedValue;

    /// <summary>
    /// 인 게임 도중에 DB에 접근해서 데이터를 가져오는 방식은 아주 느림
    /// 중요한 데이터는 세션에 미리 적재해두는 방식을 사용하는 것을 권장
    /// 
    /// 뭔가 DB를 거쳐서 가지고 와야 한다면, IPlatformDB의 Select 명령어를 써서 콜백을 호출하는 방식을 사용함
    /// </summary>
    /// 

    public bool IsValid
    {
        get
        {
            return DB != null;
        }
    }

    public static IPlatformDB DB
    {
        get;
        private set;
    }

    public static void Update<T>(DB_CATEGORY category, T Data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        // 카테고리가 이건데... T는 왜이래를 검증
        // 카테고리로 hierachy path를 추출
    }

    public static void Select<T>(DB_CATEGORY category, Action<T> pushData = null, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
       // 위에꺼랑 머 마찬가지
    }


    public static void RegisterDB(IPlatformDB db)
    {
        DB = db;
    }

    public static void UnregisterDB()
    {
        DB = null;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                UnregisterDB();
            }

            // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
            // TODO: 큰 필드를 null로 설정합니다.
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
