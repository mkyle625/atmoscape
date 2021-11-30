using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGamesController : MonoBehaviour {

    public Text mainText;

    private void Start()
    {
        AuthenticateUser();
    }
    
    void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                Debug.Log("Logged in to Google Play Games Services");

                SceneManager.LoadScene("LeaderboardUI");
            }
            else
            {
                Debug.LogError("Unable to sign in to Google Play Games Services");
                //mainText.text = "Could not login to Google Play Games Services";
                //mainText.color = Color.red;
            }
        });
        
    }

    //public static void PostToLeaderboard(long newScore)
    //{
    //    Social.ReportScore(newScore, GPGSIds.leaderboard_high_score, (bool success) => {
    //        if (success)
    //        {
    //            Debug.Log("Posted new score to leaderboard");
    //        }
    //        else
    //        {
    //            Debug.LogError("Unable to post new score to leaderboard");
    //        }
    //    });
    //}

    //public static void ShowLeaderboardUI()
    //{
    //    PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
    //}
}
