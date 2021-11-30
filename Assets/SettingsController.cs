using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Colors")]
    public Color32 disabledColor;
    public Color32 disabledBackgroundColor;
    public Color32 enabledColor;
    public Color32 enabledBackgroundColor;

    [Header("Misc.")] 
    public Animation settingsAnimation;
    public Animation settingsConfirmBoxAnimation;
    
    [Header("Music panel")]
    public Image musicCircleHolo;
    public Image musicCircleOutline;
    public Image musicLine;
    public Image musicSymbol;
    public Image musicBackgroundSquare;
    public Image musicBackgroundCircle1;
    public Image musicBackgroundCircle2;
    public TextMeshProUGUI musicDescription;

    [Header("Sound panel")]
    public Image soundCircleHolo;
    public Image soundCircleOutline;
    public Image soundLine;
    public Image soundSymbol;
    public Image soundBackgroundSquare;
    public Image soundBackgroundCircle1;
    public Image soundBackgroundCircle2;
    public TextMeshProUGUI soundDescription;

    [Header("Reset save")]
    public GameObject confirmScreenCanvas;

    private bool isOpen;
    private bool isConfirmOpen;

    // Start is called before the first frame update
    void Start()
    {
        //Load settings to set colors

        isOpen = true;
        isConfirmOpen = true;
        
        LoadSettings();
       
    }

    private void Update()
    {
        //Make sure the animation finishes before disabling the UI
        if (settingsAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }

        if (settingsConfirmBoxAnimation.isPlaying == false && isConfirmOpen == false)
        {
            confirmScreenCanvas.SetActive(false);
            isConfirmOpen = true;
        }
    }

    void LoadSettings()
    {
        //Load music settings
        if (SaveManager.Instance.state.musicMuted)
        {
            musicCircleHolo.color = disabledColor;
            musicCircleOutline.color = disabledColor;
            musicLine.color = disabledColor;
            musicSymbol.color = disabledColor;
            musicBackgroundSquare.color = disabledBackgroundColor;
            musicBackgroundCircle1.color = disabledBackgroundColor;
            musicBackgroundCircle2.color = disabledBackgroundColor;
            musicDescription.text = "Music is disabled. Tap the button to turn it on.";
        }
        else
        {
            musicCircleHolo.color = enabledColor;
            musicCircleOutline.color = enabledColor;
            musicLine.color = enabledColor;
            musicSymbol.color = enabledColor;
            musicBackgroundSquare.color = enabledBackgroundColor;
            musicBackgroundCircle1.color = enabledBackgroundColor;
            musicBackgroundCircle2.color = enabledBackgroundColor;
            musicDescription.text = "Music is enabled. Tap the button to turn it off.";
        }

        //Load sound settings
        if (SaveManager.Instance.state.sfxMuted)
        {
            soundCircleHolo.color = disabledColor;
            soundCircleOutline.color = disabledColor;
            soundLine.color = disabledColor;
            soundSymbol.color = disabledColor;
            soundBackgroundSquare.color = disabledBackgroundColor;
            soundBackgroundCircle1.color = disabledBackgroundColor;
            soundBackgroundCircle2.color = disabledBackgroundColor;
            soundDescription.text = "Sound is disabled. Tap the button to turn it on.";
        }
        else
        {
            soundCircleHolo.color = enabledColor;
            soundCircleOutline.color = enabledColor;
            soundLine.color = enabledColor;
            soundSymbol.color = enabledColor;
            soundBackgroundSquare.color = enabledBackgroundColor;
            soundBackgroundCircle1.color = enabledBackgroundColor;
            soundBackgroundCircle2.color = enabledBackgroundColor;
            soundDescription.text = "Sound is enabled. Tap the button to turn it off.";
        }
    }

    public void ClickBack()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        settingsAnimation.Play("SettingsClose");
        isOpen = false;
        //Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void ClickMusic()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //If the music is already muted, unmute the music
        if (SaveManager.Instance.state.musicMuted)
        {
            SaveManager.Instance.state.musicMuted = false;
            musicCircleHolo.color = enabledColor;
            musicCircleOutline.color = enabledColor;
            musicLine.color = enabledColor;
            musicSymbol.color = enabledColor;
            musicBackgroundSquare.color = enabledBackgroundColor;
            musicBackgroundCircle1.color = enabledBackgroundColor;
            musicBackgroundCircle2.color = enabledBackgroundColor;
            musicDescription.text = "Music is enabled. Tap the button to turn it off.";
        }
        //Else mute the music
        else
        {
            SaveManager.Instance.state.musicMuted = true;
            musicCircleHolo.color = disabledColor;
            musicCircleOutline.color = disabledColor;
            musicLine.color = disabledColor;
            musicSymbol.color = disabledColor;
            musicBackgroundSquare.color = disabledBackgroundColor;
            musicBackgroundCircle1.color = disabledBackgroundColor;
            musicBackgroundCircle2.color = disabledBackgroundColor;
            musicDescription.text = "Music is disabled. Tap the button to turn it on.";
        }

        //Save new settings
        SaveManager.Instance.Save("Hypercharge");
        //Load settings
        SaveManager.Instance.Load("Hypercharge");
    }

    public void ClickSound()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //If the sound is already muted, unmute the sound
        if (SaveManager.Instance.state.sfxMuted)
        {
            SaveManager.Instance.state.sfxMuted = false;
            soundCircleHolo.color = enabledColor;
            soundCircleOutline.color = enabledColor;
            soundLine.color = enabledColor;
            soundSymbol.color = enabledColor;
            soundBackgroundSquare.color = enabledBackgroundColor;
            soundBackgroundCircle1.color = enabledBackgroundColor;
            soundBackgroundCircle2.color = enabledBackgroundColor;
            soundDescription.text = "Sound is enabled. Tap the button to turn it off.";
        }
        //Else mute the sound
        else
        {
            SaveManager.Instance.state.sfxMuted = true;
            soundCircleHolo.color = disabledColor;
            soundCircleOutline.color = disabledColor;
            soundLine.color = disabledColor;
            soundSymbol.color = disabledColor;
            soundBackgroundSquare.color = disabledBackgroundColor;
            soundBackgroundCircle1.color = disabledBackgroundColor;
            soundBackgroundCircle2.color = disabledBackgroundColor;
            soundDescription.text = "Sound is disabled. Tap the button to turn it on.";
        }

        //Save new settings
        SaveManager.Instance.Save("Hypercharge");
        //Load settings
        SaveManager.Instance.Load("Hypercharge");
    }

    public void ClickReset()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Open the confirm box
        confirmScreenCanvas.SetActive(true);
    }

    public void ClickCancel()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Close the confirm box
        settingsConfirmBoxAnimation.Play("SettingsConfirmScreenClose");
        isConfirmOpen = false;
    }

    public void ClickConfirm()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Reset save and reload scene to apply changes
        SaveManager.Instance.ResetSave("Hypercharge");
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    public void ClickCheat()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Add credits (only for testing)
        SaveManager.Instance.state.playerCredits += 1000;
        SaveManager.Instance.state.totalCreditsEarned += 1000;
        SaveManager.Instance.Save("Hypercharge");
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

}
