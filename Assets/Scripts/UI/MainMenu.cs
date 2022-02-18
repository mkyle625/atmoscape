using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour
{

    [Header("Unity Setup")]
    public GameObject buttonPlay;
    public GameObject gameHudCanvas;
    public ShopController shopCanvas;
    public GameObject googlePlayCanvas;
    public GameObject settingsCanvas;
    public GameObject helpCanvas;
    public GameManager gameManager;
    public GameObject _particleEffect;
    public Animation _mainMenuAnimation;
    public Color32 buttonPlayColor;
    public Color32 buttonUpgradesColor;
    public Color32 buttonSettingsColor;
    public GameObject watchAdButton;
    
    [Header("Text objects")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerCreditsText;
    public TextMeshProUGUI gamesPlayedText;
    public TextMeshProUGUI playHighscore;
    public TextMeshProUGUI shopCredits;

    public bool watchAd;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //Set variables
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            playerNameText.text = Social.localUser.userName;
        }
        else
        {
            playerNameText.text = "Player";
        }

        playerCreditsText.text = "Total Credits Earned: " + SaveManager.Instance.state.totalCreditsEarned.ToString();
        gamesPlayedText.text = "Games Played: " + SaveManager.Instance.state.gamesPlayed.ToString();
        playHighscore.text = "Highscore: " + SaveManager.Instance.state.highScore;
        shopCredits.text = "Credits: " + SaveManager.Instance.state.playerCredits;
    }

    public void Update()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
        {
            playerNameText.text = Social.localUser.userName;
        }
        else
        {
            playerNameText.text = "Player";
        }
    }

    public void ToggleCanvasGroup(CanvasGroup c, bool toggle)
    {
        //True - enable canvas group
        if (toggle)
        {
            c.alpha = 1;
            c.interactable = true;
            c.blocksRaycasts = true;
        }
        else
        {
            c.alpha = 0;
            c.interactable = false;
            c.blocksRaycasts = false;
        }
    }
    
    /** Show this HUD when the game starts **/
    public void EnableHUD()
    {
        //Enable this HUD
        gameObject.SetActive(true);
    }

    private IEnumerator DelayDisable()
    {
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }

    public void ClickWatchAd()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //watchAd = true;
        //FindObjectOfType<AdManager>().ShowRewardedAd();
    }
    
    /** When you click the play button on the main menu **/
    public void ClickPlay()
    {
        if (_mainMenuAnimation.isPlaying == false)
        {
            //Play click sound
            SoundManager.Instance.PlayClickSound();

            //Disable main menu canvas to start the game
            _mainMenuAnimation.Play("MainMenuClickPlay");

            //StartCoroutine(DelayDisable());
        
            //Show the game HUD
            gameHudCanvas.SetActive(true);

            // Start HUD updates on game hud
            gameHudCanvas.GetComponent<GameHUD>().EnableHUD();

            //Enable player, game manager
            gameManager.startGame();

            StartCoroutine(DelayDisable());
        }
        
    }

    public void ClickSettings()
    {
        if (_mainMenuAnimation.isPlaying == false)
        {
            //Play click sound
            SoundManager.Instance.PlayClickSound();
            
            settingsCanvas.SetActive(true);
        }
       
    }

    public void ClickShop()
    {
        if (_mainMenuAnimation.isPlaying == false)
        {
            //Play click sound
            SoundManager.Instance.PlayClickSound();
            
            shopCanvas.EnableHUD();
            gameObject.SetActive(false);
        }
        
    }

    public void ClickHelp()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        helpCanvas.SetActive(true);
    }

    public void ClickGooglePlay()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        googlePlayCanvas.SetActive(true);
    }
    
}
