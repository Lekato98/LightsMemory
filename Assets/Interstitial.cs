using System;
using GoogleMobileAds.Api;
using UnityEngine;
using GoogleMobileAds.Common;

public class Interstitial : MonoBehaviour
{
    InterstitialAd interstitial;
    public string interstitialId = "ca-app-pub-1115893046616981/6302475235";
    void Start()
    {
        RequestInterstitial();
    }

    void RequestInterstitial()    {
        interstitial = new InterstitialAd(interstitialId);

        //call events
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        interstitial.OnAdOpening += HandleOnAdOpened;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;


        //create and ad request
        if (PlayerPrefs.HasKey("Consent"))
        {
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.LoadAd(request); //load & show the banner ad
        } else
        {
            AdRequest request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            interstitial.LoadAd(request); //load & show the banner ad (non-personalised)
        }
        
    }

    //show the ad
    public void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
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