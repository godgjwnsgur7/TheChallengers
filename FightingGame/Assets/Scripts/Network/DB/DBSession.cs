using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

    private static IPlatformDB DB;

    // 현재 디비 카테고리에 해당하는 + 저장 가능한 데이터 타입을 걸러주긴 하는데... 사용이 불편하다면 스스로 Select, UpdateNickname() 등의 편의 함수를 만들어 사용하는 것을 추천함
    // 리턴하는 값은 대체적인 성공/실패 여부임, 어지간하면 맞겠지만 보장할 수는 없음 > 대응이 필요하다면 연락
    public static bool Update<T>(DB_CATEGORY category, ENUM_LOGIN_TYPE loginType, string userId, T data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if (!CheckCategoryDataType(category, typeof(T)))
            return false;

        string token = GetHashToken(loginType, userId);
        string[] hierachyPath = new string[] { token, category.ToString() };

        return DB.UpdateDB<T>(hierachyPath, data, OnSuccess, OnFailed, OnCanceled);
    }

    public static bool Select<T>(DB_CATEGORY category, ENUM_LOGIN_TYPE loginType, string userId, Action<T> pushData = null, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if (!CheckCategoryDataType(category, typeof(T)))
            return false;

        string token = GetHashToken(loginType, userId);
        string[] hierachyPath = new string[] { token, category.ToString() };

        return DB.SelectDB<T>(hierachyPath, pushData, OnSuccess, OnFailed, OnCanceled);
    }

    private static string GetHashToken(ENUM_LOGIN_TYPE type, string id)
    {
        string hashToken = string.Empty;

        string typeStr = type.ToString();

        using(MD5 md5 = MD5.Create())
        {
            var loginData = Encoding.UTF8.GetBytes(id);
            var typeData = Encoding.UTF8.GetBytes(typeStr);

            md5.TransformBlock(loginData, 0, loginData.Length, loginData, 0);
            md5.TransformBlock(typeData, 0, typeData.Length, typeData, 0);
            md5.TransformFinalBlock(new byte[0], 0, 0);

            hashToken = Encoding.Default.GetString(md5.Hash);
        }

        return hashToken;
    }

    private static bool CheckCategoryDataType(DB_CATEGORY category, Type type)
    {
        if(category == DB_CATEGORY.Nickname)
        {
            Debug.LogError($"데이터 타입 에러, {category}에는 string 타입이 매핑되어야 합니다.");
            return type == typeof(string);
        }
        else if(category == DB_CATEGORY.PurchaseCoffee)
        {
            Debug.LogError($"데이터 타입 에러, {category}에는 long 타입이 매핑되어야 합니다.");
            return type == typeof(long);
        }
        else if(category == DB_CATEGORY.VictoryPoint)
        {
            Debug.LogError($"데이터 타입 에러, {category}에는 int 타입이 매핑되어야 합니다.");
            return type == typeof(int);
        }

        return false;
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
