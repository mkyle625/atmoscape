using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationController : MonoBehaviour
{
    [Header("Unity Setup")] 
    public Button buttonRocket; //Reference to the rocket button
    public Button buttonColor; //Reference to the color button
    public GameObject rocketPanel; //Reference to the rocket customization panel
    public GameObject colorPanel; //Reference to the color customization panel
    public Slider wallSliderR; //Reference to the wall red slider 
    public Slider wallSliderG; //Reference to the wall green slider
    public Slider wallSliderB; //Reference to the wall blue slider
    public GameObject leftWall; //Reference to the left wall
    public GameObject rightWall; //Reference to the right wall
    public Slider backgroundSliderR; //Reference to the background red slider
    public Slider backgroundSliderG; //Reference to the background green slider
    public Slider backgroundSliderB; //Reference to the background blue slider
    public CanvasGroup canvasGroup; //Reference to the canvas group
    public MainMenu mainMenu; //Reference to the main menu canvas

    private Button _testButton;

    /** Show this HUD when the game starts **/
    public void EnableHUD()
    {
        //Enable this HUD
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        //Set attributes
        //Rocket customization will be shown first, make color look unselected
        buttonColor.GetComponent<Image>().color = Color.gray;

        //Set values for sliders
        wallSliderR.value = 1;
        wallSliderG.value = 1;
        wallSliderB.value = 1;
        backgroundSliderR.value = 0;
        backgroundSliderG.value = 0;
        backgroundSliderB.value = 0;

    }

    public void Click_Back()
    {
        //Disable main menu canvas to start the game
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        
        //Show the customization HUD
        mainMenu.EnableHUD();
    }

    public void Click_Rocket()
    {
        //Reverse button colors
        buttonColor.GetComponent<Image>().color = Color.gray;
        buttonRocket.GetComponent<Image>().color = Color.white;
        
        //Switch panels
        rocketPanel.SetActive(true);
        colorPanel.SetActive(false);
        
    }
    
    public void Click_Color()
    {
        //Reverse button colors
        buttonColor.GetComponent<Image>().color = Color.white;
        buttonRocket.GetComponent<Image>().color = Color.gray;
        
        //Switch panels
        rocketPanel.SetActive(false);
        colorPanel.SetActive(true);
    }

    /** Update the color when the user touches the slider **/
    public void Update_Color()
    {
        //Anything that is tagged with 'Colorable' will change color
        // GameObject[] objects = GameObject.FindGameObjectsWithTag("Colorable");
        // foreach (var obj in objects)
        // {
        //     obj.GetComponent<Image>().color = new Color(sliderR.value, sliderG.value, sliderB.value);
        // }
        
        //Change color of walls
        leftWall.GetComponent<SpriteRenderer>().color = new Color(wallSliderR.value, wallSliderG.value, wallSliderB.value);
        rightWall.GetComponent<SpriteRenderer>().color = new Color(wallSliderR.value, wallSliderG.value, wallSliderB.value);
        
        //Change color of background
        Camera.main.backgroundColor = new Color(backgroundSliderR.value, backgroundSliderG.value, backgroundSliderB.value);
        
    }
    
    
}
