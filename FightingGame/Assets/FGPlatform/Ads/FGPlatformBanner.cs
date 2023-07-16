using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGPlatformBanner : FGPlatformAd
{
#if UNITY_EDITOR || !RELEASE
	private readonly string TestBannerID_AOS = "ca-app-pub-3940256099942544/6300978111";
#else
	private readonly string TestBannerID_AOS = "ca-app-pub-8487169308959261/3239795176";
#endif

	BannerView bannerView = null;

	public FGPlatformBanner(AdRequest adRequest, AdPosition adPos) : base(adRequest)
	{
		bannerView = new BannerView(TestBannerID_AOS, AdSize.SmartBanner, adPos);
		bannerView.LoadAd(request);
		bannerView.Hide();

		RegisterEvent();
	}

	public override bool Show()
	{
		if (base.Show())
		{
			bannerView.Show();
			return true;
		}

		return false;
	}

	public override bool Hide()
	{
		if (base.Hide())
		{
			bannerView.Hide();
			return true;
		}

		return false;
	}

	public override void Unload()
	{
		base.Unload();
		bannerView.Destroy();
	}

	protected override void RegisterEvent()
	{
		base.RegisterEvent();

		bannerView.OnAdLoaded += OnAdLoadedView;
		bannerView.OnAdFailedToLoad += OnAdFailedToLoadView;
		bannerView.OnAdClosed += OnAdClosedView;
		bannerView.OnAdOpening += OnAdOpeningView;
		bannerView.OnPaidEvent += OnPaidEventView;
	}

	protected override void UnregisterEvent()
	{
		base.UnregisterEvent();

		bannerView.OnAdLoaded -= OnAdLoadedView;
		bannerView.OnAdFailedToLoad -= OnAdFailedToLoadView;
		bannerView.OnAdClosed -= OnAdClosedView;
		bannerView.OnAdOpening -= OnAdOpeningView;
		bannerView.OnPaidEvent -= OnPaidEventView;
	}
}
