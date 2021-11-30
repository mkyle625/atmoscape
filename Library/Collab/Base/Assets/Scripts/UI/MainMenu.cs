using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [Header("Unity Setup")] 
    public Button buttonPlay;
    public CanvasGroup canvasGroup;
    public GameHUD gameHUD;
    public CustomizationController customization;
    public ShopController shopController;
    public GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
    /** Show this HUD when the game starts **/
    public void EnableHUD()
    {
        //Enable this HUD
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }

    /** When you click the play button on the main menu **/
    public void ClickPlay()
    {
        //Disable main menu canvas to start the game
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        
        //Show the game HUD
        gameHUD.EnableHUD();
        
        //Enable player, game manager
        gameManager.startGame();
        
    }

    public void ClickSettings()
    {
        
    }

    public void ClickCustomize()
    {
        //Disable main menu canvas and open customization
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        
        //Show the customization HUD
        customization.EnableHUD();
    }

    public void ClickShop()
    {
        //Disable main menu canvas and open the shop
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;

        shopController.EnableHUD();
    }

    public void ClickHelp()
    {
        
    }
    
}
