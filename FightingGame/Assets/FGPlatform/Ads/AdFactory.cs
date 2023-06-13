using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AdFactory
{
	private const string TestKeyword = "ProjectFG";
	private static AdRequest request = null;
	private static AdRequest Request
	{
		get
		{
			if (request == null)
			{
				request = new AdRequest.Builder()
				.AddKeyword(TestKeyword)
				.Build();
			}

			return request;
		}
	}

	public static FGPlatformAd Create(AdvertisementType type, AdPosition adPos = AdPosition.Center)
	{
		switch (type)
		{
				case AdvertisementType.Banner:
#if ENABLE_BANNER
					return new FGPlatformBanner(Request, adPos);
#else
					Debug.LogError("ENABLE_BANNER 디파인이 없는 상태에서 배너 생성을 시도합니다.");
					break;
#endif

				case AdvertisementType.Interstitial:
#if ENABLE_INTERSTITIAL
					return new FGPlatformInterstitial(Request);
#else
					Debug.LogError("ENABLE_INTERSTITIAL 디파인이 없는 상태에서 전면 광고 생성을 시도합니다.");
					break;
#endif

			case AdvertisementType.Rewarded:
#if ENABLE_REWARDED
				return new FGPlatformRewarded(Request);
#else
					Debug.LogError("ENABLE_REWARDED 디파인이 없는 상태에서 보상형 광고 생성을 시도합니다.");
					break;
#endif

		}

		return null;
	}
}