using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeUnlockedCanvas : MonoBehaviour
{
    [Header("Unity Setup")] 
    public TextMeshProUGUI upgradeTitle;
    public TextMeshProUGUI upgradeDescription;
    public Image upgradeIcon;
    public Animation upgradeUnlockedAnimation;

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
        if (upgradeUnlockedAnimation.isPlaying == false && isOpen == false)
        {
            gameObject.SetActive(false);
            isOpen = true;
        }
    }

    public void Click_Next()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        upgradeUnlockedAnimation.Play("UpgradeUnlockedClose");
        isOpen = false;
    }
}
