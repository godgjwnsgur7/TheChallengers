using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using System.Text;

/// <summary>
/// (ID 토큰, 플랫폼) - 닉네임 - 승점 - 커피 구매 갯수
/// 너무 적어서 디비를 나눌 필요도 없음
/// 
/// 소리쿤.0.0.0.0 같은 느낌으로 저장한다.
/// </summary>

public enum DB_CATEGORY
{
    Nickname,
    VictoryPoint,
    DefeatPoint,
    RatingPoint,
    PurchaseCoffee
}

namespace FGPlatform.Datebase
{
    /// <summary>
    /// Insert나 Delete는 클라에서 해줄 요청이 아니긴 한데...
    /// </summary>

    public interface IPlatformDB
    {
        public void InitDataBase();
        bool UpdateDB<T>(DB_CATEGORY category, string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);

        bool InsertDB(string[] hierachyPath, string nickname, Action<DBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);

        // 가져올 땐 한 방에
        bool SelectDB(string[] hierachyPath, bool isMine, Action<DBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);
    }


    public delegate void DataSnapAction(DataSnapshot shot);

    /// <summary>
    /// 파이어베이스의 DB는 Json 형식으로 데이터를 저장함
    /// </summary>

    public class PlatformDB : IPlatformDB
    {
        private readonly string URL = "https://projectfg-c8bc3-default-rtdb.firebaseio.com";

        private FirebaseApp app;
        private FirebaseDatabase database;

        private DatabaseReference dbRootReference;

        private DBUserData myUserData = null;

        public void InitDataBase()
        {
            app = FirebaseApp.DefaultInstance;
            app.Options.DatabaseUrl = new Uri(URL);

            database = FirebaseDatabase.DefaultInstance;
            dbRootReference = database.RootReference;
        }

        public bool UpdateDB<T>(DB_CATEGORY category, string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
        {
            if (!typeof(T).IsSerializable)
            {
                Debug.LogError("넣을 데이터가 Serializable한 형식이 아닙니다.");
                return false;
            }

            DatabaseReference reference = dbRootReference;

            foreach (string path in hierachyPath)
            {
                reference = reference.Child(path);
            }

            string jsonData = string.Empty;

            if (myUserData != null)
            {
                SetDBData(category, data, ref myUserData);
                jsonData = ParseUserData(myUserData);
            }
            else
            {
                SelectDB(hierachyPath, true, (userData) =>
                {
                    UpdateDB(category, hierachyPath, data, OnSuccess, OnFailed, OnCanceled);
                });

                Debug.LogError("Update 메소드 최초 실행 때 시간이 오래 걸릴 수 있습니다. \n 로그인 후에 미리 한번 수행해주세요.");
                return false;
            }

            reference.SetValueAsync(jsonData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    OnFailed?.Invoke();
                }
                else if (task.IsCanceled)
                {
                    OnCanceled?.Invoke();
                }
                else if (task.IsCompleted)
                {
                    OnSuccess?.Invoke(data);
                }
            });

            return true;
        }

        /// <summary>
        /// 최초 자신에 대하여 1회 실행 권장함
        /// 실행하고, null이라면 Insert로 초기값을 넣어주어야 함
        /// </summary>

        public bool SelectDB(string[] hierachyPath, bool isMine, Action<DBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
        {
            if (!typeof(DBUserData).IsSerializable)
            {
                Debug.LogError("넣을 데이터가 Serializable한 형식이 아닙니다.");
                return false;
            }

            DatabaseReference reference = dbRootReference;

            foreach (string path in hierachyPath)
            {
                reference = reference.Child(path);
            }

            reference.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    OnFailed?.Invoke();
                }
                else if (task.IsCanceled)
                {
                    OnCanceled?.Invoke();
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    string data = (string)(snapshot.Value);
                    var userData = ParseStringData(data);

                    if (isMine)
                        myUserData = userData;

                    OnSuccess?.Invoke(userData);
                }
            });

            return true;
        }

        public bool InsertDB(string[] hierachyPath, string nickname, Action<DBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
        {
            DatabaseReference reference = dbRootReference;

            foreach (string path in hierachyPath)
            {
                reference = reference.Child(path);
            }

            myUserData = new DBUserData(nickname, 0L, 0L, 0L, 0L);
            string jsonData = ParseUserData(myUserData);

            reference.SetValueAsync(jsonData).ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    OnFailed?.Invoke();
                }
                else if (task.IsCanceled)
                {
                    OnCanceled?.Invoke();
                }
                else if (task.IsCompleted)
                {
                    OnSuccess?.Invoke(myUserData);
                }
            });

            return true;
        }

        private DBUserData ParseStringData(string data)
		{
            if(data == null)
            {
                return null;
            }

            string[] splitData = data.Split('.');

            if (splitData.Length <= (int)DB_CATEGORY.PurchaseCoffee)
                return null;

            string nickname = splitData[(int)DB_CATEGORY.Nickname];
            Int64.TryParse(splitData[(int)DB_CATEGORY.VictoryPoint], out long VictoryPoint);
            Int64.TryParse(splitData[(int)DB_CATEGORY.DefeatPoint], out long DefeatPoint);
            Int64.TryParse(splitData[(int)DB_CATEGORY.RatingPoint], out long RatingPoint);
            Int64.TryParse(splitData[(int)DB_CATEGORY.PurchaseCoffee], out long PurchaseCoffee);

            var outData = new DBUserData(nickname, VictoryPoint, DefeatPoint, RatingPoint, PurchaseCoffee);
            return outData;
        }

		private string ParseUserData(DBUserData data)
		{
            if (data == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(data.nickname);
            sb.Append('.');
            sb.Append(data.victoryPoint);
            sb.Append('.');
            sb.Append(data.defeatPoint);
            sb.Append('.');
            sb.Append(data.ratingPoint);
            sb.Append('.');
            sb.Append(data.purchaseCoffeeCount);

            return sb.ToString();
        }

        private void SetDBData(DB_CATEGORY category, object value, ref DBUserData data)
		{
            if(category == DB_CATEGORY.Nickname)
			{
				data.nickname = (string)value;
            }
            else if(category == DB_CATEGORY.VictoryPoint)
            {
                data.victoryPoint = (long)value;
            }
            else if(category == DB_CATEGORY.DefeatPoint)
			{
                data.defeatPoint = (long)value;
            }
            else if(category == DB_CATEGORY.RatingPoint)
			{
                data.ratingPoint = (long)value;
            }
            else if(category == DB_CATEGORY.PurchaseCoffee)
			{
                data.purchaseCoffeeCount = (long)value;
            }
		}
	}

}

