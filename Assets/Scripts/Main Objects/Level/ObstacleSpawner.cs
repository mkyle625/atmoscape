using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Obstacle
    {
        public GameObject prefab = null; // Obstacle prefab
        public float rotation = 0f; // The angle of rotation to spawn the obstacle at
        [Tooltip("Use torque impulse as a range \n[-torqueImpulse, torqueImpulse].")]
        public bool TIRange = false;
        public float torqueImpulse = 0f; // Torque applied to obstacle upon spawning
        [Tooltip("Use horizontal impulse as a range \n[-horizontalImpulse, horizontalImpulse].")]
        public bool HIRange = false;
        public float horizontalImpulse = 0f; // Force on the x-axis applied to obstacle upon spawning
        [Tooltip("Use vertical impulse as a range \n[-verticalImpulse, verticalImpulse].")]
        public bool VIRange = false;
        public float verticalImpulse = 0f; // Force on the y-axis applied to obstacle upon spawning
    }

    [System.Serializable]
    public class ObstaclePreset
    {
        [SerializeField] private string name = "";
        
        [Range(0,5)]
        public int difficulty = 1;
        
        [Header("Randomly Placed Obstacles")]
        [Tooltip("Uses available central spawpoints to randomly spawn the specified obstacle.")]
        public Obstacle[] randomCentralObstacles;
        [Tooltip("Uses available extra spawpoints to randomly spawn the specified obstacle.")]
        public Obstacle[] randomExtraObstacles;

        [Header("Predetermined Obstacles")]
        public Obstacle[] centralSpawnpointObstacles = new Obstacle[5]; // Main 5 spawnpoints in the center of the playspace
        public Obstacle[] extraSpawnpointObstacles = new Obstacle[6]; // Extra 6 spawnpoints in the center and sides of the playspace
        public Obstacle[] leftSpawnpointObstacles = new Obstacle[3]; // 3 spawnpoints to the left of the playspace
        public Obstacle[] rightSpawnpointObstacles = new Obstacle[3]; // 3 spawnpoints to the right of the playspace
    }

    [System.Serializable]
    public class ExtendedObstaclePreset
    {
        [SerializeField] private string name = "";
        
        [Header("Settings")] 
        [Range(0,5)]
        public int difficulty = 1;
        [Tooltip("Preset spawned before repeatedly spawning this preset.")]
        public ObstaclePreset masterPreset; // Preset spawned before repeatedly spawning this preset
        [Tooltip("The number of clone presets to spawn after the master preset.")]
        [Range(1, 20)]
        public int length = 1; // The number of obstacle presets to spawn after this preset
        [Tooltip("Preset that is going to be spawned repeatedly.")]
        public ObstaclePreset clonePreset; // Preset that is going to be spawned repeatedly
    }

    [Header("Presets")]
    public ObstaclePreset[] obstaclePresets; // Normal one row obstacle presets
    public ExtendedObstaclePreset[] extendedObstaclePresets; // Extended obstacle presets
    public ObstaclePreset[] fuelObstaclePresets; // Obstalce presets containing fuel
    public ObstaclePreset[] tutorialObstaclePresets;
    public GameObject tutorialCheckpoint;

    [Header("Settings")]
    [Range(0f,100f)]
    public float chanceForCredits;
    public GameObject creditPrefab;
    public float fuelDistance;
    public float maxSpawnDistance; // The largest distance between spawned presets
    public float minSpawnDistance; // The smallest distance between spawned presets
    public bool testMode = false;
    public bool tutorialMode = false;
    public bool tutorialObstacleCompleted = false;
    public int currentTutorialIndex = 0;

    [Header("Unity Setup")]
    public Transform[] extraSpawnpoints; // Extra 6 spawnpoints between the main 5
    public Transform[] centralSpawnpoints; // Main 5 spawnpoints within the side walls
    public Transform[] leftSpawnpoints; // Spawnpoints to the left of the playspace
    public Transform[] rightSpawnpoints; // Spawnpoints to the right of the playspace
    public Transform playerTransform; // The player's position

    public int currentDifficulty = 1;
    
    /* Private variables */
    private Vector2 lastSpawnPos;
    private Vector2 lastFuelPos;
    private bool readyToSpawn = false;
    private int numOfPresets = 0;
    private Vector2 lastTutorialObstaclePos;

    private int currentCloneIteration = 0; // Stores the current iteration of the long preset being spawned
    private ExtendedObstaclePreset currentExtendedPreset = null; // The current long preset to be spawned
    
    private List<ObstaclePreset> testObstaclePresets = new List<ObstaclePreset>();
    private List<ExtendedObstaclePreset> testExtendedObstaclePresets = new List<ExtendedObstaclePreset>();
    
    public void Start()
    {
        // Check for testing presets (DEV ONLY)
        for (int i = 0; i < obstaclePresets.Length; i++)
        {
            if (obstaclePresets[i].difficulty == 0)
            {
                testObstaclePresets.Add(obstaclePresets[i]);
            }
        }

        for (int i = 0; i < extendedObstaclePresets.Length; i++)
        {
            if (extendedObstaclePresets[i].difficulty == 0)
            {
                testExtendedObstaclePresets.Add(extendedObstaclePresets[i]);
            }
        }
        
        // Set last spawn position to current position
        lastSpawnPos = transform.position;

        // Stores the number of available presets, not including the fuel presets
        numOfPresets = obstaclePresets.Length + extendedObstaclePresets.Length;

        // Set last fuel position to current position plus fuelDistance on the y
        lastFuelPos = new Vector2(0, transform.position.y + fuelDistance);
    }

    /// Handles distance based spawning ///
    public void Update()
    {
        if (playerTransform != null)
        {
            if (playerTransform.position.y < 100)
            {
                currentDifficulty = 1;
            }
            else if (playerTransform.position.y < 250)
            {
                currentDifficulty = 2;
            }
            else if (playerTransform.position.y < 500)
            {
                currentDifficulty = 3;
            }
            else if (playerTransform.position.y < 1000)
            {
                currentDifficulty = 4;
            }
            else
            {
                currentDifficulty = 5;
            }
        }

        if (transform.position.y - lastSpawnPos.y >= maxSpawnDistance && readyToSpawn)
        {
            readyToSpawn = false;

            if (testMode)
            {
                SpawnTestPreset();
            }
            else if (!tutorialMode)
            {
                SpawnPreset();
            }
        }

        if (transform.position.y - lastSpawnPos.y >= minSpawnDistance && currentExtendedPreset != null)
        {
            if (currentCloneIteration < currentExtendedPreset.length)
            {
                // increment the index of the clone preset
                currentCloneIteration++;

                // Run through and spawn the preset
                BuildPreset(currentExtendedPreset.clonePreset);
            }
            else if (currentExtendedPreset != null)
            {
                // Set current preset to null to prevent spawning
                currentExtendedPreset = null;

                // Reset the current clone iteration to 0
                currentCloneIteration = 0;

                // Set readyToSpawn to true to allow next spawn
                readyToSpawn = true;
            }
        }
    }

    public void BeginSpawning()
    {
        if (testMode)
        {
            SpawnTestPreset();
        }
        else if (tutorialMode)
        {
            SpawnTutorialPreset();
        }
        else
        {
            SpawnPreset();
        }
    }
    
    /// Spawns a random obstacle preset ///
    public void SpawnPreset()
    {
        // Check if it is time to spawn more fuel
        if (transform.position.y - lastFuelPos.y >= fuelDistance)
        {
            // Choose the index of the obstacle preset to be spawned randomly
            int randPreset = Random.Range(0, fuelObstaclePresets.Length);

            // Run through and spawn the preset
            BuildPreset(fuelObstaclePresets[randPreset]);

            // Set the current position as the last fuel position
            lastFuelPos = transform.position;

            // Set readyToSpawn to true to allow next spawn
            readyToSpawn = true;
        }
        else
        {
            // The current preset's difficulty
            int presetDifficulty = 0;

            int randPreset = 0;
            char type = 'N'; // N for normal and E for extended

            while (presetDifficulty < 1 || presetDifficulty > currentDifficulty)
            {
                // Choose the index of the random type of preset to spawn
                randPreset = Random.Range(0, numOfPresets);
                
                if (randPreset < obstaclePresets.Length)
                {
                    // Store the presets difficulty under the preset difficulty variable
                    presetDifficulty = obstaclePresets[randPreset].difficulty;

                    type = 'N';
                }
                else
                {
                    // Fix the index to match the proper index in the extended obstacles array
                    randPreset -= obstaclePresets.Length;

                    // Store the presets difficulty under the preset difficulty variable
                    presetDifficulty = extendedObstaclePresets[randPreset].difficulty;

                    type = 'E';
                }
            }
            
            if (type == 'N')
            {
                // Run through and spawn the preset
                BuildPreset(obstaclePresets[randPreset]);

                // Set readyToSpawn to true to allow next spawn
                readyToSpawn = true;
            }
            else
            {
                // Run through and spawn the preset's initial preset
                BuildPreset(extendedObstaclePresets[randPreset].masterPreset);

                // Run through and spawn the clones for the preset as specified
                if (extendedObstaclePresets[randPreset].length > 0)
                {
                    currentExtendedPreset = extendedObstaclePresets[randPreset];
                }
            }
        }
    }

    /// Spawns a random obstacle test preset ///
    public void SpawnTestPreset()
    {
        // Check if it is time to spawn more fuel
        if (transform.position.y - lastFuelPos.y >= fuelDistance)
        {
            // Choose the index of the obstacle preset to be spawned randomly
            int randPreset = Random.Range(0, fuelObstaclePresets.Length);

            // Run through and spawn the preset
            BuildPreset(fuelObstaclePresets[randPreset]);

            // Set the current position as the last fuel position
            lastFuelPos = transform.position;

            // Set readyToSpawn to true to allow next spawn
            readyToSpawn = true;
        }
        else
        {
            int randPreset = Random.Range(0, 2);

            if (randPreset == 1 && testObstaclePresets.Count > 0 || testExtendedObstaclePresets.Count < 1)
            {
                randPreset = Random.Range(0, testObstaclePresets.Count);
                
                // Run through and spawn the preset
                BuildPreset(testObstaclePresets[randPreset]);

                // Set readyToSpawn to true to allow next spawn
                readyToSpawn = true;
            }
            else if (testExtendedObstaclePresets.Count > 0)
            {
                randPreset = Random.Range(0, testExtendedObstaclePresets.Count);
                
                // Run through and spawn the preset's initial preset
                BuildPreset(testExtendedObstaclePresets[randPreset].masterPreset);

                // Run through and spawn the clones for the preset as specified
                if (testExtendedObstaclePresets[randPreset].length > 0)
                {
                    currentExtendedPreset = testExtendedObstaclePresets[randPreset];
                }
            }
        }
    }
    
    /// Spawns a random obstacle preset ///
    public void SpawnTutorialPreset()
    {
        // Run through and spawn the preset
        if (currentTutorialIndex < tutorialObstaclePresets.Length)
        {
            BuildPreset(tutorialObstaclePresets[currentTutorialIndex]);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        Instantiate(tutorialCheckpoint, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);

        if (tutorialObstacleCompleted)
        {
            tutorialObstacleCompleted = false;
        }
        
        // Set readyToSpawn to true to allow next spawn
        readyToSpawn = true;
    }
    
    /// Builds the obstacle preset and spawns obstacles ///
    public void BuildPreset(ObstaclePreset preset)
    {
        // Create a list of open central spawnpoints that will store empty spawnpoints
        List<Transform> openCentralSpawnpoints = new List<Transform>();
        // Create a list of open extra spawnpoints that will store empty spawnpoints
        List<Transform> openExtraSpawnpoints = new List<Transform>();

        // Run through the preset's central spawnpoints and spawn accordingly
        for (int i = 0; i < centralSpawnpoints.Length; i++)
        {
            // If there is not an obstacle to be spawned then add transform to openCentralSpawnpoints
            if (preset.centralSpawnpointObstacles[i].prefab == null)
                openCentralSpawnpoints.Add(centralSpawnpoints[i]);
            else
            {
                // Spawn the obstacle using the SpawnObstacle function, passing in the current preset and spawnpoint
                SpawnObstacle(preset.centralSpawnpointObstacles[i], centralSpawnpoints[i]);
            }
        }
        // Run through the preset's extra spawnpoints and spawn accordingly
        for (int i = 0; i < extraSpawnpoints.Length; i++)
        {
            // If there is not an obstacle to be spawned then add transform to openExtraSpawnpoints
            if (preset.extraSpawnpointObstacles[i].prefab == null)
                openExtraSpawnpoints.Add(extraSpawnpoints[i]);
            else
            {
                // Spawn the obstacle using the SpawnObstacle function, passing in the current preset and spawnpoint
                SpawnObstacle(preset.extraSpawnpointObstacles[i], extraSpawnpoints[i]);
            }
        }
        // Run through the preset's left spawnpoints and spawn accordingly
        for (int i = 0; i < leftSpawnpoints.Length; i++)
        {
            // If there is an obstacle to be spawned then spawn it
            if (preset.leftSpawnpointObstacles[i].prefab != null)
            {
                // Spawn the obstacle using the SpawnObstacle function, passing in the current preset and spawnpoint
                SpawnObstacle(preset.leftSpawnpointObstacles[i], leftSpawnpoints[i]);
            }
        }
        // Run through the preset's right spawnpoints and spawn accordingly
        for (int i = 0; i < rightSpawnpoints.Length; i++)
        {
            // If there is an obstacle to be spawned then spawn it
            if (preset.rightSpawnpointObstacles[i].prefab != null)
            {
                // Spawn the obstacle using the SpawnObstacle function, passing in the current preset and spawnpoint
                SpawnObstacle(preset.rightSpawnpointObstacles[i], rightSpawnpoints[i]);
            }
        }

        // Check if there are obstacles to be randomly spawned using central spawnpoints
        if (preset.randomCentralObstacles.Length > 0)
        {
            // If so, then run through them and spawn them using openCentralSpawnpoints
            for (int i = 0; i < preset.randomCentralObstacles.Length; i++)
            {
                // Randomly select an open central spawnpoint to be used
                int randSpawnpoint = Random.Range(0, openCentralSpawnpoints.Count);

                // Pass in the obstacle to spawn as well as the random central spawnpoint selected
                SpawnObstacle(preset.randomCentralObstacles[i], openCentralSpawnpoints[randSpawnpoint]);

                // Remove the now used spawnpoint from the openCentralSpawnpoints list
                openCentralSpawnpoints.Remove(openCentralSpawnpoints[randSpawnpoint]);
            }
        }
        // Check if there are obstacles to be randomly spawned using extra spawnpoints
        if (preset.randomExtraObstacles.Length > 0)
        {
            // If so, then run through them and spawn them using openExtraSpawnpoints
            for (int i = 0; i < preset.randomExtraObstacles.Length; i++)
            {
                // Randomly select an open extra spawnpoint to be used
                int randSpawnpoint = Random.Range(0, openExtraSpawnpoints.Count);

                // Pass in the obstacle to spawn as well as the random extra spawnpoint selected
                SpawnObstacle(preset.randomExtraObstacles[i], openExtraSpawnpoints[randSpawnpoint]);

                // Remove the now used spawnpoint from the openExtraSpawnpoints list
                openExtraSpawnpoints.Remove(openExtraSpawnpoints[randSpawnpoint]);
            }
        }

        // Use any free spawnpoints to spawn credits based on the set chance
        if (openCentralSpawnpoints.Count > 0 && !tutorialMode)
        {
            // Run through spawnpoints and spawn credits using chanceForCredits
            for (int i = 0; i < openCentralSpawnpoints.Count; i++)
            {
                if (Random.Range(0f,100f) < chanceForCredits)
                {
                    SpawnPickup(creditPrefab, openCentralSpawnpoints[i]);
                }
            }
        }

        // Set last spawn position to current position
        lastSpawnPos = transform.position;
    }

    /// Spawns the new obstacle and applies forces based on the provided preset ///
    // Takes in object of type Obstacle that should be spawned, and spawnpoint transform to be used //
    public void SpawnObstacle(Obstacle obstacle, Transform spawnpoint)
    {
        // Set the angle to spawn the obstacle at
        Quaternion spawnRotation = Quaternion.Euler(0f, 0f, obstacle.rotation);

        // Instantiate the obstacle at the provided spawnpoint transform and store its rigidbody in a variable
        Rigidbody2D obstacleRb = Instantiate(obstacle.prefab, spawnpoint.position, spawnRotation).GetComponent<Rigidbody2D>() as Rigidbody2D;

        // Check if the obstacle spawned had a rigibody
        if (obstacleRb != null)
        {
            // Check if using range for horizontal impulse or not
            if (obstacle.HIRange)
            {
                // Randomize the horizontal force using the provided by the obstacle
                float horizontalForce = Random.Range(-obstacle.horizontalImpulse, obstacle.horizontalImpulse);

                // Apply the randomized horizontal force to the obstacle
                obstacleRb.AddForce(Vector2.right * horizontalForce, ForceMode2D.Impulse);
            }
            else
            {
                // Apply the provided horizontal impulse to the obstacle
                obstacleRb.AddForce(Vector2.right * obstacle.horizontalImpulse, ForceMode2D.Impulse);
            }

            // Check if using range for vertical impulse or not
            if (obstacle.VIRange)
            {
                // Randomize the vertical force using the provided by the obstacle
                float verticalForce = Random.Range(-obstacle.verticalImpulse, obstacle.verticalImpulse);

                // Apply the randomized vertical force to the obstacle
                obstacleRb.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
            }
            else
            {
                // Apply the provided vertical impulse to the obstacle
                obstacleRb.AddForce(Vector2.up * obstacle.verticalImpulse, ForceMode2D.Impulse);
            }

            // Check if using range for torque impulse or not
            if (obstacle.TIRange)
            {
                // Randomize the torque force using the provided by the obstacle
                float torqueForce = Random.Range(-obstacle.torqueImpulse, obstacle.torqueImpulse);

                // Apply the randomized torque force to the obstacle
                obstacleRb.AddTorque(torqueForce, ForceMode2D.Impulse);
            }
            else
            {
                // Apply the provided torque impulse to the obstacle
                obstacleRb.AddTorque(obstacle.torqueImpulse, ForceMode2D.Impulse);
            }
        }
    }

    /// Spawns pickups ///
    // Takes in the pickup to be used and spawnpoint transform to be used //
    public void SpawnPickup(GameObject pickup, Transform spawnpoint)
    {
        // Instantiate the pickup at the provided spawnpoint transform
        Instantiate(pickup, spawnpoint.position, Quaternion.identity);
    }
}
