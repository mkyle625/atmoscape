using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FX : MonoBehaviour
{
    [Header("Settings")]
    public float lifetime = 1f;
    public bool shakeCamera = false;
    [SerializeField] private float _shakeRange = 10f;

    public void Start()
    {
        Transform playerTransform = null;
        
        // Check if the Player is null, and store the player's transform in playerTransform variable
        if (FindObjectOfType<Player>() != null)
            playerTransform = FindObjectOfType<Player>().gameObject.transform;

        // Check if the player transform exists
        if (playerTransform != null)
        {
            // Check if camera shake is to be used and if the player is within the _shakeRange
            if (shakeCamera && (playerTransform.position - transform.position).magnitude <= _shakeRange)
            {
                // Get the object with the MainCamera tag, find it's animator component, and play the camera shake animation
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().Play("Camera Shake",-1,0);
            }
        }
        
        
        // Destroys the game object after the set lifetime
        Destroy(gameObject, lifetime);
    }
}
