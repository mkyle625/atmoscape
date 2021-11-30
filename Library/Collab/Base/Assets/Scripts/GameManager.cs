using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    
    [Header("Unity Setup")] 
    public bool gameStart = false;
    public Player player;
    public PlayerCameraHolder playerCamera;
    public GameObject playspace;
    public SaveManager saveManager;
    public ObstacleSpawner obstacleSpawner;
    public string playerName;
    public int playerCredits;

    [Header("Debug stuff")]
    public string googleSignInMessage;

    public void Awake()
    {
        //Load the save
        SaveManager.Instance.Load("save");
        //Load the values from the save
        //SaveManager.Instance.ResetSave("save");
        playerName = SaveManager.Instance.state.playerName;
        playerCredits = SaveManager.Instance.state.playerCredits;

        //Activate google play
        StartGooglePlay();

    }

    void StartGooglePlay()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames()
            .Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success == true)
            {
                googleSignInMessage = "Logged in to Google Play Games Services";

            }
            else
            {
                googleSignInMessage = "Unable to sign in to Google Play Games Services";
                //mainText.text = "Could not login to Google Play Games Services";
                //mainText.color = Color.red;
            }
        });

        
    }

    //Stuff to do when the player clicks "play"
    public void startGame()
    {
        obstacleSpawner.Spawn();
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
    }

}
