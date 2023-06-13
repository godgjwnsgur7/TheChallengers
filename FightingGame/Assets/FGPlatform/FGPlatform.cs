using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGPlatform.Auth;
using FGPlatform.Datebase;
using FGPlatform.Advertisement;
using FGPlatform.Purchase;
using Firebase;
using Firebase.Extensions;

[Serializable]
public class DBUserData
{
	public string nickname = string.Empty;
	public long victoryPoint = 0L;
	public long defeatPoint = 0L;
	public long ratingPoint = 1500L;
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
		private IPlatformAuth Auth
		{
			get
			{
				if (auth == null)
				{
					auth = PlatformAuthFactory.Create();
                }

				return auth;
			}
		}
		private IPlatformAuth auth = null;

		private IPlatformDB DB = new PlatformDB();
		private IPlatformCrashlytics Crashlytics = new PlatformCrashlytics();	
		private IAdMobController AdMob = new AdMobController();
		private CoffeeMachine IAPController = new IAPController();

		private EventHandler onAuthChangedHandler = null;

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
#if UNITY_ANDROID
			IAPController.Init();
			AdMob.Init(BannerPosition.Bottom);
#endif

            FirebaseApp.CheckAndFixDependenciesAsync()
			   .ContinueWithOnMainThread(task =>
			   {
				   if (task.Result == DependencyStatus.Available)
				   {
					   Auth.TryConnectAuth();
					   DB.InitDataBase();
					   Crashlytics.Init();

					   if (onAuthChangedHandler != null)
					   {
						   Auth.UnregistStateChanged(onAuthChangedHandler);
						   Auth.RegistStateChanged(onAuthChangedHandler);
					   }

					   Debug.Log("파이어베이스 인증 성공");
				   }
				   else // 호출 시도 아직 안 해봄
				   {
					   Debug.LogError("파이어베이스 인증 실패");
				   }
			   });
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

		public void RegistAuthChanged(Action onLogin, Action onLogout)
		{
			onAuthChangedHandler = new EventHandler((sender, args) =>
			{
				Debug.LogWarning($"{sender.ToString()}로부터 {args.ToString()} 인증 상황 변화가 전달되었습니다.");

				if(Auth.IsLogin)
				{
					onLogin?.Invoke();
				}
				else
				{
					onLogout?.Invoke();	
				}
			});

			if(Auth.IsAuthValid)
			{
				Auth.UnregistStateChanged(onAuthChangedHandler);
				Auth.RegistStateChanged(onAuthChangedHandler);
			}
		}

		public void Login(Action _OnSignInSuccess = null, Action _OnSignInFailed = null, Action _OnSignCanceled = null, Action<bool> _OnCheckFirstUser = null)
		{
			Auth.SignIn(OnSignInSuccess: () => // 로그인 성공 시 
			{
				_OnCheckFirstUser += (bool b) => // 체크 및 Initialize 먼저 하고, 
				{
					_OnSignInSuccess?.Invoke(); // 성공 콜백을 호출해야 서순이 맞음
				};

				CheckFirstUserOrInitialize(_OnCheckFirstUser);
			},
			_OnSignInFailed, _OnSignCanceled);
		}

		public void Logout()
		{
			if(Auth.IsLogin)
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
				Debug.LogWarning("다른 유저의 데이터베이스에 접근합니다. 주의해주세요.");
			}

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			return DB.SelectDB(hierachyPath, isMine, OnSuccess, OnFailed, OnCanceled);
		}

		private void CheckFirstUserOrInitialize(Action<bool> checkRoutine)
		{
			InitializeCurrentUserDB(OnCompleted: (data) =>
			{
				checkRoutine?.Invoke(data == null);
			});
		}

		private void InitializeCurrentUserDB(Action<DBUserData> OnCompleted = null)
		{
			var loginType = Auth.CurrentLoginType;
			var userId = GetUserID();

			if (Auth.IsLogin == false)
			{
				Debug.LogError("로그인 상태가 아닙니다.");
				return;
			}

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			DB.SelectDB(hierachyPath, true, (data) =>
			{
				if (data == null)
				{
					DB.InsertDB(hierachyPath, userId);
				}

				OnCompleted?.Invoke(data);
			});
		}

		public void ShowBanner()
		{
			AdMob.ShowAd(AdvertisementType.Interstitial);
		}

		public void HideBanner()
		{
			AdMob.HideAd(AdvertisementType.Interstitial);
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
