using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        ToggleCanvasGroup(canvasGroup, true);
    }

    /** When you click the play button on the main menu **/
    public void ClickPlay()
    {
        //Disable main menu canvas to start the game
        ToggleCanvasGroup(canvasGroup, false);
        
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
        ToggleCanvasGroup(canvasGroup, false);

        //Show the customization HUD
        customization.EnableHUD();
    }

    public void ClickShop()
    {
        //Disable main menu canvas and open the shop
        ToggleCanvasGroup(canvasGroup, false);

        shopController.EnableHUD();
    }

    public void ClickHelp()
    {
        
    }
    
}
