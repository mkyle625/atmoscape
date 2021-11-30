using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;

public class TimeManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _slowdownFactor = 0.25f; // The new timescale when slowdown is activated
    [SerializeField] private float _slowdownLength = 4f; // The length of time in seconds before time scale returns to normal
    [SerializeField] private AudioMixer _masterAudio; // The master audio mixer in the game

    [Header("Unity Setup")] 
    [SerializeField] private GameObject _deathScreen; // Reference to the death screen UI
    
    private float _pitchValue = 1f; // The current pitch of the master audio mixer
    public bool _inSlowdown = false; // Stores whether the slow down effect is currently active
    private bool _playerIsDead = false; // Stores whether the player is dead or not

    /*********************************************************************
     * Handles updating the time scale every frame to a value closer to
     * 1f (Normal time), maintains normal physics updates during slow
     * motion, and changes audio pitch based on current time scale
     ********************************************************************/
    private void Update()
    {
        // Check if the game is currently using the slow down and if time scale is less than 1f
        if (_inSlowdown && Time.timeScale < 1f)
        {
            // Slowly increases time scale based on unscaled time
            Time.timeScale += (1f / _slowdownLength) * Time.unscaledDeltaTime;
        
            // Clamps the new time scale value between 0f and 1f
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        
            /* Scales fixed delta time to stay the same as time scale changes,
            maintaining 50 physics updates every realtime second */
            Time.fixedDeltaTime = Time.timeScale * .02f;
        
            // Gets the current master pitch value and stores it in _pitchValue
            _masterAudio.GetFloat("MasterPitch", out _pitchValue);
        
            // Applies pitch to master mixer
            _masterAudio.SetFloat("MasterPitch", Time.timeScale);
        }
        else
        {
            // Check if _inSlowdown is set to true and change it to false
            if (_inSlowdown)
                _inSlowdown = false;

            // Check if the player is dead
            if (_playerIsDead)
            {
                // Display the death screen UI
                DisplayDeathScreen();
            }
        }
    }

    private void DisplayDeathScreen()
    {
        // Check if the death screen UI is already enabled
        if (!_deathScreen.activeSelf)
        {
            // Enable the death screen UI
            _deathScreen.SetActive(true);
        }
    }

    /*********************************************************************
     * Sets the time scale to the slowdown factor in order to begin enter
     * motion.
     *
     * - Run from the Player() script when player dies
     ********************************************************************/
    public void DoSlowmotion(bool playerDeath)
    {
        // Set time scale = slowdown factor (ex: _slowdownFactor = .25f, so time is 4 times slower)
        Time.timeScale = _slowdownFactor;
        
        // Sets _inSlowdown to true in order to prevent time from being altered when paused
        _inSlowdown = true;

        // Check if playerDeath is set to true
        if (playerDeath)
        {
            // Set _playerIsDead to true
            _playerIsDead = true;
        }
    }
}
