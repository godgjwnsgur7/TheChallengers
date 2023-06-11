using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformBanner : FGPlatformAd
{
	private readonly string TestBannerID_AOS = "ca-app-pub-3940256099942544/6300978111";
	BannerView bannerView = null;

	public FGPlatformBanner(AdRequest adRequest, AdPosition adPos)
	{
		bannerView = new BannerView(TestBannerID_AOS, AdSize.SmartBanner, adPos);
		bannerView.LoadAd(adRequest);

		RegisterEvent();
	}

	public override void Show()
	{
		base.Show();

		bannerView.Show();
	}

	public override void Hide()
	{
		base.Hide();

		bannerView.Hide();
	}

	public override void Unload()
	{
		base.Unload();
		bannerView.Destroy();
	}

	protected override void RegisterEvent()
	{
		bannerView.OnAdLoaded += BannerView_OnAdLoaded;
		bannerView.OnAdFailedToLoad += BannerView_OnAdFailedToLoad;
		bannerView.OnAdClosed += BannerView_OnAdClosed;
		bannerView.OnAdOpening += BannerView_OnAdOpening;
		bannerView.OnPaidEvent += BannerView_OnPaidEvent;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		bannerView.OnAdLoaded -= BannerView_OnAdLoaded;
		bannerView.OnAdFailedToLoad -= BannerView_OnAdFailedToLoad;
		bannerView.OnAdClosed -= BannerView_OnAdClosed;
		bannerView.OnAdOpening -= BannerView_OnAdOpening;
		bannerView.OnPaidEvent -= BannerView_OnPaidEvent;
	}
}
