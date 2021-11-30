using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Shield : MonoBehaviour
{
    [Header("Variables")]
    public float shieldRechargeTime;
    public bool shieldRecharged = false;

    [Header("GFX")]
    public Image shield; // The player's shield world UI image
    public Sprite shieldSprite;
    public Sprite shieldChargeSprite;
    public ParticleSystem shieldBreakParticles;
    public ParticleSystem shieldRechargedParticles;

    [Header("SFX")] 
    public AudioSource breakSound;
    public AudioSource shieldRechargeSound;
    
    [Header("Animation")]
    public Animator animator;
    public String shieldPulseName = "Shield Pulse";

    [Header("Unity Setup")] 
    public Player player;
    
    /* Private Variables */
    private bool shieldOwned = false; // Stores whether the player has bought the shield upgrade or not
    private float currentCharge = 0;

    /// Run through the Player script when the game begins ///
    public void StartShield()
    {
        //Check that the player has the shield
        if (SaveManager.Instance.state.shieldPurchased && !player.tutorialMode)
        {
            // Enable the shield to recharge
            shieldOwned = true; 
            
            // Set the current charge to max
            currentCharge = shieldRechargeTime;
        }
    }

    void Update()
    {
        // Check if the shield is owned, and don't run anything if it isn't
        if (!shieldOwned)
            return;

        // Controls variables based on shield state and current charge
        if (!shieldRecharged && currentCharge < shieldRechargeTime)
        {
            // Increments current charge by 1 each second while shield is innactive
            currentCharge += 1f * Time.deltaTime;

            // Sets the shield sprite to the charging sprite
            shield.sprite = shieldChargeSprite;

            // Updates the shield image's fill
            UpdateShieldImage();
        }
        else
        {
            // Checks if the shield is already set to active or not
            if (!shieldRecharged)
            {
                shieldRecharged = true; // Sets shield state to active

                // Runs once when the shield reaches a full charge in order to play the animation
                shieldRechargedParticles.Play(); // Plays particles for when the shield reaches a full charge
                
                shieldRechargeSound.pitch = 1.5f;
                shieldRechargeSound.Play();
            }

            // Sets the currentCharge to the shieldRechargeTime (full)
            currentCharge = shieldRechargeTime;

            // Sets the shield sprite to its charged sprite and fill ammount to 1 (full)
            shield.sprite = shieldSprite;
            shield.fillAmount = 1f;
        }
    }

    /// Breaks the shield. Controlled through the Player script ///
    public void BreakShield()
    {
        // Play the shield break sound
        breakSound.Play();
        
        // Reset the current charge to 0
        currentCharge = 0f;

        // Set shield charged state to false (broken) and play the particle effect for it
        shieldRecharged = false;
        shieldBreakParticles.Play();
    }

    /// Updates the shield image to it's current proper state ///
    void UpdateShieldImage()
    {
        // Determines the current fill of the graphic based on shieldRechargeTime
        float currentFill = currentCharge / shieldRechargeTime;
        float newFill = 0f; // Resets the new fill to 0

        // Determines the newFill of the graphic based on the currentValue (in 12ths)
        if (currentFill < 1 * (1f / 12f))
        {
            newFill = 1 * (1f / 12f);
        }
        else if (currentFill < 2 * (1f / 12f))
        {
            newFill = 2 * (1f / 12f);
        }
        else if (currentFill < 3 * (1f / 12f))
        {
            newFill = 3 * (1f / 12f);
        }
        else if (currentFill < 4 * (1f / 12f))
        {
            newFill = 4 * (1f / 12f);
        }
        else if (currentFill < 5 * (1f / 12f))
        {
            newFill = 5 * (1f / 12f);
        }
        else if (currentFill < 6 * (1f / 12f))
        {
            newFill = 6 * (1f / 12f);
        }
        else if (currentFill < 7 * (1f / 12f))
        {
            newFill = 7 * (1f / 12f);
        }
        else if (currentFill < 8 * (1f / 12f))
        {
            newFill = 8 * (1f / 12f);
        }
        else if (currentFill < 9 * (1f / 12f))
        {
            newFill = 9 * (1f / 12f);
        }
        else if (currentFill < 10 * (1f / 12f))
        {
            newFill = 10 * (1f / 12f);
        }
        else if (currentFill < 11 * (1f / 12f))
        {
            newFill = 11 * (1f / 12f);
        }
        else if (currentFill <= 1f)
        {
            newFill = 1f;
        }

        // Runs once when the shield reaches a full charge in order to play the animation
        if (shield.fillAmount != newFill)
        {
            animator.Play(shieldPulseName, -1, 0); // Plays the animation

            shieldRechargeSound.pitch = 1f;
            shieldRechargeSound.Play();
        }
        
        shield.fillAmount = newFill; // Sets the shield graphic's fill to the newFill ammount
    }
}
