using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f,100f)]
    [SerializeField] private float _fuelAmount; // The percentage of fuel the pickup will restore
    [SerializeField] private GameObject _pickupEffect; // The pickup effect prefab

    private Player player; // Refference to the Player script
    
    

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Collect();
        }
    }



    private void Collect()
    {
        // Set the player's new fuel amount
        //float newFuelAmount = player.currentFuel + player.fuelCapacity / (100f / _fuelPercentage);

        // Apply and clamp the new fuel amount to the player's fuel
        player.currentFuel = Mathf.Clamp(player.currentFuel + _fuelAmount, 0f, player.fuelCapacity);
        
        // Instantiate the pickup effect prefab
        Instantiate(_pickupEffect, transform.position, Quaternion.identity);
        
        // Destroy the game object
        Destroy(gameObject);
    }
}
