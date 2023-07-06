using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FGPlatformAd
{
	public bool isLoaded
	{
		get;
		private set;
	}

	public bool isShow
	{
		get;
		private set;
	}

	protected AdRequest request = null;

	public event Action<EventArgs> OnAdLoaded = null;
	public event Action<AdFailedToLoadEventArgs> OnAdFailedToLoad = null;
	public event Action<EventArgs> OnAdOpening = null;
	public event Action<EventArgs> OnAdClosed = null;
	public event Action<AdValueEventArgs> OnPaidEvent = null;

	public FGPlatformAd(AdRequest request)
	{
		this.request = request;
	}

	public virtual bool Show()
	{
		if (isShow)
		{
			Debug.LogError($"이미 {this.GetType()} 타입 광고가 떠 있습니다.");
			return false;
		}

		isShow = true;
		return true;
	}

	public virtual bool Hide()
	{
		if (!isShow)
		{
			Debug.LogError($"이미 {this.GetType()} 타입 광고가 꺼져 있습니다.");
			return false;
		}

		isShow = false;
		return true;
	}

	public virtual void Unload()
	{
		isLoaded = false;
		UnregisterEvent();
	}

	// 정의만
	protected virtual void RegisterEvent()
	{

	}

	// 정의만
	protected virtual void UnregisterEvent()
	{
		OnAdLoaded = null;
		OnAdFailedToLoad = null;
		OnAdOpening = null;
		OnAdClosed = null;
		OnPaidEvent = null;
	}

	protected void BannerView_OnPaidEvent(object sender, AdValueEventArgs e)
	{
		OnPaidEvent?.Invoke(e);
		OnPaidEvent = null;
	}

	protected void BannerView_OnAdOpening(object sender, EventArgs e)
	{
		OnAdOpening?.Invoke(e);
		OnAdOpening = null;
	}

	protected void BannerView_OnAdClosed(object sender, EventArgs e)
	{
		OnAdClosed?.Invoke(e);
		OnAdClosed = null;
	}

	protected void BannerView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
	{
		OnAdFailedToLoad?.Invoke(e);
		OnAdFailedToLoad = null;

		isLoaded = false;
	}

	protected void BannerView_OnAdLoaded(object sender, EventArgs e)
	{
		OnAdLoaded?.Invoke(e);
		OnAdLoaded = null;

		isLoaded = true;
	}

}
