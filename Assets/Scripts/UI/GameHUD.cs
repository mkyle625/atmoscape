using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Unity Setup")]
    public TextMeshProUGUI fuelPercent;
    public Image fuelBar;
    public TextMeshProUGUI scoreText;
    public GameObject creditPickupNotification;
    public TextMeshProUGUI creditPickupNotificationAmountText;
    public GameObject pauseMenuCanvas;
    public Player player;
    public TextMeshProUGUI creditCountText;
    public Animation gameHudAnimation;

    //Private variables
    private bool isEnabled = false;

    /** Show this HUD when the game starts **/
    public void EnableHUD()
    {
        //Enable this HUD
        isEnabled = true;

        //fuelConsumptionText.text = "0.50/m";
        fuelPercent.text = "100%";
        fuelBar.fillAmount = 1.0f;
    }

    void Update()
    {
        if (isEnabled)
        {
            HandleFuelBar();
            UpdateScore();
            HandleCreditPickup();
        }
    }

    public void ClickPause()
    {
        //Open the pause menu
        pauseMenuCanvas.SetActive(true);
        
        //Pause the game
        Time.timeScale = 0f;
    }

    public void ClickRegularBoost()
    {
        if (SaveManager.Instance.state.playerCredits >= 50)
        {
            AchievementUnlocker.Unlock("boost");
            StartCoroutine(FindObjectOfType<Player>().StarterBoost(150));
            gameHudAnimation.Play("GameCanvasBoost");
            SaveManager.Instance.state.playerCredits -= 50;
            SaveManager.Instance.Save("Hypercharge");
        }
        
    }

    public void ClickUltraBoost()
    {
        if (SaveManager.Instance.state.playerCredits >= 100)
        {
            AchievementUnlocker.Unlock("boost");
            StartCoroutine(FindObjectOfType<Player>().StarterBoost(300));
            gameHudAnimation.Play("GameCanvasBoost");
            SaveManager.Instance.state.playerCredits -= 100;
            SaveManager.Instance.Save("Hypercharge");
        }
    }
    
    private void HandleCreditPickup()
    {
        creditCountText.text = SaveManager.Instance.state.playerCredits.ToString();
    }

    /// Handles the player's fuel bar ///
    void HandleFuelBar()
    {
        fuelPercent.text = Mathf.Round((fuelBar.fillAmount * 100)).ToString() + "%";
        fuelBar.fillAmount = Mathf.Lerp(fuelBar.fillAmount , player.currentFuel / player.fuelCapacity, 3f * Time.deltaTime);
    }

    void UpdateScore()
    {
        if (player.score >= 1000)
        {
            scoreText.text = Math.Round(player.score / 1000.0f, 2) + "K";
        }
        else
        {
            scoreText.text = player.score.ToString();
        }
        
    }

}
