using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformInterstitial : FGPlatformAd
{
#if UNITY_EDITOR || !RELEASE
	private readonly string TestInterstitialID_AOS = "ca-app-pub-3940256099942544/1033173712";
#else
	private readonly string TestInterstitialID_AOS = "ca-app-pub-8487169308959261/7749885828";
#endif

	public override bool IsShow
	{
		get
		{
			return ad?.IsLoaded() ?? false;
		}
	}

	private InterstitialAd ad;

	public FGPlatformInterstitial(AdRequest adRequest) : base(adRequest)
	{
		ad = new InterstitialAd(TestInterstitialID_AOS);
		ad.LoadAd(request);

		RegisterEvent();
	}

	public override bool Show()
	{
		if (!ad.IsLoaded())
			ad.LoadAd(request);

		ad.Show();
		return true;
	}

	public override bool Hide()
	{
		Debug.LogError("전면 광고는 숨기기 기능이 없습니다.");
		return false;
	}

	public override void Unload()
	{
		base.Unload();
		ad.Destroy();
	}

	protected override void RegisterEvent()
	{
		base.RegisterEvent();

		ad.OnAdLoaded += OnAdLoadedView;
		ad.OnAdFailedToLoad += OnAdFailedToLoadView;
		ad.OnAdClosed += OnAdClosedView;
		ad.OnAdOpening += OnAdOpeningView;
		ad.OnPaidEvent += OnPaidEventView;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		ad.OnAdLoaded -= OnAdLoadedView;
		ad.OnAdFailedToLoad -= OnAdFailedToLoadView;
		ad.OnAdClosed -= OnAdClosedView;
		ad.OnAdOpening -= OnAdOpeningView;
		ad.OnPaidEvent -= OnPaidEventView;
	}
}


