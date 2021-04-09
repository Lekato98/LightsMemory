using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Common;

public class Rewarded : MonoBehaviour
{
    RewardedAd rewardAd;
    public Button watchAd;
    public string rewardId;
    void Start()
    {
        watchAd.onClick.AddListener(() => {
            rewardAd.Show();
        });
        RequestRewardedAd();
    }

    //make button interactable if video ad is ready
    void Update()
    {
        if (rewardAd.IsLoaded())
        {
            
            watchAd.interactable = true;
        }
    }

    void RequestRewardedAd()
    {
        rewardAd = new RewardedAd(rewardId);

        //call events
        rewardAd.OnAdLoaded += HandleRewardAdLoaded;
        rewardAd.OnAdFailedToLoad += HandleRewardAdFailedToLoad;
        rewardAd.OnAdOpening += HandleRewardAdOpening;
        rewardAd.OnAdFailedToShow += HandleRewardAdFailedToShow;
        rewardAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardAd.OnAdClosed += HandleRewardAdClosed;


        //create and ad request
        if (PlayerPrefs.HasKey("Consent"))
        {
            AdRequest request = new AdRequest.Builder().Build();
            rewardAd.LoadAd(request); //load & show the banner ad
        } else
        {
            AdRequest request = new AdRequest.Builder().AddExtra("npa", "1").Build();
            rewardAd.LoadAd(request); //load & show the banner ad (non-personalised)
        }
    }

    //attach to a button that plays ad if ready
    public void ShowRewardedAd()
    {
        if (rewardAd.IsLoaded())
        {
            rewardAd.Show();
        }
    }

    //call events
    public void HandleRewardAdLoaded(object sender, EventArgs args)
    {
        //do this when ad loads
    }

    public void HandleRewardAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        //do this when ad fails to loads
        Debug.Log("Ad failed to load" + args.Message);
    }

    public void HandleRewardAdOpening(object sender, EventArgs args)
    {
        //do this when ad is opening
    }

    public void HandleRewardAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        //do this when ad fails to show
    }

    public void HandleUserEarnedReward(object sender, EventArgs args)
    {
        //reward the player here
        //RevivePlayer();
    }

    public void HandleRewardAdClosed(object sender, EventArgs args)
    {
        //do this when ad is closed
        RequestRewardedAd();
        FindObjectOfType<MyGameManager>().RewardedExtraTime();
    }

}