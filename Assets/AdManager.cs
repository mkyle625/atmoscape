using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    public MainMenu _mainMenu;
    
    private string gameId = "3435012";

    //private string placementId_video = "video";
    private string placementId_rewardedVideo = "rewardedVideo";

    public bool testMode = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameId = "3435013";
        } 
        else if (Application.platform == RuntimePlatform.Android)
        {
            gameId = "3435012";
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, testMode);
    }

    public void ShowAd()
    {
        Advertisement.Show();
    }

    public void ShowRewardedAd()
    {
        Advertisement.Show(placementId_rewardedVideo);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, UnityEngine.Advertisements.ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == UnityEngine.Advertisements.ShowResult.Finished)
        {
            // Ad type: Out of fuel, give the player fuel and continue on finish
            if (FindObjectOfType<Player>().stalled)
            {
                FindObjectOfType<Player>().Refuel();
            }
            //Ad type: Watch ad on screen
            if (_mainMenu.watchAd)
            {
                SaveManager.Instance.state.playerCredits += 20;
                SaveManager.Instance.state.totalCreditsEarned += 20;
                SaveManager.Instance.Save("Hypercharge");
                _mainMenu.watchAd = false;
                Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            }
        }
        else if (showResult == UnityEngine.Advertisements.ShowResult.Skipped)
        {
            
        }
        else if (showResult == UnityEngine.Advertisements.ShowResult.Failed)
        {
            
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == placementId_rewardedVideo)
        {
            _mainMenu.watchAdButton.SetActive(true);       
        }
        else
        {
            _mainMenu.watchAdButton.SetActive(false);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

}
