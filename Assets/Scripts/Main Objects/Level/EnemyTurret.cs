using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Audio;

public class EnemyTurret : MonoBehaviour
{
    [Header("Turret Settings")] 
    [SerializeField] private float _fireRate = 1f; // The number of shots to be fired every second
    [SerializeField] private float _range = 8f; // The range (in unity units) the turret can begin to fire at
    [SerializeField] private float _yOffset = 3f; // How many units above the player the turret will aim
    
    [Header("Unity Setup")] 
    [SerializeField] private Transform _turretPivot; // The turret's pivot transform
    [SerializeField] private Transform[] _firePoints; // The turret's fire point transforms
    [SerializeField] private GameObject _turretProjectile; // The turret's projectile prefab
    [SerializeField] private GameObject _shootEffect; // The effect instantiated at the fire point when shooting
    [SerializeField] private GameObject _parentAsteroid; // The asteroid the turret sits on
    [SerializeField] private AudioSource _fireSound; // The sound when the turret fires
    
    private Transform _playerTransform; // The current Transform of the player
    private Player _player; // The player's Player component
    private float _fireDelay; // The current time to wait until the next shot

    
    
    private void Start()
    {
        // Find the game object with the "Player" tag and get it's transform
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Find the player component from the player transform
        _player = FindObjectOfType<Player>();
    }

    
    
     /*********************************************************************
     * Resets the _fireDelay to the new time before the next shot is to be
     * fired, and instantiate a new projectile.
     *********************************************************************/
    private void Fire()
    {
        // Play the fire sound effect
        _fireSound.Play();
        
        // Set fire delay to 1f divided by the _fireRate
        _fireDelay = 1f / _fireRate;
        
        // Instantiate _shootEffect at the _firePoint[0] position and rotation
        Instantiate(_shootEffect, _firePoints[0].position, _firePoints[0].rotation);
        
        // Cycle through all turret firepoints
        for (int i = 0; i < _firePoints.Length; i++)
        {
            // Instantiate _turretProjectile at the _firePoint position and rotation, cast it as type TurretProjectile
            TurretProjectile projectile = Instantiate(_turretProjectile, _firePoints[i].position, _firePoints[i].rotation).GetComponent<TurretProjectile>();
            
            // Set the parent object of the projectile equal to this obstacle
            projectile.ParentObject = _parentAsteroid;
        }
    }

    
    
    /***********************************************************************
     * Used to update the rotation of the turret to face the player,
     * as well as checking if the player is in range and the turret is
     * ready to fire before running the Fire() method.
     **********************************************************************/
    private void Update()
    {
        // Check if _playerTransform was found
        if (!_player.isDead)
        {
            /* Set the _turretPivot's transform.up vector to the different between the player's position vector
             with it's applied yOffset, and the turret pivot position vector */
            _turretPivot.up = ((_playerTransform.position + new Vector3(0,_yOffset,0)) - _turretPivot.position);
            
            // Check if the distance from the player to the turret is within turret range and if turret is ready to fire
            if ((_playerTransform.position - _turretPivot.position).magnitude <= _range && _fireDelay <= 0)
            {
                Fire();
            }
        }

        
        
        // Check if the _fireDelay is greater than 0
        if (_fireDelay > 0)
        {
            // Decrease _fireDelay over time by 1f every second until it reaches 0
            _fireDelay -= 1f * Time.deltaTime;
        }
    }
}
