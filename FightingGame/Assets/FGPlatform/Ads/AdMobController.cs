using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
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

public enum AdvertisementType
{
	Banner, // 배너
	Interstitial // 전면 광고
}

namespace FGPlatform.Advertisement
{
    public interface IAdMobController
    {
        public void Init(BannerPosition bannerPos, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null);
        public void LoadAd(BannerPosition bannerPos = BannerPosition.Bottom, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null);
        public void UnloadAd();
        public void ShowAd(AdvertisementType type, Action<EventArgs> OnOpening = null, Action<EventArgs> OnClosed = null);
        public void HideAd(AdvertisementType type);
    }

	/// <summary>
	/// 배너
	/// </summary>

	public class AdMobController : IAdMobController
    {
        private readonly string AppID = "ca-app-pub-3940256099942544~3347511713";

        private FGPlatformAd bannerAd = null;
		private FGPlatformAd interstitialAd = null;

		public void Init(BannerPosition bannerPos, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null)
		{
            MobileAds.Initialize(initStatus => 
            {
                var map = initStatus.getAdapterStatusMap();

                foreach(var pair in map)
				{
                    string className = pair.Key;
                    var status = pair.Value;

                    MonoBehaviour.print($"Adapter : {className} is {status.Description}");
                }

                LoadAd(bannerPos, OnLoaded, OnLoadFailed);
            });
        }

        public void LoadAd(BannerPosition bannerPos, Action<EventArgs> OnLoaded = null, Action<string> OnLoadFailed = null)
        {
            LoadAd(bannerPos, OnLoaded, (AdFailedToLoadEventArgs args) =>
            {
                OnLoadFailed(args.LoadAdError?.GetResponseInfo()?.GetResponseId());
            });
        }

        private void LoadAd(BannerPosition bannerPos, Action<EventArgs> OnLoaded = null, Action<AdFailedToLoadEventArgs> OnLoadFailed = null)
        {
			var adPos = (AdPosition)Enum.Parse(typeof(AdPosition), bannerPos.ToString());
			bannerAd = AdFactory.Create(AdvertisementType.Banner, adPos);
            if(bannerAd != null)
            {
				bannerAd.OnAdLoaded += OnLoaded;
				bannerAd.OnAdFailedToLoad += OnLoadFailed;
			}

			interstitialAd = AdFactory.Create(AdvertisementType.Interstitial);
            if(interstitialAd != null)
            {
				interstitialAd.OnAdLoaded += OnLoaded;
				interstitialAd.OnAdFailedToLoad += OnLoadFailed;
			}
		}

        public void UnloadAd()
        {
            bannerAd?.Unload();
			interstitialAd?.Unload();
		}

        public void ShowAd(AdvertisementType type, Action<EventArgs> OnOpening = null, Action<EventArgs> OnClosed = null)
        {
            switch(type)
            {
                case AdvertisementType.Banner:
				    bannerAd?.Show();
                    break;

                case AdvertisementType.Interstitial:
					interstitialAd?.Show();
					break;
			}
		}

        public void HideAd(AdvertisementType type)
        {
			switch (type)
			{
				case AdvertisementType.Banner:
					bannerAd?.Hide();
					break;

				case AdvertisementType.Interstitial:
					break;
			}
		}
    }

}
