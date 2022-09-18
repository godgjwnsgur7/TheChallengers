using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGPlatform.Auth;
using FGPlatform.Datebase;
using FGPlatform.Advertisement;
using FGPlatform.Purchase;

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
		}

		public void Login(ENUM_LOGIN_TYPE loginType, string email = "", string password = "", Action OnSignInSuccess = null, Action OnSignInFailed = null, Action OnSignCanceled = null)
		{
			Auth.SignIn(loginType, email, password, OnSignInSuccess, OnSignInFailed, OnSignCanceled);
		}

		public void SignOut()
		{
			Auth.SignOut();
		}

		public bool DBUpdate<T>(DB_CATEGORY category, ENUM_LOGIN_TYPE loginType, string userId, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
		{
			if (!CheckCategoryDataType(category, typeof(T)))
				return false;

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			return DB.UpdateDB<T>(hierachyPath, data, OnSuccess, OnFailed, OnCanceled);
		}

		public bool DBSelect<T>(DB_CATEGORY category, ENUM_LOGIN_TYPE loginType, string userId, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
		{
			if (!CheckCategoryDataType(category, typeof(T)))
				return false;

			string token = GetHashToken(loginType, userId);
			string[] hierachyPath = new string[] { token };

			return DB.SelectDB<T>(hierachyPath, OnSuccess, OnFailed, OnCanceled);
		}

		private string GetHashToken(ENUM_LOGIN_TYPE type, string id)
		{
			string hashToken = type.ToString() + id;

			// string typeStr = type.ToString();

			//using(MD5 md5 = MD5.Create())
			//{
			//    var loginData = Encoding.UTF8.GetBytes(id);
			//    var typeData = Encoding.UTF8.GetBytes(typeStr);

			//    md5.TransformBlock(loginData, 0, loginData.Length, loginData, 0);
			//    md5.TransformBlock(typeData, 0, typeData.Length, typeData, 0);
			//    md5.TransformFinalBlock(new byte[0], 0, 0);

			//    hashToken = Encoding.Default.GetString(md5.Hash);
			//}

			return hashToken;
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
