using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [Header("Colors")]
    public Color32 disabledColor;
    public Color32 enabledColor;

    [Header("Music button")]
    public Image musicCircleHolo;
    public Image musicCircleOutline;
    public Image musicSymbol;
    
    [Header("Sound button")]
    public Image soundCircleHolo;
    public Image soundCircleOutline;
    public Image soundSymbol;

    [Header("Things to scale")] 
    public GameObject[] scales;
    
    void Start()
    {
        LoadSettings();
    }
    
    void LoadSettings()
    {
        //Load music settings
        if (SaveManager.Instance.state.musicMuted)
        {
            musicCircleHolo.color = disabledColor;
            musicCircleOutline.color = disabledColor;
            musicSymbol.color = disabledColor;
        }
        else
        {
            musicCircleHolo.color = enabledColor;
            musicCircleOutline.color = enabledColor;
            musicSymbol.color = enabledColor;
        }

        //Load sound settings
        if (SaveManager.Instance.state.sfxMuted)
        {
            soundCircleHolo.color = disabledColor;
            soundCircleOutline.color = disabledColor;
            soundSymbol.color = disabledColor;
        }
        else
        {
            soundCircleHolo.color = enabledColor;
            soundCircleOutline.color = enabledColor;
            soundSymbol.color = enabledColor;
        }
    }
    
    public void ClickResume()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Set the timescale back to 1
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    
    public void ClickMainMenu()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        //Unpause the game first
        Time.timeScale = 1f;
        
        //Quit to the main menu (reload scene)
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
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
            musicSymbol.color = enabledColor;
        }
        //Else mute the music
        else
        {
            SaveManager.Instance.state.musicMuted = true;
            musicCircleHolo.color = disabledColor;
            musicCircleOutline.color = disabledColor;
            musicSymbol.color = disabledColor;
        }

        //Save new settings
        SaveManager.Instance.Save("Hypercharge");
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
            soundSymbol.color = enabledColor;
        }
        //Else mute the sound
        else
        {
            SaveManager.Instance.state.sfxMuted = true;
            soundCircleHolo.color = disabledColor;
            soundCircleOutline.color = disabledColor;
            soundSymbol.color = disabledColor;
        }

        //Save new settings
        SaveManager.Instance.Save("Hypercharge");
    }
}
