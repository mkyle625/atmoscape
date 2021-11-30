using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    [Header("Player Attributes")]
    public int health; // Player health
    public int score; // The current score to display
    public float speed; // Player speed
    public float maxVelocity; // Max rocket velocity
    public float minVelocity; // The slowest the player can move before dying
    public float boostVelocity; // Max velocity while boosting
    public float turningSpeed; // Represents force applied to move left or right
    public float fuelConsumption; // The ammount of fuel used per second
    public float fuelCapacity = 100f; // The fuel capacity of the player
    public float currentFuel; // Stores the current fuel on the player

    [Header("Functionality")]
    public Rigidbody2D rb; // Player's rigidbody
    public Shield playerShield; // The player shield script object
    public GameObject watchAdScreen;
    public TimeManager timeManager;
    public GameObject[] playerColliders;

    [Header("Cosmetics")]
    public ParticleSystem deathEffect; // Rocket death effect Unity Setup References   

    public ParticleSystem engineParticles; // Rocket engine particle effects for when not boosting or turning
    public ParticleSystem turningParticles; // Rocket turning particle effecta when boosting left or right
    public ParticleSystem boosterParticles; // Rocket booster effect when using boost ability
    public ParticleSystem starterBoostParticles;
    public GameObject stopStarterBoostEffect;

    public GameObject obstacleCollisionFlash; // The flash when the player breaks an obstacle

    public Animator animator; // The player's animator component

    public Animator mainCameraAnimator; // The MainCamera's animator component

    public AudioSource engineSound;
    public AudioSource boostSound;
    public AudioSource startBoosSound;

    [Header("Other Variables")]
    public bool boosting = false; // Determines if the player is boosting or not
    public bool stunned = false; // Determines whether the player's movement function will run or not
    public bool inPlayspace = true; // Stores whether the player is outside the playspace
    public bool stalled = false; // Stores whether the player has run out of fuel and stalled or not
    public bool tutorialMode = false;
    public bool isDead = false;
    public bool invulnerable = false;

    [Header("UI References")] 
    [SerializeField] private GameObject _pauseButton; // The pause button

    /* Private Variables */
    private bool inFlight = false; // Determines whether the player is currently flying or not
    private float thrustMult = 0f; // Determines how much thrust force will be used to move the ship
    private float inputTimespace = 0f; // Stores the time since the previous input
    private bool doubleInputAvailable = true; // Determines whether a second input can be accepted
    private bool launched; // Determines whether the player rocket has completed launch

    public void Start()
    {
        currentFuel = fuelCapacity; // Set the current fuel to the fuel capacity at start
    }

    public void BeginFlight()
    {
        inFlight = true;
        playerShield.StartShield();
    }

    public void Update()
    {
        //Update the score
        score = (int)transform.position.y;

        // Handle player fuel
        if (inFlight && currentFuel > 0 && !isDead)
        {
            // Set stalled to false to allow player movement
            stalled = false;

            // Set the rb drag to normal
            rb.drag = 0;

            if (!invulnerable)
            {
                if (boosting)
                    currentFuel -= 1f * fuelConsumption * thrustMult * 10f * Time.deltaTime;
                else
                    currentFuel -= 1f * fuelConsumption * thrustMult * Time.deltaTime;
            }
        }
        else if (inFlight && !isDead)
        {
            if (!stalled)
                Stall();
        }
        
        // Check if the player has run out of fuel and stopped moving
        if (stalled && rb.velocity.magnitude <= 0.1f)
        {
            if (!watchAdScreen.activeSelf && !tutorialMode)
            {
                // Enable the run out of fuel UI
                watchAdScreen.SetActive(true);
            }
        }

        if (inFlight || !stalled)
        {
            // Increase thrust multiplier to 1f over time
            if (thrustMult < 1f)
            {
                thrustMult += .25f * Time.deltaTime;
            }
            else
            {
                thrustMult = 1f;
            }

            // Increase inputTimespace to .2f over time
            if (inputTimespace < .05f)
            {
                inputTimespace += 1f * Time.deltaTime;
            }
        }
    }

    public void FixedUpdate()
    {
        if (isDead || stalled)
        {
            if (engineSound.isPlaying)
            {
                engineSound.Stop();
            }
            if (boostSound.isPlaying)
            {
                boostSound.Stop();
            }
        }
        else if (!engineSound.isPlaying && inFlight)
        {
            engineSound.Play();
        }
        
        if (isDead && boostSound.isPlaying)
        {
            boostSound.Stop();
        }

        if (!inFlight)
        {
            return;
        }

        /// Player Movement Functionality

        // Rotate player based on current velocity vector
        if (rb.velocity.magnitude > .5f && !boosting)
        {
            if (!invulnerable)
                transform.up = rb.velocity.normalized;

            if (!launched)
                launched = true;
        }

        if (stalled)
            return;

        // Apply forward force to the player
        rb.AddForce(Vector2.up * speed * thrustMult, ForceMode2D.Force);

        // Control player movement
        if (!isDead && !invulnerable) 
            Movement();

        // Limit velocity based on current max
        if (boosting)
        {
            if (rb.velocity.magnitude > boostVelocity)
            {
                Vector3 newVelocity = rb.velocity.normalized;
                newVelocity *= boostVelocity;
                rb.velocity = newVelocity;
            }
        }
        else
        {
            if (rb.velocity.magnitude > maxVelocity)
            {
                Vector3 newVelocity = rb.velocity.normalized;
                newVelocity *= maxVelocity;
                rb.velocity = newVelocity;
            }
        }

        // Check if the player has come to a halt and kill
        if (launched && rb.velocity.y < 0f)
            rb.AddForce(new Vector2(0f,5f), ForceMode2D.Impulse);

    }

    /// Handle player movement and controls ///
    void Movement()
    {
        // Apply input based on number of touches
        if (Input.touchCount > 0) // Check if touch input exists
        {
            if (!doubleInputAvailable)
            {
                doubleInputAvailable = true; // Allow double input
                inputTimespace = 0f; // Set time between last input to 0
            }

            if (doubleInputAvailable && inputTimespace < .075f && Input.touchCount == 2 && inPlayspace) // If 2 fingers are on screen
            {
                inputTimespace = 0; // Set the time since last input to 0 while input is being received

                Touch firstTouch = Input.GetTouch(0); // Save the first touch index = touch 0
                Touch secondTouch = Input.GetTouch(1); // Save the second touch index = touch 1

                if (firstTouch.position.x > (Screen.width / 2) && secondTouch.position.x < (Screen.width / 2) ||
                    firstTouch.position.x < (Screen.width / 2) && secondTouch.position.x > (Screen.width / 2))
                {
                    // Set player boosting to true and unfreeze rigidbody rotation
                    boosting = true;
                    rb.freezeRotation = true;

                    // Play boosting effect
                    PlayMovementEffect(boosterParticles);

                    // Apply impulse force to player in order to reach boost velocity
                    rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
                }
                else
                {
                    // Set player boosting to false and unfreeze rigidbody rotation
                    rb.freezeRotation = false;
                    boosting = false;
                }
            }
            else
            {
                rb.freezeRotation = false;
                boosting = false; // Set player boosting to false

                Touch firstTouch = Input.GetTouch(0); // Save the first touch index = touch

                if (firstTouch.position.x > (Screen.width / 2)) // Check which side of the screen touch input is on
                    rb.AddForce(Vector2.right * turningSpeed * thrustMult, ForceMode2D.Force); // Apply force to the right
                else
                    rb.AddForce(Vector2.right * -turningSpeed * thrustMult, ForceMode2D.Force); // Apply force to the left

                // Play turning effect
                PlayMovementEffect(turningParticles);
            }
        }
        else
        {
            // Disalow double input
            doubleInputAvailable = false;

            // Set player boosting to false and unfreeze rigidbody rotation
            boosting = false;
            rb.freezeRotation = false;

            // Dampen rocket left and right movement
            if (rb.velocity.x > 0) // If heading right too fast
                rb.AddForce(Vector2.right * (-turningSpeed / 4) * thrustMult, ForceMode2D.Force); // Apply force to the left
            else if (rb.velocity.x < 0) // If heading left too fast
                rb.AddForce(Vector2.right * (turningSpeed / 4) * thrustMult, ForceMode2D.Force); // Apply force to the right

            // Play engine effect
            PlayMovementEffect(engineParticles);
        }
    }

    /// Prevent Movement function from running while the player is stunned ///
    public IEnumerator Stun()
    {
        animator.Play("Stunned", -1, 0);

        stunned = true; // Set stunned to true

        // Set player boosting to false and unfreeze rigifbody rotation
        boosting = false;
        rb.freezeRotation = false;

        // Wait 1 second before setting stunned back to false
        yield return new WaitForSecondsRealtime(1f);
        stunned = false;
        inPlayspace = true;
    }

    /// Toogles particle systems on or off based on ParticleSystem passed ///
    public void PlayMovementEffect(ParticleSystem system)
    {
        if (system == engineParticles && !engineParticles.isPlaying)
        {
            engineParticles.Play();
        }
        else if (system != engineParticles && engineParticles.isPlaying)
        {
            engineParticles.Stop();
        }

        if (system == turningParticles && !turningParticles.isPlaying)
        {
            turningParticles.Play();
        }
        else if (system != turningParticles && turningParticles.isPlaying)
        {
            turningParticles.Stop();
        }

        if (system == boosterParticles && !boosterParticles.isPlaying)
        {
            boosterParticles.Play();
            boostSound.Play();
        }
        else if (system != boosterParticles && boosterParticles.isPlaying)
        {
            boosterParticles.Stop();
            boostSound.Stop();
        }
    }

    public void Stall()
    {
        SoundManager.Instance.ToggleMusic(true,false);
        
        // Set stalled to true to prevent player movement
        stalled = true;

        // Set thrustMult to 0 in order to allow thrust to build up over time (like when the player first begins flight)
        thrustMult = 0f;

        // Set the rb drag to normal
        rb.drag = .8f;

        // Stop the particle booster effects
        engineParticles.Stop();
        turningParticles.Stop();
        boosterParticles.Stop();
        engineSound.Stop();
        
        // Disable Rigidbody2D constraints
        rb.constraints = RigidbodyConstraints2D.None;
        
        //Save the last score
        SaveManager.Instance.state.lastScore = score;
    }

    public void Refuel()
    {
        SoundManager.Instance.ToggleMusic(false,false);
        
        // Give the player fuel again
        currentFuel = fuelCapacity / 2f;

        // Have the death canvas disabled
        watchAdScreen.SetActive(false);
        
        // Find all obstacles in the game and store them in the obstacles array
        ObstaclePrefab[] obstacles = FindObjectsOfType<ObstaclePrefab>();
        
        // Destroy all obstacles on screen
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].destructable = true;
            obstacles[i].Explode();
        }

        // Run Respawn in case the player has died to a bullet
        Respawn();
    }

    /// Kills the player ///
    public void Die(Collision2D collisionInfo)
    {
        SoundManager.Instance.ToggleMusic(true,true);
            
        engineSound.Stop();
            
        Social.localUser.Authenticate((bool success1) =>
        {
            if (success1)
            {
                Social.ReportScore(score, "CgkIz4GOyIwQEAIQAg", (bool success2) => {
                    // handle success or failure
                });   
            }
        });
            
        SaveManager.Instance.state.lastScore = score;
        if (score > SaveManager.Instance.state.highScore)
        {
            SaveManager.Instance.state.highScore = score;
            SaveManager.Instance.Save("Hypercharge");
        }
            
        timeManager.DoSlowmotion(true);

        // Disable the pause button
        _pauseButton.SetActive(false);

        // Disable the player's shield game object
        playerShield.gameObject.SetActive(false);
        
        // Stop the particle booster effects
        engineParticles.Stop();
        turningParticles.Stop();
        boosterParticles.Stop();
        
        // Disable and freeze the player in place
        for (int i = 0; i < playerColliders.Length; i++)
        {
            playerColliders[i].SetActive(false);
        }
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        
        // Set is dead to true
        isDead = true;
        
        // Check if collision information was passed in
        if (collisionInfo != null)
        {
            // Instantiate death effect at point of collision
            Instantiate(deathEffect, collisionInfo.contacts[0].point, Quaternion.identity);
        }
        else
        {
            // Instantiate death effect at current position
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }

    IEnumerator TutorialDeath()
    {
        yield return new WaitForSecondsRealtime(2f);
        Respawn();
    }
    
    /// Respawns the player ///
    private void Respawn()
    {
        // Re-enable the player
        for (int i = 0; i < playerColliders.Length; i++)
        {
            playerColliders[i].SetActive(true);
        }
        rb.constraints = RigidbodyConstraints2D.None;

        Stun();
        
        // Enable the pause button
        _pauseButton.SetActive(true);
        
        // Enables the player's shield game object
        playerShield.gameObject.SetActive(true);
        
        // Set is dead to false
        isDead = false;
    }

    public IEnumerator StarterBoost(int distance)
    {
        engineSound.Play();
        startBoosSound.Play();
        
        gameObject.GetComponent<Collider2D>().enabled = true;
        
        float originalVelocity = maxVelocity;
        maxVelocity *= 8f;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        invulnerable = true;
        rb.transform.up = Vector3.up;

        starterBoostParticles.Play();
        
        while (transform.position.y < distance)
        {
            yield return new WaitForEndOfFrame();
            rb.AddForce(Vector2.up * maxVelocity,ForceMode2D.Force);
        }

        EndBoost(originalVelocity);
    }

    public void EndBoost(float originalVelocity)
    {
        engineSound.Stop();
        
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        maxVelocity = originalVelocity;
        invulnerable = false;
        rb.constraints = RigidbodyConstraints2D.None;
        
        starterBoostParticles.Stop();
        
        // Find all obstacles in the game and store them in the obstacles array
        ObstaclePrefab[] obstacles = FindObjectsOfType<ObstaclePrefab>();
        
        // Destroy all obstacles on screen
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].destructable = true;
            obstacles[i].Explode();
        }

        Instantiate(stopStarterBoostEffect, transform.position, Quaternion.identity);
    }
    
    /// Handles crashes received from the player tip collider and normal collider (decides what to do with the player based on crash info) ///
    public void Crash(Collision2D collisionInfo, ObstaclePrefab obstacle, bool sharpCollision)
    {
        if (invulnerable)
        {
            if (obstacle != null)
            {
                Instantiate(obstacleCollisionFlash, collisionInfo.contacts[0].point, Quaternion.identity);
                obstacle.destructable = true;
                obstacle.Explode();
            }
        }
        else if (stunned || stalled)
        {
            if (obstacle.type == "yellow" && boosting)
            {
                Instantiate(obstacleCollisionFlash, collisionInfo.contacts[0].point, Quaternion.identity);
                obstacle.Explode();
            }
        }
        else if (!stunned && !stalled)
        {
            if (obstacle.type == "yellow" && boosting)
            {
                Instantiate(obstacleCollisionFlash, collisionInfo.contacts[0].point, Quaternion.identity);
                obstacle.Explode();
            }
            else if (obstacle.type == "red" || obstacle.type == "orange" || obstacle.type == "yellow")
            {
                Instantiate(obstacleCollisionFlash, collisionInfo.contacts[0].point, Quaternion.identity);
                TakeDamage(collisionInfo, obstacle);
            }
            else if (sharpCollision && obstacle.type != "green")
            {
                TakeDamage(collisionInfo, null);
            }
        }
    }

    /// Deals damage to the player or shield based on info passed in from Crash() ///
    public void TakeDamage(Collision2D collisionInfo, ObstaclePrefab obstacle)
    {
        if (!playerShield.shieldRecharged)
        {
            Die(collisionInfo);
        }
        else
        {
            if (obstacle != null)
                obstacle.Explode(); // Destroy the obstacle since the shield was broken

            // Stun the player
            StartCoroutine(Stun());
            
            // Breaks the shield
            playerShield.BreakShield();
            
            // Plays the camera shake animation on the Main Camera
            mainCameraAnimator.Play("Camera Shake",-1,0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (invulnerable)
        {
            ObstaclePrefab obstacle = other.GetComponent<ObstaclePrefab>();
            CreditPickup creditPickup = other.GetComponent<CreditPickup>();
            
            if (obstacle != null)
            {
                obstacle.destructable = true;
                obstacle.Explode();
            }

            if (creditPickup != null)
            {
                Destroy(creditPickup.gameObject);
            }
        }
    }
}
