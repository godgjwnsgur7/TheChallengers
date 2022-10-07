using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGPlatform.Auth;
using FGPlatform.Datebase;
using FGPlatform.Advertisement;
using FGPlatform.Purchase;

[Serializable]
public class DBUserData
{
	public string nickname = string.Empty;
	public long victoryPoint = 0L;
	public long defeatPoint = 0L;
	public long ratingPoint = 0L;
	public long purchaseCoffeeCount = 0L;

	public DBUserData(string nickname, long victoryPoint, long defeatPoint, long ratingPoint, long purchaseCoffeeCount)
	{
		this.nickname = nickname;
		this.victoryPoint = victoryPoint;
		this.defeatPoint = defeatPoint;
		this.ratingPoint = ratingPoint;
		this.purchaseCoffeeCount = purchaseCoffeeCount;
	}
}

namespace FGPlatform
{
	public class PlatformMgr
	{
		private IPlatformAuth Auth = new PlatformAuth();
		private IPlatformDB DB = new PlatformDB();
		private IAdMobController AdMob = new AdMobController();
		private CoffeeMachine IAPController = new IAPController();

		private static readonly Dictionary<DB_CATEGORY, Type> validCategoryTypeDictionary = new Dictionary<DB_CATEGORY, Type>()
	{
		{ DB_CATEGORY.Nickname, typeof(string) },
		{ DB_CATEGORY.VictoryPoint, typeof(long) },
		{ DB_CATEGORY.DefeatPoint, typeof(long) },
		{ DB_CATEGORY.RatingPoint, typeof(long) },
		{ DB_CATEGORY.PurchaseCoffee, typeof(long) },
	};

		public void Initialize()
		{
			Auth.TryConnectAuth();
			DB.InitDataBase();
			IAPController.Init();
			AdMob.Init(BannerPosition.Top);
		}

		public string GetUserID()
		{
			if(!Auth.IsAuthValid)
			{
				Debug.LogError("아직 로그인이 되지 않은 상태입니다.");
				return string.Empty;
			}

			return Auth.UserId;
		}

		public ENUM_LOGIN_TYPE CurrentLoginType
		{
			get
			{
				return Auth.CurrentLoginType;
			}
		}

		public void Login(ENUM_LOGIN_TYPE loginType, Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null, string email = "", string password = "")
		{
			Auth.SignIn(loginType, OnSignInSuccess, OnSignInFailed, OnSignCanceled, email, password);
		}

		public void Logout()
		{
			Auth.SignOut();
		}

		public bool DBUpdate<T>(DB_CATEGORY category, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
		{
			if (!CheckCategoryDataType(category, typeof(T)))
				return false;

			var loginType = Auth.CurrentLoginType;
			var userId = GetUserID();

			if (loginType == ENUM_LOGIN_TYPE.None || userId.Equals(string.Empty))
			{
				Debug.LogError("로그인 상태가 아닙니다.");
				return false;
			}

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			return DB.UpdateDB<T>(category, hierachyPath, data, OnSuccess, OnFailed, OnCanceled);
		}

		public bool DBSelect(ENUM_LOGIN_TYPE loginType, string userId, Action<DBUserData> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
		{
			bool isMine = userId == GetUserID() && loginType == CurrentLoginType;
			if(!isMine)
			{
				Debug.LogError("다른 유저의 데이터베이스에 접근합니다. 주의해주세요.");
			}

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			return DB.SelectDB(hierachyPath, isMine, OnSuccess, OnFailed, OnCanceled);
		}

		/// <summary>
		/// 유저 로그인 시 1회 호출합니다.
		/// 데이터가 있는 지 검사하고, 데이터가 없다면 새로운 데이터로 초기화합니다.
		/// </summary>

		public void InitializeCurrentUserDB(string nickname, Action<DBUserData> OnCompleted = null)
		{
			var loginType = Auth.CurrentLoginType;
			var userId = GetUserID();

			if (loginType == ENUM_LOGIN_TYPE.None || userId.Equals(string.Empty))
			{
				Debug.LogError("로그인 상태가 아닙니다.");
				return;
			}

			OnCompleted += (data) => { Debug.Log($"{loginType} - {userId} 데이터 초기화 완료"); };

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			DB.SelectDB(hierachyPath, true, (data) =>
			{
				if(data == null)
				{
					DB.InsertDB(hierachyPath, nickname, OnCompleted);
				}
			});
		}

		public void ShowBanner()
		{
			AdMob.ShowBanner();
		}

		public void HideBanner()
		{
			AdMob.HideBanner();
		}

		private string GetHashToken(ENUM_LOGIN_TYPE type, string id)
		{
			return type.ToString() + id;
		}

		private bool CheckCategoryDataType(DB_CATEGORY category, Type parameterType)
		{
			if (validCategoryTypeDictionary.TryGetValue(category, out var type))
			{
				if (type == parameterType)
				{
					return true;
				}
				else
				{
					Debug.LogError($"넘긴 파라미터 타입 : {parameterType} / 올바른 타입 : {type}");
					return false;
				}
			}

			Debug.LogError($"{category} 미등록");
			return false;
		}
	}

}
