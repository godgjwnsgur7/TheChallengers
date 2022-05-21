using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BannerPosition
{
    Top = 0,
    Bottom = 1,
    TopLeft = 2,
    TopRight = 3,
    BottomLeft = 4,
    BottomRight = 5,
    Center = 6
}

public class AdBannerController
{
    private readonly string TestBannerID_AOS = "ca-app-pub-3940256099942544/6300978111";
    private readonly string TestKeyword = "ProjectFG";

    private BannerView bannerView = null;

    private event Action<EventArgs> OnAdLoaded = null;
    private event Action<AdFailedToLoadEventArgs> OnAdFailedToLoad = null;
    private event Action<EventArgs> OnAdOpening = null;
    private event Action<EventArgs> OnAdClosed = null;
    private event Action<AdValueEventArgs> OnPaidEvent = null;

    public void LoadBanner(BannerPosition bannerPos = BannerPosition.Bottom, Action<EventArgs> OnLoaded = null, Action<AdFailedToLoadEventArgs> OnLoadFailed = null)
    {
        var adPos = (AdPosition)Enum.Parse(typeof(AdPosition), bannerPos.ToString());
        bannerView = new BannerView(TestBannerID_AOS, AdSize.SmartBanner, adPos);

        RegisterEvent();

        OnAdLoaded += OnLoaded;
        OnAdFailedToLoad += OnLoadFailed;

        AdRequest req = new AdRequest.Builder()
            .AddKeyword(TestKeyword)
            .Build();

        bannerView.LoadAd(req); 
    }

    public void UnloadBanner()
    {
        UnregisterEvent();
        bannerView.Destroy();
    }

    private void RegisterEvent()
    {
        bannerView.OnAdLoaded += BannerView_OnAdLoaded;
        bannerView.OnAdFailedToLoad += BannerView_OnAdFailedToLoad;
        bannerView.OnAdClosed += BannerView_OnAdClosed;
        bannerView.OnAdOpening += BannerView_OnAdOpening;
        bannerView.OnPaidEvent += BannerView_OnPaidEvent;
    }

    private void UnregisterEvent()
    {
        bannerView.OnAdLoaded -= BannerView_OnAdLoaded;
        bannerView.OnAdFailedToLoad -= BannerView_OnAdFailedToLoad;
        bannerView.OnAdClosed -= BannerView_OnAdClosed;
        bannerView.OnAdOpening -= BannerView_OnAdOpening;
        bannerView.OnPaidEvent -= BannerView_OnPaidEvent;

        OnAdLoaded = null;
        OnAdFailedToLoad = null;
        OnAdOpening = null;
        OnAdClosed = null;
        OnPaidEvent = null;
    }

    private void BannerView_OnPaidEvent(object sender, AdValueEventArgs e)
    {
        OnPaidEvent?.Invoke(e);
    }

    private void BannerView_OnAdOpening(object sender, EventArgs e)
    {
        OnAdOpening?.Invoke(e);
    }

    private void BannerView_OnAdClosed(object sender, EventArgs e)
    {
        OnAdClosed?.Invoke(e);
    }

    private void BannerView_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        OnAdFailedToLoad?.Invoke(e);
    }

    private void BannerView_OnAdLoaded(object sender, EventArgs e)
    {
        OnAdLoaded?.Invoke(e);
    }

    public void ShowBanner(Action<EventArgs> OnShow = null)
    {
        OnAdOpening += OnShow;
        bannerView?.Show();
    }

    public void HideBanner(Action<EventArgs> OnHide = null)
    {
        OnAdClosed += OnHide;
        bannerView?.Hide();
    }
}
