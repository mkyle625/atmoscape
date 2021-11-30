using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    
    [Header("Unity Setup")] 
    public bool gameStart = false;
    public Player player;
    public PlayerCameraHolder playerCamera;
    public GameObject playspace;
    public ObstacleSpawner obstacleSpawner;
    public AudioMixer masterMixer;
    public TimeManager timeManager;
    public Transform highscoreBar;
    public Transform highscoreBarCanvas;
    public TextMeshProUGUI highscoreBarText;

    public void Awake()
    {
        //Activate google play, only if user is not already logged in
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            StartGooglePlay();
            
            //Check achievements
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

        SaveManager.Instance.state.watchedAd = false;
    }

    private void Start()
    {
        SoundManager.Instance.ToggleMusic(false,true);
    }

    void StartGooglePlay()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        { });

    }

    //Stuff to do when the player clicks "play"
    public void startGame()
    {
        UpdateHighscoreBar();
        obstacleSpawner.BeginSpawning();
        player.BeginFlight();
        playerCamera.followPlayer = true;
        gameStart = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        //Only update when the player clicks "play"
        if (gameStart)
        {
            //Stuff to update here
            
            //Move the playspace with the player
            if (player != null) // Check if the player exists
                playspace.transform.position = new Vector3(playspace.transform.position.x, player.transform.position.y, playspace.transform.position.z);
            
        }

        if (SaveManager.Instance.state.sfxMuted || Time.timeScale <= 0f && !timeManager._inSlowdown)
        {
            masterMixer.SetFloat("SFXVolume",-80f);
        }
        else
        {
            masterMixer.SetFloat("SFXVolume",0f);
        }
        
        if (SaveManager.Instance.state.musicMuted)
        {
            masterMixer.SetFloat("MusicVolume",-80f);
        }
        else
        {
            masterMixer.SetFloat("MusicVolume",0f);
        }
        
        highscoreBarCanvas.position = Vector3.Lerp(highscoreBarCanvas.position, new Vector3(player.transform.position.x,highscoreBarCanvas.position.y,highscoreBarCanvas.position.z),2f * Time.deltaTime );
    }

    public void UpdateHighscoreBar()
    {
        if (SaveManager.Instance.state.highScore > 50f)
        {
            highscoreBar.position = new Vector3(0f,SaveManager.Instance.state.highScore,0f);
            highscoreBarText.text = SaveManager.Instance.state.highScore.ToString() + " m";
        }
    }

}
