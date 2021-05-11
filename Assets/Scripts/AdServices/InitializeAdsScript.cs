using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Advertisements;

public class InitializeAdsScript : MonoBehaviour, IUnityAdsListener
{
    [SerializeField, Range(0f, 1f)]
    float adsChanceProc = 0.7f;

    [SerializeField, Range(0f, 1f)]
    float videoChanceProc = 0.3f;


    [SerializeField]
    bool testMode = false;

    string gameId = "4119085";
    string mySurfacingId = "rewardedVideo";

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.onGameEnd += ShowRewardedVideo;
        GameManager.Instance.onGameRestarted += OnGameRestartedAds;

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    private void OnGameRestartedAds()
    {
        float r = Random.Range(0f, 1f);

        if (r <= adsChanceProc)
        {
            r = Random.Range(0f, 1f);
            if (r <= videoChanceProc && Advertisement.IsReady(mySurfacingId))
            {
                ShowRewardedVideo();
            }
            else
            {
                ShowInterstitialAd();
            }
        }

        
        
    }


    public void ShowRewardedVideo()
    {
        
        if (Advertisement.IsReady(mySurfacingId))
        {
            Advertisement.Show(mySurfacingId);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }


    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show("GameEnd");
            // Replace yourPlacementID with the ID of the placements you wish to display as shown in your Unity Dashboard.
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("Finished!");
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Debug.Log("Skipped!");
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    
}
