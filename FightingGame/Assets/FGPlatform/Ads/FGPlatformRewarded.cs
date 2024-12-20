using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformRewarded : FGPlatformAd
{
#if UNITY_EDITOR || !RELEASE
	private readonly string TestRewardedID_AOS = "ca-app-pub-3940256099942544/5224354917";
#else
	private readonly string TestRewardedID_AOS = "ca-app-pub-8487169308959261/6129276840";
#endif

	public override bool IsShow
	{
		get
		{
			return rewardedAd?.IsLoaded() ?? false;
		}
	}

	private RewardedAd rewardedAd = null;

	public FGPlatformRewarded(AdRequest request) : base(request)
	{
		rewardedAd = new RewardedAd(TestRewardedID_AOS);
		rewardedAd.LoadAd(request);

		RegisterEvent();
	}

	public override bool Show()
	{
		if (!rewardedAd.IsLoaded())
			rewardedAd.LoadAd(request);

		rewardedAd.Show();
		return true;
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
		base.RegisterEvent();

		rewardedAd.OnAdLoaded += OnAdLoadedView;
		rewardedAd.OnAdFailedToLoad += OnAdFailedToLoadView;
		rewardedAd.OnAdClosed += OnAdClosedView;
		rewardedAd.OnAdOpening += OnAdOpeningView;
		rewardedAd.OnPaidEvent += OnPaidEventView;
		rewardedAd.OnUserEarnedReward += OnUserEarnedRewardView;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		rewardedAd.OnAdLoaded -= OnAdLoadedView;
		rewardedAd.OnAdFailedToLoad -= OnAdFailedToLoadView;
		rewardedAd.OnAdClosed -= OnAdClosedView;
		rewardedAd.OnAdOpening -= OnAdOpeningView;
		rewardedAd.OnPaidEvent -= OnPaidEventView;
		rewardedAd.OnUserEarnedReward -= OnUserEarnedRewardView;
	}
}
