using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHelpCanvas : MonoBehaviour
{

    public Animation helpCanvasAnimation;
    public GameObject tutorialCanvas;
    
    private bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Make sure the animation finishes before disabling the UI
        if (helpCanvasAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }
    }

    public void Click_Back()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        helpCanvasAnimation.Play("HelpClose");
        isOpen = false;
    }

    public void Click_Tutorial()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        tutorialCanvas.SetActive(true);
        SaveManager.Instance.state.lookAtTutorial = true;
        SaveManager.Instance.Save("Hypercharge");
        if (SaveManager.Instance.state.lookAtTutorial && SaveManager.Instance.state.lookAtPrivacyPolicy)
        {
            AchievementUnlocker.Unlock("informed");
        }
    }

    public void Click_PrivacyPolicy()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        Application.OpenURL("https://www.nitroxidestudios.com/atmoscape/privacy-policy");
        SaveManager.Instance.state.lookAtPrivacyPolicy = true;
        SaveManager.Instance.Save("Hypercharge");
        if (SaveManager.Instance.state.lookAtTutorial && SaveManager.Instance.state.lookAtPrivacyPolicy)
        {
            AchievementUnlocker.Unlock("informed");
        }
    }
}
