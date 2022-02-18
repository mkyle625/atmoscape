using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _distanceFlownText; 
    public Animation _deathScreenAnimation;

    private void Start()
    {
        SaveManager.Instance.state.gamesPlayed += 1;
        _distanceFlownText.text = "You made it " + SaveManager.Instance.state.lastScore + " meters";
        _deathScreenAnimation.Play("DeathScreenShow");
        
        if (SaveManager.Instance.state.lastScore >= 100)
            AchievementUnlocker.Unlock("distance1");
        if (SaveManager.Instance.state.lastScore >= 250)
            AchievementUnlocker.Unlock("distance2");
        if (SaveManager.Instance.state.lastScore >= 500)
            AchievementUnlocker.Unlock("distance3");
        if (SaveManager.Instance.state.lastScore >= 750)
            AchievementUnlocker.Unlock("distance4");
        if (SaveManager.Instance.state.lastScore >= 1000)
            AchievementUnlocker.Unlock("distance5");
        
        if (SaveManager.Instance.state.totalCreditsEarned >= 100)
            AchievementUnlocker.Unlock("credits1");
        if (SaveManager.Instance.state.totalCreditsEarned >= 500)
            AchievementUnlocker.Unlock("credits2");
        if (SaveManager.Instance.state.totalCreditsEarned >= 1500)
            AchievementUnlocker.Unlock("credits3");
        if (SaveManager.Instance.state.totalCreditsEarned >= 5000)
            AchievementUnlocker.Unlock("credits4");
        if (SaveManager.Instance.state.totalCreditsEarned >= 10000)
            AchievementUnlocker.Unlock("credits5");
    }

    public void Click_Back()
    {
        //Play click sound
        SoundManager.Instance.PlayClickSound();
        
        // Show an ad first for that sweet money!
        //FindObjectOfType<AdManager>().ShowAd();
    
        // Save the game LoL
        SaveManager.Instance.Save("Hypercharge");
    
        //Go to the main menu (reload scene)
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

}
