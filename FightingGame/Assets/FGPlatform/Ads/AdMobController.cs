using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	Interstitial, // 전면 광고
    Rewarded // 보상형 광고
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
        private Dictionary<AdvertisementType, FGPlatformAd> banners = new Dictionary<AdvertisementType, FGPlatformAd>();

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
			var bannerAd = AdFactory.Create(AdvertisementType.Banner, adPos);
            if(bannerAd != null)
            {
				bannerAd.OnAdLoaded += OnLoaded;
				bannerAd.OnAdFailedToLoad += OnLoadFailed;
			}

            banners[AdvertisementType.Banner] = bannerAd;

			var interstitialAd = AdFactory.Create(AdvertisementType.Interstitial);
            if(interstitialAd != null)
            {
				interstitialAd.OnAdLoaded += OnLoaded;
				interstitialAd.OnAdFailedToLoad += OnLoadFailed;
			}

			banners[AdvertisementType.Interstitial] = interstitialAd;

			var rewardedAd = AdFactory.Create(AdvertisementType.Rewarded);
			if (rewardedAd != null)
			{
				rewardedAd.OnAdLoaded += OnLoaded;
				rewardedAd.OnAdFailedToLoad += OnLoadFailed;
			}

			banners[AdvertisementType.Rewarded] = rewardedAd;
		}

        public void UnloadAd()
        {
            foreach(var banner in banners.Select(p => p.Value))
            {
				banner?.Unload();
            }
		}

        public void ShowAd(AdvertisementType type, Action<EventArgs> OnOpening = null, Action<EventArgs> OnClosed = null)
        {
            if (banners.ContainsKey(type))
            {
				banners[type]?.Show();
			}
		}

        public void HideAd(AdvertisementType type)
        {
			if (banners.ContainsKey(type))
			{
				banners[type]?.Hide();
			}
		}
    }

}
