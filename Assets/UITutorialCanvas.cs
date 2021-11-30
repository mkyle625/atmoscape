using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialCanvas : MonoBehaviour
{
    public Animation _tutorialCanvasAnimation;

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
        if (_tutorialCanvasAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }
    }

    public void ClickBack()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        _tutorialCanvasAnimation.Play("TutorialClose");
        isOpen = false;
    }
}
