using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformRewarded : FGPlatformAd
{
	private readonly string TestRewardedID_AOS = "ca-app-pub-3940256099942544/5224354917";

	private RewardedAd rewardedAd = null;

	public FGPlatformRewarded(AdRequest request) : base(request)
	{
		rewardedAd = new RewardedAd(TestRewardedID_AOS);
		rewardedAd.LoadAd(request);
	}

	public override bool Show()
	{
		if (base.Show())
		{
			if (!rewardedAd.IsLoaded())
				rewardedAd.LoadAd(request);

			rewardedAd.Show();
			return true;
		}

		return false;
	}

	public override bool Hide()
	{
		Debug.LogError("보상형 광고는 숨기기 기능이 없습니다.");
		return false;
	}

	public override void Unload()
	{
		base.Unload();
		rewardedAd.Destroy();
	}

	protected override void RegisterEvent()
	{
		rewardedAd.OnAdLoaded += BannerView_OnAdLoaded;
		rewardedAd.OnAdFailedToLoad += BannerView_OnAdFailedToLoad;
		rewardedAd.OnAdClosed += BannerView_OnAdClosed;
		rewardedAd.OnAdOpening += BannerView_OnAdOpening;
		rewardedAd.OnPaidEvent += BannerView_OnPaidEvent;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		rewardedAd.OnAdLoaded -= BannerView_OnAdLoaded;
		rewardedAd.OnAdFailedToLoad -= BannerView_OnAdFailedToLoad;
		rewardedAd.OnAdClosed -= BannerView_OnAdClosed;
		rewardedAd.OnAdOpening -= BannerView_OnAdOpening;
		rewardedAd.OnPaidEvent -= BannerView_OnPaidEvent;
	}
}
