using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
   
    [Header("Shop prices")]
    public int shieldPrice;
    public int shieldPowerPrice;
    public int fuelPowerPrice;
    public int creditUpgradePrice;

    [Header("Colors")]
    public Color32 defaultColor;
    public Color32 defaultColorBackground;
    public Color32 disabledColor;
    public Color32 disabledColorBackground;
    public Color32 unlockedColor;
    public Color32 unlockedColorBackground;

    [Header("Unity Setup")]
    public CanvasGroup canvasGroup;
    public MainMenu mainMenu;
    public Button buttonUpgrade;
    public Button buttonRocketSkin;
    public GameObject upgradePanel;
    public GameObject rocketSkinPanel;
    public GameObject unlockNextSkinButton;
    public TextMeshProUGUI shopTypeText;
    public TextMeshProUGUI creditCountText;
    public TMP_FontAsset redFont;
    public TMP_FontAsset blueFont;
    public TMP_FontAsset greenFont;
    public Animation shopCanvasAnimation;
    public GameObject upgradeUnlockedCanvas;
    public Image playerProfileImage;
    public Image buttonRocketSkinImage;

    [Header("Upgrade Unlocked Icons")] 
    public Sprite shieldIcon;
    public Sprite shieldPowerIcon;
    public Sprite fuelPowerIcon;
    public Sprite doubleCreditIcon;
    public Sprite turningSpeedIcon;
    
    [Header("Rocket Skins")]
    public Image lockedIcon;
    public GameObject[] buyableSkins = new GameObject[50];
    public Sprite[] skinArray = new Sprite[50]; //Holds all the skins to apply to the player
    public SpriteRenderer playerSpriteRenderer; //Reference to player sprite renderer
    public TextMeshProUGUI purchasePriceText;

    [Header("Shop Buttons")]
    public Image upgradesButtonHoloBackground;
    public Image upgradesButtonCircleOutline;
    public Image upgradesButtonSymbol;
    public Image skinsButtonHoloBackground;
    public Image skinsButtonCircleOutline;
    public Image skinsButtonSymbol;

    [Header("Shield Upgrade")]
    public GameObject shieldUpgrade;
    public TextMeshProUGUI shieldUpgradeTitle;
    public Image shieldUpgradeBackgroundSqaure;
    public Image shieldUpgradeBackgroundCircleLeft;
    public Image shieldUpgradeBackgroundCircleRight;
    public TextMeshProUGUI shieldUpgradeCreditsAmount;
    public TextMeshProUGUI shieldUpgradeDescription;
    public Image shieldUpgradeLine;
    public Image shieldUpgradeSymbol;
    public Image shieldUpgradeHoloCircle;
    public Image shieldUpgradeOutlineCircle;

    [Header("Shield Power Upgrade")]
    public GameObject shieldPowerUpgrade;
    public TextMeshProUGUI shieldPowerUpgradeTitle;
    public Image shieldPowerUpgradeBackgroundSqaure;
    public Image shieldPowerUpgradeBackgroundCircleLeft;
    public Image shieldPowerUpgradeBackgroundCircleRight;
    public TextMeshProUGUI shieldPowerUpgradeCreditsAmount;
    public TextMeshProUGUI shieldPowerUpgradeDescription;
    public Image shieldPowerUpgradeLine;
    public Image shieldPowerUpgradeSymbol;
    public Image shieldPowerLevelCircle;
    public Image shieldPowerUpgradeHoloCircle;
    public Image shieldPowerUpgradeOutlineCircle;

    [Header("Fuel Power Upgrade")]
    public GameObject fuelPowerUpgrade;
    public TextMeshProUGUI fuelPowerUpgradeTitle;
    public Image fuelPowerUpgradeBackgroundSqaure;
    public Image fuelPowerUpgradeBackgroundCircleLeft;
    public Image fuelPowerUpgradeBackgroundCircleRight;
    public TextMeshProUGUI fuelPowerUpgradeCreditsAmount;
    public TextMeshProUGUI fuelPowerUpgradeDescription;
    public Image fuelPowerUpgradeLine;
    public Image fuelPowerUpgradeSymbol;
    public Image fuelPowerLevelCircle;
    public Image fuelPowerUpgradeHoloCircle;
    public Image fuelPowerUpgradeOutlineCircle;

    [Header("Credit Upgrade")]
    public GameObject creditUpgrade;
    public TextMeshProUGUI creditUpgradeTitle;
    public Image creditUpgradeBackgroundSqaure;
    public Image creditUpgradeBackgroundCircleLeft;
    public Image creditUpgradeBackgroundCircleRight;
    public TextMeshProUGUI creditUpgradeCreditsAmount;
    public TextMeshProUGUI creditUpgradeDescription;
    public Image creditUpgradeLine;
    public Image creditUpgradeSymbol;
    public Image creditUpgradeHoloCircle;
    public Image creditLevelCircle;
    public Image creditUpgradeOutlineCircle;
    
    [Header("Credit Upgrade")]
    public GameObject rocketSkin;
    public TextMeshProUGUI rocketSkinTitle;
    public Image rocketSkinBackgroundSqaure;
    public Image rocketSkinBackgroundCircleLeft;
    public Image rocketSkinBackgroundCircleRight;
    public TextMeshProUGUI rocketSkinCreditsAmount;
    public TextMeshProUGUI rocketSkinDescription;
    public Image rocketSkinLine;
    public Image rocketSkinSymbol;
    public Image rocketSkinHoloCircle;
    public Image rocketSkinOutlineCircle;

    private bool isOpen; 
    
    public void Start()
    {
        isOpen = true;
        
        //Load variables and set them to the upgrade panel
        LoadUpgrades();

        //Load rocket skin data
        LoadSkins();
        
        InitPanels();

        //Set shop prices
        shieldUpgradeCreditsAmount.text = shieldPrice.ToString();
        shieldPowerUpgradeCreditsAmount.text = shieldPowerPrice.ToString();
        fuelPowerUpgradeCreditsAmount.text = fuelPowerPrice.ToString();
        creditUpgradeCreditsAmount.text = creditUpgradePrice.ToString();
    }

    private void Update()
    {
        //Make sure the animation finishes before disabling the UI
        if (shopCanvasAnimation.isPlaying == false && isOpen == false)
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
            isOpen = true;
        }
    }

    public void InitPanels()
    {
        shopTypeText.text = "Upgrades";
        //Set the upgrade panel to start first
        //Reverse button colors
        skinsButtonHoloBackground.color = disabledColor;
        skinsButtonCircleOutline.color = disabledColor;
        skinsButtonSymbol.color = disabledColor;

        upgradesButtonHoloBackground.color = defaultColor;
        upgradesButtonCircleOutline.color = defaultColor;
        upgradesButtonSymbol.color = defaultColor;

        //Switch panels
        upgradePanel.SetActive(true);
        rocketSkinPanel.SetActive(false);
    }

    public void LoadUpgrades()
    {

        creditCountText.text = SaveManager.Instance.state.playerCredits.ToString();
        purchasePriceText.text = SaveManager.Instance.state.nextSkinPurchasePrice.ToString();

        //Shield Upgrade
        if (SaveManager.Instance.state.shieldPurchased)
        {
            //If purchased, change the color to reflect it
            shieldUpgrade.SetActive(false);

            //Unlock the ability to purchase the shield power upgrade
            shieldPowerUpgradeBackgroundSqaure.color = defaultColorBackground;
            shieldPowerUpgradeBackgroundCircleLeft.color = defaultColorBackground;
            shieldPowerUpgradeBackgroundCircleRight.color = defaultColorBackground;
            shieldPowerUpgradeCreditsAmount.enabled = true;
            shieldPowerUpgradeCreditsAmount.text = shieldPowerPrice.ToString();
            shieldPowerUpgradeDescription.text = "Reduce the amount of time it takes for the regenerating shield to recharge";
            shieldPowerUpgradeDescription.font = blueFont;
            shieldPowerUpgradeTitle.font = blueFont;
            shieldPowerUpgradeLine.color = defaultColor;
            shieldPowerUpgradeSymbol.color = defaultColor;
            shieldPowerUpgradeHoloCircle.color = defaultColor;
            shieldPowerUpgradeOutlineCircle.color = defaultColor;
            shieldPowerLevelCircle.fillAmount = 0.0f;
            shieldPowerUpgrade.SetActive(true);
        }
        else if (SaveManager.Instance.state.shieldPurchased == false)
        {
            //Put the credit amount
            shieldUpgradeCreditsAmount.enabled = true;
            shieldUpgradeCreditsAmount.text = shieldPrice.ToString();

            //Lock the ability to purchase the shield power upgrade
            shieldPowerUpgradeBackgroundSqaure.color = disabledColorBackground;
            shieldPowerUpgradeBackgroundCircleLeft.color = disabledColorBackground;
            shieldPowerUpgradeBackgroundCircleRight.color = disabledColorBackground;
            shieldPowerUpgradeCreditsAmount.enabled = false;
            shieldPowerUpgradeDescription.text = "This upgrade requires the \"Regenerating Shield\" upgrade first";
            shieldPowerUpgradeDescription.font = blueFont;
            shieldPowerUpgradeTitle.font = blueFont;
            shieldPowerUpgradeLine.color = disabledColor;
            shieldPowerUpgradeSymbol.color = disabledColor;
            shieldPowerUpgradeHoloCircle.color = disabledColor;
            shieldPowerUpgradeOutlineCircle.color = disabledColor;
            shieldPowerLevelCircle.fillAmount = 0.0f;
        }

        //Shield Power Upgrade - 5 levels (including 0)
        if (SaveManager.Instance.state.shieldPowerPurchased == 4)
        {
            shieldPowerUpgradeBackgroundSqaure.color = unlockedColorBackground;
            shieldPowerUpgradeBackgroundCircleLeft.color = unlockedColorBackground;
            shieldPowerUpgradeBackgroundCircleRight.color = unlockedColorBackground;
            shieldPowerUpgradeCreditsAmount.enabled = false;
            shieldPowerUpgradeDescription.text = "This upgrade is already purchased";
            shieldPowerUpgradeDescription.font = greenFont;
            shieldPowerUpgradeTitle.font = greenFont;
            shieldPowerUpgradeLine.color = unlockedColor;
            shieldPowerUpgradeSymbol.color = unlockedColor;
            shieldPowerUpgradeHoloCircle.color = unlockedColor;
            shieldPowerUpgradeOutlineCircle.color = unlockedColor;
            shieldPowerLevelCircle.fillAmount = 1.0f;
        }
        else if (SaveManager.Instance.state.shieldPowerPurchased == 3)
        {
            shieldPowerLevelCircle.fillAmount = 0.75f;
            shieldPowerPrice = 1200;
        }
        else if (SaveManager.Instance.state.shieldPowerPurchased == 2)
        {
            shieldPowerLevelCircle.fillAmount = 0.50f;
            shieldPowerPrice = 600;
        }
        else if (SaveManager.Instance.state.shieldPowerPurchased == 1)
        {
            shieldPowerLevelCircle.fillAmount = 0.25f;
            shieldPowerPrice = 300;
        }
        else if (SaveManager.Instance.state.shieldPowerPurchased == 0)
        {
            shieldPowerLevelCircle.fillAmount = 0.0f;
            shieldPowerPrice = 150;
        }

        //Fuel Power Upgrade
        if (SaveManager.Instance.state.fuelPowerPurchased == 4)
        {
            fuelPowerUpgradeBackgroundSqaure.color = unlockedColorBackground;
            fuelPowerUpgradeBackgroundCircleLeft.color = unlockedColorBackground;
            fuelPowerUpgradeBackgroundCircleRight.color = unlockedColorBackground;
            fuelPowerUpgradeCreditsAmount.enabled = false;
            fuelPowerUpgradeDescription.text = "This upgrade is already purchased";
            fuelPowerUpgradeDescription.font = greenFont;
            fuelPowerUpgradeTitle.font = greenFont;
            fuelPowerUpgradeLine.color = unlockedColor;
            fuelPowerUpgradeSymbol.color = unlockedColor;
            fuelPowerUpgradeHoloCircle.color = unlockedColor;
            fuelPowerUpgradeOutlineCircle.color = unlockedColor;
            fuelPowerLevelCircle.fillAmount = 1.0f;
        }
        else if (SaveManager.Instance.state.fuelPowerPurchased == 3)
        {
            fuelPowerLevelCircle.fillAmount = 0.75f;
            fuelPowerPrice = 800;
        }
        else if (SaveManager.Instance.state.fuelPowerPurchased == 2)
        {
            fuelPowerLevelCircle.fillAmount = 0.50f;
            fuelPowerPrice = 400;
        }
        else if (SaveManager.Instance.state.fuelPowerPurchased == 1)
        {
            fuelPowerLevelCircle.fillAmount = 0.25f;
            fuelPowerPrice = 200;
        }
        else if (SaveManager.Instance.state.shieldPowerPurchased == 0)
        {
            fuelPowerLevelCircle.fillAmount = 0.0f;
            fuelPowerPrice = 100;
        }
        
        //Credit Upgrade
        if (SaveManager.Instance.state.creditUpgradePurchased)
        {
            creditUpgradeBackgroundSqaure.color = unlockedColorBackground;
            creditUpgradeBackgroundCircleLeft.color = unlockedColorBackground;
            creditUpgradeBackgroundCircleRight.color = unlockedColorBackground;
            creditUpgradeCreditsAmount.enabled = false;
            creditUpgradeDescription.text = "This upgrade is already purchased";
            creditUpgradeDescription.font = greenFont;
            creditUpgradeTitle.font = greenFont;
            creditUpgradeLine.color = unlockedColor;
            creditUpgradeSymbol.color = unlockedColor;
            creditUpgradeHoloCircle.color = unlockedColor;
            creditUpgradeOutlineCircle.color = unlockedColor;
            creditLevelCircle.fillAmount = 1.0f;
        }
        
        //Skins button
        if (SaveManager.Instance.state.rocketSkinsPurchased == 50)
        {
            rocketSkinBackgroundSqaure.color = unlockedColorBackground;
            rocketSkinBackgroundCircleLeft.color = unlockedColorBackground;
            rocketSkinBackgroundCircleRight.color = unlockedColorBackground;
            rocketSkinCreditsAmount.enabled = false;
            rocketSkinDescription.text = "All skins unlocked";
            rocketSkinDescription.font = greenFont;
            rocketSkinTitle.font = greenFont;
            rocketSkinLine.color = unlockedColor;
            rocketSkinSymbol.color = unlockedColor;
            rocketSkinHoloCircle.color = unlockedColor;
            rocketSkinOutlineCircle.color = unlockedColor;
        }
        
        shieldPowerUpgradeCreditsAmount.text = shieldPowerPrice.ToString();
        fuelPowerUpgradeCreditsAmount.text = fuelPowerPrice.ToString();
    }

    void LoadSkins()
    {
        //Set images for all skins and buttons
        for (int i = 0; i < buyableSkins.Length; i++)
        {
            //buyableSkins[i].GetComponentInChildren<Image>().sprite = skinArray[i];
            Image[] images = buyableSkins[i].GetComponentsInChildren<Image>();
            for (int o = 0; o < images.Length; o++)
            {
                if (images[o].gameObject.CompareTag("Skin"))
                {
                    images[o].sprite = skinArray[i];
                }
            }
        }

        //Set unlocked state for skins that are unlocked
        for (int i = 0; i < SaveManager.Instance.state.rocketSkinsPurchased; i++)
        {
            buyableSkins[i].SetActive(true);
        }

        //Set the current skin
        playerSpriteRenderer = FindObjectOfType<Player>().GetComponentInChildren<SpriteRenderer>();
        playerSpriteRenderer.sprite = skinArray[SaveManager.Instance.state.currentSkinApplied];
        playerProfileImage.sprite = skinArray[SaveManager.Instance.state.currentSkinApplied];
        buttonRocketSkinImage.sprite = skinArray[SaveManager.Instance.state.currentSkinApplied];
    }

    public void EnableHUD()
    {
        //Enable this HUD
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        shopCanvasAnimation.Play("ShopOpen");

    }
    
    public void Click_Back()
    {
        shopCanvasAnimation.Play("ShopClose");
        isOpen = false;
        //Reload the scene (easy!)
        

    }

    public void Click_Upgrade()
    {
        shopTypeText.text = "Upgrades";

        //Reverse button colors
        skinsButtonHoloBackground.color = disabledColor;
        skinsButtonCircleOutline.color = disabledColor;
        skinsButtonSymbol.color = disabledColor;

        upgradesButtonHoloBackground.color = defaultColor;
        upgradesButtonCircleOutline.color = defaultColor;
        upgradesButtonSymbol.color = defaultColor;

        //Switch panels
        upgradePanel.SetActive(true);
        rocketSkinPanel.SetActive(false);

        //Make rocket button inactive
        unlockNextSkinButton.SetActive(false);
    }

    public void Click_RocketSkin()
    {
        shopTypeText.text = "Rocket Skins";

        //Reverse button colors
        upgradesButtonHoloBackground.color = disabledColor;
        upgradesButtonCircleOutline.color = disabledColor;
        upgradesButtonSymbol.color = disabledColor;

        skinsButtonHoloBackground.color = defaultColor;
        skinsButtonCircleOutline.color = defaultColor;
        skinsButtonSymbol.color = defaultColor;

        //Switch panels
        upgradePanel.SetActive(false);
        rocketSkinPanel.SetActive(true);

        //Make button active
        unlockNextSkinButton.SetActive(true);
    }

    public void Click_Shield()
    {
        Purchase("shield");
    }

    public void Click_ShieldPower()
    {
        Purchase("shieldPower");
    }

    public void Click_FuelPower()
    {
        Purchase("fuelPower");
    }

    public void Click_CreditUpgrade()
    {
        Purchase("creditUpgrade");
    }

    public void Click_NextSkin()
    {
        Purchase("nextSkin");
    }

    public void Click_Skin(int skinIndex)
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        playerSpriteRenderer = FindObjectOfType<Player>().GetComponentInChildren<SpriteRenderer>();
        playerSpriteRenderer.sprite = skinArray[skinIndex];

        //Save the skin applied
        SaveManager.Instance.state.currentSkinApplied = skinIndex;
        SaveManager.Instance.Save("Hypercharge");
        
        playerProfileImage.sprite = skinArray[SaveManager.Instance.state.currentSkinApplied];
        buttonRocketSkinImage.sprite = skinArray[SaveManager.Instance.state.currentSkinApplied];
    }

    public void Purchase(string upgrade)
    {
        switch (upgrade)
        {
            case "shield": //Shield upgrade
                if (SaveManager.Instance.state.playerCredits >= shieldPrice)
                {
                    //Play upgrade sound
                    SoundManager.Instance.PlayUpgradeClickSound();
                    
                    //Show the upgrade unlocked screen
                    upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeDescription.text =
                        "+ You can now purchase \"Shield Power\" upgrade";
                    upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeIcon.sprite = shieldIcon;
                    upgradeUnlockedCanvas.SetActive(true);

                    SaveManager.Instance.state.playerCredits -= shieldPrice;
                    SaveManager.Instance.state.shieldPurchased = true;
                    SaveManager.Instance.Save("Hypercharge");
                    LoadUpgrades(); //Reload shop
                }
                else
                {
                    SoundManager.Instance.PlayInvalidClickSound();
                }
                break;

            case "shieldPower": //Shield power upgrade
                //Check to make sure the player purchased the shield first
                if (SaveManager.Instance.state.shieldPurchased && SaveManager.Instance.state.playerCredits >= shieldPowerPrice)
                {
                    if (SaveManager.Instance.state.shieldPowerPurchased < 4)
                    {
                        //Play upgrade sound
                        SoundManager.Instance.PlayUpgradeClickSound();
                        
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeDescription.text =
                            "Upgraded to Level " + (SaveManager.Instance.state.shieldPowerPurchased + 1);
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeIcon.sprite = shieldPowerIcon;
                        upgradeUnlockedCanvas.SetActive(true);
                        
                        SaveManager.Instance.state.playerCredits -= shieldPowerPrice;
                        SaveManager.Instance.state.shieldPowerPurchased += 1;
                    }
                    SaveManager.Instance.Save("Hypercharge");
                    LoadUpgrades(); //Reload shop
                }
                else
                {
                    SoundManager.Instance.PlayInvalidClickSound();
                }
                break;

            case "fuelPower": //Fuel power upgrade
                if (SaveManager.Instance.state.playerCredits >= fuelPowerPrice)
                {
                    if (SaveManager.Instance.state.fuelPowerPurchased < 4)
                    {
                        //Play upgrade sound
                        SoundManager.Instance.PlayUpgradeClickSound();
                        
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeDescription.text =
                            "Upgraded to Level " + (SaveManager.Instance.state.fuelPowerPurchased + 1);
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeIcon.sprite = fuelPowerIcon;
                        upgradeUnlockedCanvas.SetActive(true);
                        
                        SaveManager.Instance.state.playerCredits -= fuelPowerPrice;
                        SaveManager.Instance.state.fuelPowerPurchased += 1;
                    }
                    SaveManager.Instance.Save("Hypercharge");
                    LoadUpgrades(); //Reload shop
                }
                else
                {
                    SoundManager.Instance.PlayInvalidClickSound();
                }
                break;
            
            case "creditUpgrade": //Fuel power upgrade
                if (SaveManager.Instance.state.playerCredits >= creditUpgradePrice)
                {
                    //Play upgrade sound
                    SoundManager.Instance.PlayUpgradeClickSound();
                    
                    upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeDescription.text =
                        "Double credits now have a chance to spawn";
                    upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeIcon.sprite = doubleCreditIcon;
                    upgradeUnlockedCanvas.SetActive(true);
                    
                    SaveManager.Instance.state.playerCredits -= creditUpgradePrice;
                    SaveManager.Instance.state.creditUpgradePurchased = true;
                    SaveManager.Instance.Save("Hypercharge");
                    LoadUpgrades(); //Reload shop
                }
                else
                {
                    SoundManager.Instance.PlayInvalidClickSound();
                }
                break;
            
            case "nextSkin":
                if (SaveManager.Instance.state.playerCredits >= SaveManager.Instance.state.nextSkinPurchasePrice)
                {
                    if (SaveManager.Instance.state.rocketSkinsPurchased < 50)
                    {
                        //Play upgrade sound
                        SoundManager.Instance.PlayUpgradeClickSound();
                        
                        Sprite nextSkinUnlockedSprite = shieldIcon;
                        for (int i = 0; i < skinArray.Length; i++)
                        {
                            nextSkinUnlockedSprite = skinArray[SaveManager.Instance.state.rocketSkinsPurchased];
                        }
                        
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeDescription.text =
                            " ";
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeTitle.text =
                            "Skin Unlocked";
                        upgradeUnlockedCanvas.GetComponent<UIUpgradeUnlockedCanvas>().upgradeIcon.sprite = nextSkinUnlockedSprite;
                        upgradeUnlockedCanvas.SetActive(true);

                        SaveManager.Instance.state.playerCredits -= SaveManager.Instance.state.nextSkinPurchasePrice;
                        SaveManager.Instance.state.rocketSkinsPurchased += 1;
                        SaveManager.Instance.state.nextSkinPurchasePrice += 2;
                    }
                    SaveManager.Instance.Save("Hypercharge");
                    LoadUpgrades(); //Reload shop
                    LoadSkins();
                    
                    //Check for achievements
                    if (SaveManager.Instance.state.rocketSkinsPurchased >= 9)
                        AchievementUnlocker.Unlock("skins1");
                    if (SaveManager.Instance.state.rocketSkinsPurchased >= 19)
                        AchievementUnlocker.Unlock("skins2");
                    if (SaveManager.Instance.state.rocketSkinsPurchased >= 29)
                        AchievementUnlocker.Unlock("skins3");
                    if (SaveManager.Instance.state.rocketSkinsPurchased >= 39)
                        AchievementUnlocker.Unlock("skins4");
                    if (SaveManager.Instance.state.rocketSkinsPurchased >= 49)
                        AchievementUnlocker.Unlock("skins5");
                }
                else
                {
                    SoundManager.Instance.PlayInvalidClickSound();
                }
                break;
        }
    }
}
