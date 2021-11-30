using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float _impulse = 4f; // Impulse force applied to the projectile at start
    [SerializeField] private float _lifetime = 5f; // Lifetime of the projectile
    
    [Header("Unity Setup")]
    [SerializeField] private Rigidbody2D _rigidbody; // The projectile's rigidbody
    [SerializeField] private GameObject _destroyEffect; // The effect object to instantiate when destroying the bullet

    public GameObject ParentObject; // The parent ofject to the bullet

    private void Start()
    {
        // Apply impulse force to projectile using it's transform.up vector (the way it faces) upon spawning in
        _rigidbody.AddForce(transform.up * _impulse, ForceMode2D.Impulse);
        
        // Start the Lifetime coroutine which waits lifetime before killing the projectile
        StartCoroutine(Lifetime());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object the bullet hit contains the tag "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Get the player component from the scene
            Player player = FindObjectOfType<Player>();

            if (!player.invulnerable)
            {
                // run the TakeDamage() function
                player.TakeDamage(null, null);
            }

            // Kill the projectile
            KillProjectile();
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            if (other.gameObject != ParentObject)
            {
                // Kill the projectile
                KillProjectile();
            }
        }
    }

    /*********************************************************************
     * Waits for lifetime seconds and runs the KillProjectile() function
     ********************************************************************/
    private IEnumerator Lifetime()
    {
        // Wait for set _lifetime
        yield return new WaitForSeconds(_lifetime);
        
        // Kill the projectile
        KillProjectile();
    }

    /*********************************************************************
     * Instantiates destroy effect and destroys the game object
     ********************************************************************/
    private void KillProjectile()
    {
        // Instantiate the _destroyEffect object when the game object is destroyed
        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        
        // Destroy the game object
        Destroy(gameObject);
    }
}
