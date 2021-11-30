using System;
using GooglePlayGames;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GooglePlayCanvas : MonoBehaviour
{
    [Header("Unity things")]
    public GameObject signInButton;
    public TextMeshProUGUI authenticationTitleText;
    public TextMeshProUGUI descriptionText;
    public GameObject signOutButton;
    public Animation googlePlayAnimation;

    public bool isOpen; //Is the canvas open?
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = true;
        
        //Player is signed in, display sign-out text/image
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            signInButton.SetActive(false);
            signOutButton.SetActive(true);
            
            authenticationTitleText.text = "Sign-out";
            descriptionText.text = "Sign-out of the Google Play Games services.";
        }
        else
        {
            signInButton.SetActive(true);
            signOutButton.SetActive(false);
            
            authenticationTitleText.text = "Sign-in";
            descriptionText.text = "Sign-in to the Google Play Games services.";
        }
    }

    private void Update()
    {
        //Make sure the animation finishes before disabling the UI
        if (googlePlayAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }
    }

    public void Click_Back()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        googlePlayAnimation.Play("GooglePlayClose");
        isOpen = false;
    }

    public void Click_Authenticate()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Checks if signed-in
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            //Player is signed in, sign out
            PlayGamesPlatform.Instance.SignOut();

            signInButton.SetActive(true);
            signOutButton.SetActive(false);
            
            authenticationTitleText.text = "Sign-in";
            descriptionText.text = "Sign-in to the Google Play Games services.";
        }
        else
        {
            //Player is signed out, sign in
            Social.localUser.Authenticate((bool success) =>
            {
                if (success == true)
                {
                    signInButton.SetActive(false);
                    signOutButton.SetActive(true);
                    
                    authenticationTitleText.text = "Sign-out";
                    descriptionText.text = "Sign-out of the Google Play Games services.";
                }
                
            });
        }
    }

    public void Click_Achievements()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        Social.ShowAchievementsUI();
    }

    public void Click_Leaderboards()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        Social.ShowLeaderboardUI();
    }

}
