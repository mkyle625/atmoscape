using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WatchAdScreen : MonoBehaviour
{

    public Animation _watchAdAnimation;
    public TextMeshProUGUI _distanceFlownText;
    public GameObject _watchAdText;
    public GameObject _watchAdButton;
    
    private bool isOpen;

    private void OnEnable()
    {
        Start();
    }

    private void Start()
    {
        if (SaveManager.Instance.state.watchedAd)
        {
            _watchAdText.SetActive(false);
            _watchAdButton.SetActive(false);
        }
        
        _distanceFlownText.text = "You made it " + (int)FindObjectOfType<Player>().transform.position.y + " meters";
        isOpen = true;
        
        if (SaveManager.Instance.state.lastScore >= 100)
            AchievementUnlocker.Unlock("distance1");
        if (SaveManager.Instance.state.lastScore >= 250)
            AchievementUnlocker.Unlock("distance2");
        if (SaveManager.Instance.state.lastScore >= 500)
            AchievementUnlocker.Unlock("distance3");
        if (SaveManager.Instance.state.lastScore >= 750)
            AchievementUnlocker.Unlock("distance4");
        if (SaveManager.Instance.state.lastScore >= 1000)
            AchievementUnlocker.Unlock("distance5");
        
        if (SaveManager.Instance.state.totalCreditsEarned >= 100)
            AchievementUnlocker.Unlock("credits1");
        if (SaveManager.Instance.state.totalCreditsEarned >= 500)
            AchievementUnlocker.Unlock("credits2");
        if (SaveManager.Instance.state.totalCreditsEarned >= 1500)
            AchievementUnlocker.Unlock("credits3");
        if (SaveManager.Instance.state.totalCreditsEarned >= 5000)
            AchievementUnlocker.Unlock("credits4");
        if (SaveManager.Instance.state.totalCreditsEarned >= 10000)
            AchievementUnlocker.Unlock("credits5");
    }

    private void Update()
    {
        //Make sure the animation finishes before disabling the UI
        if (_watchAdAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }
    }

    public void Click_Back()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        _watchAdAnimation.Play("WatchAdDeathScreenClose");
        FindObjectOfType<Player>().stalled = false;
        FindObjectOfType<Player>().Die(null);

        isOpen = false;

    }

    public void Click_WatchAd()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Call to unity ads here or something like that
        gameObject.SetActive(false); 
        FindObjectOfType<AdManager>().ShowRewardedAd();
        SaveManager.Instance.state.watchedAd = true;
    }    
}
