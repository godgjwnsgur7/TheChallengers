using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformInterstitial : FGPlatformAd
{
	private readonly string TestInterstitialID_AOS = "ca-app-pub-3940256099942544/1033173712";

	private InterstitialAd ad;

	public FGPlatformInterstitial(AdRequest adRequest)
	{
		ad = new InterstitialAd(TestInterstitialID_AOS);
		ad.LoadAd(adRequest);

		RegisterEvent();
	}

	public override void Show()
	{
		base.Show();

		ad.Show();
	}

	public override void Unload()
	{
		base.Unload();
		ad.Destroy();
	}

	protected override void RegisterEvent()
	{
		ad.OnAdLoaded += BannerView_OnAdLoaded;
		ad.OnAdFailedToLoad += BannerView_OnAdFailedToLoad;
		ad.OnAdClosed += BannerView_OnAdClosed;
		ad.OnAdOpening += BannerView_OnAdOpening;
		ad.OnPaidEvent += BannerView_OnPaidEvent;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		ad.OnAdLoaded -= BannerView_OnAdLoaded;
		ad.OnAdFailedToLoad -= BannerView_OnAdFailedToLoad;
		ad.OnAdClosed -= BannerView_OnAdClosed;
		ad.OnAdOpening -= BannerView_OnAdOpening;
		ad.OnPaidEvent -= BannerView_OnPaidEvent;
	}
}


