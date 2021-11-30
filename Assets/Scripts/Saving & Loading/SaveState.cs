using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState
{
    // Stores all important player information
    //Attributes
    public string playerName = "Player";
    public int playerCredits = 0;
    public int gamesPlayed = 0;
    public int highScore = 0;
    public int lastScore = 0;

    //Upgrades
    public bool shieldPurchased = false; //If the shield upgrade is purchased
    public int shieldPowerPurchased = 0; //0 = not purchased, 1 = level 1, etc.
    public int fuelPowerPurchased = 0; //0 = not purchased, 1 = level 1, etc.
    public int rocketSkinsPurchased = 1; //Goes to 50. 1 = starting skin
    public bool creditUpgradePurchased = false; //Either true or false
    public int nextSkinPurchasePrice = 2; //First skin is 5, increases by 5 everytime
    public int currentSkinApplied = 0; //Default 0 = first skin

    // Stores all important game settings
    public bool sfxMuted = false;
    public bool musicMuted = false;
    public bool watchedAd = false;
    
    //Achievement stuff
    public bool lookAtTutorial = false;
    public bool lookAtPrivacyPolicy = false;
    public int totalCreditsEarned = 0;
}
