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

namespace FGPlatform.Advertisement
{
    public interface IAdMobController
    {
        public void LoadBanner(BannerPosition bannerPos = BannerPosition.Bottom, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null);
        public void UnloadBanner();
        public void ShowBanner(Action<EventArgs> OnShow = null);
        public void HideBanner(Action<EventArgs> OnHide = null);

    }

    /// <summary>
    /// 배너
    /// </summary>

    public partial class AdMobController : IAdMobController
    {
        private readonly string TestBannerID_AOS = "ca-app-pub-3940256099942544/6300978111";
		private readonly string TestKeyword = "ProjectFG";

        private bool isLoaded = false;
        private bool isShow = false;

		private BannerView bannerView = null;

        private event Action<EventArgs> OnAdLoaded = null;
        private event Action<AdFailedToLoadEventArgs> OnAdFailedToLoad = null;
        private event Action<EventArgs> OnAdOpening = null;
        private event Action<EventArgs> OnAdClosed = null;
        private event Action<AdValueEventArgs> OnPaidEvent = null;

        public void LoadBanner(BannerPosition bannerPos = BannerPosition.Bottom, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null)
        {
            LoadBanner(bannerPos, OnLoaded, (AdFailedToLoadEventArgs args) =>
            {
                OnLoadFailed(args.LoadAdError?.GetResponseInfo()?.GetResponseId());
            });
        }

        private void LoadBanner(BannerPosition bannerPos = BannerPosition.Bottom, Action<EventArgs> OnLoaded = null, Action<AdFailedToLoadEventArgs> OnLoadFailed = null)
        {
            if (isLoaded)
			{
                Debug.LogError("이미 배너가 로드되어있습니다.");
                return;
            }

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

        public void ShowBanner(Action<EventArgs> OnShow = null)
        {
            if (isShow)
			{
                Debug.LogError("이미 광고 배너가 떠 있습니다.");
                return;
            }

            OnAdOpening += OnShow;
            bannerView?.Show();

            isShow = true;
        }

        public void HideBanner(Action<EventArgs> OnHide = null)
        {
            if (!isShow)
            {
                Debug.LogError("이미 광고 배너가 떠 있지 않습니다.");
                return;
            }

            OnAdClosed += OnHide;
            bannerView?.Hide();

            isShow = false;
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
            
            isLoaded = false;
            isShow = false;
        }

        private void BannerView_OnAdLoaded(object sender, EventArgs e)
        {
            OnAdLoaded?.Invoke(e);

            isLoaded = true;
            isShow = true;
        }
    }

}
