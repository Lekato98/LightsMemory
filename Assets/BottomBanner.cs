using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class BottomBanner : MonoBehaviour
{
    BannerView banner;
    public string bannerId;
    
    public void Start() {
        RequestBanner();
    }

    public void RequestBanner() {
        banner = new BannerView(bannerId, AdSize.Banner, AdPosition.Bottom);
        
        //call events
        banner.OnAdLoaded += HandleOnAdLoaded;
        banner.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        banner.OnAdOpening += HandleOnAdOpened;
        banner.OnAdClosed += HandleOnAdClosed;
        banner.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        //create and ad request
        if (PlayerPrefs.HasKey("Consent"))
        {
            AdRequest request = new AdRequest.Builder().Build();
            banner.LoadAd(request); //load & show the banner ad
        } else
        {
            AdRequest request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            banner.LoadAd(request); //load & show the banner ad (non-personalised)
        }
    }



    //events below
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        //do this when ad loads
    }

    public void HandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        //do this when ad fails to load
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //do this when ad is opened
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //do this when ad is closed
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        //do this when on leaving application;
    }
}