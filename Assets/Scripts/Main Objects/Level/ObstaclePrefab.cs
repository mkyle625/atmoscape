using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstaclePrefab : MonoBehaviour
{
    [Header("Attributes")]
    public bool randomizeType = false;
    public string type = "white";
    public bool randomizeSprite = false;
    public bool destructable = false;

    [Header("Applied Forces")]
    [Tooltip("Use torque impulse as a range \n[-torqueImpulse, torqueImpulse].")]
    public bool TIRange = false;
    public float torqueImpulse = 0f; // Torque applied to obstacle upon spawning
    [Tooltip("Use horizontal impulse as a range \n[-horizontalImpulse, horizontalImpulse].")]
    public bool HIRange = false;
    public float horizontalImpulse = 0f; // Force on the x-axis applied to obstacle upon spawning
    [Tooltip("Use vertical impulse as a range \n[-verticalImpulse, verticalImpulse].")]
    public bool VIRange = false;
    public float verticalImpulse = 0f; // Force on the y-axis applied to obstacle upon spawning

    [Header("Colors")]
    public Color whiteColor;
    public Color redColor;
    public Color orangeColor;
    public Color yellowColor;
    public Color greenColor;

    [Header("GFX & Colliders")]
    public Sprite[] obstacleSprites;
    public GameObject explodeEffect;

    [Header("Unity Setup")]
    public SpriteRenderer spriteRend;
    public Rigidbody2D rb;

    public void Start()
    {
        SetObstacle();
        gameObject.AddComponent<PolygonCollider2D>();
        ApplyForces();
    }

    /// Applies impulse forces at the instantiation point ///
    public void ApplyForces()
    {
        // Check if using range for horizontal impulse or not
        if (HIRange)
        {
            // Convert impulse based on mass
            horizontalImpulse = horizontalImpulse * rb.mass / 5f;

            // Randomize the horizontal force using the provided by the obstacle
            float horizontalForce = Random.Range(-horizontalImpulse, horizontalImpulse);

            // Apply the randomized horizontal force to the obstacle
            rb.AddForce(Vector2.right * horizontalForce, ForceMode2D.Impulse);
        }
        else
        {
            // Apply the provided horizontal impulse to the obstacle
            rb.AddForce(Vector2.right * horizontalImpulse, ForceMode2D.Impulse);
        }

        // Check if using range for torque impulse or not
        if (TIRange)
        {
            // Convert impulse based on mass
            torqueImpulse = torqueImpulse * rb.mass / 5f;

            // Randomize the torque force using the provided by the obstacle
            float torqueForce = Random.Range(-torqueImpulse, torqueImpulse);

            // Apply the randomized torque force to the obstacle
            rb.AddTorque(torqueForce, ForceMode2D.Impulse);
        }
        else
        {
            // Apply the provided torque impulse to the obstacle
            rb.AddTorque(torqueImpulse, ForceMode2D.Impulse);
        }
    }

    /// Set up the obstacle ///
    public void SetObstacle()
    {
        float heavierMass = 5;
        float lighterMass = .025f;
        
        float heavierDrag = 1;
        float lighterDrag = 0f;
        
        if (randomizeType)
        {
            int randType = Random.Range(0, 5);

            switch (randType)
            {
                case 0:
                    type = "white";
                    spriteRend.color = whiteColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = false;
                    break;
                case 1:
                    type = "red";
                    spriteRend.color = redColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = true;
                    break;
                case 2:
                    type = "orange";
                    spriteRend.color = orangeColor;
                    rb.mass = lighterMass;
                    rb.drag = lighterDrag;
                    destructable = true;
                    break;
                case 3:
                    type = "yellow";
                    spriteRend.color = yellowColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = true;
                    break;
                case 4:
                    type = "green";
                    spriteRend.color = greenColor;
                    rb.mass = lighterMass;
                    rb.drag = lighterDrag;
                    destructable = false;
                    break;
                default:
                    type = "white";
                    spriteRend.color = whiteColor;
                    rb.mass = lighterMass;
                    rb.drag = heavierDrag;
                    destructable = false;
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case "white":
                    spriteRend.color = whiteColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = false;
                    break;
                case "red":
                    spriteRend.color = redColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = true;
                    break;
                case "orange":
                    spriteRend.color = orangeColor;
                    rb.mass = lighterMass;
                    rb.drag = lighterDrag;
                    destructable = true;
                    break;
                case "yellow":
                    spriteRend.color = yellowColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = true;
                    break;
                case "green":
                    spriteRend.color = greenColor;
                    rb.mass = lighterMass;
                    rb.drag = lighterDrag;
                    destructable = false;
                    break;
                default:
                    spriteRend.color = whiteColor;
                    rb.mass = heavierMass;
                    rb.drag = heavierDrag;
                    destructable = false;
                    break;
            }
        }

        if (randomizeSprite)
        {
            int obstacleIndex = Random.Range(0, obstacleSprites.Length);
            spriteRend.sprite = obstacleSprites[obstacleIndex];
        }
    }

    /// Destroys the asteroid ///
    public void Explode()
    {
        if (destructable)
        {
            ParticleSystem[] systems = Instantiate(explodeEffect, transform.position, transform.rotation).GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < systems.Length; i++)
            {
                systems[i].startColor = spriteRend.color;
                systems[i].Play();
            }

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (type == "green")
        {
            ObstaclePrefab otherObstacle = other.gameObject.GetComponent<ObstaclePrefab>();
            if (otherObstacle != null && otherObstacle.type == "orange")
            {
                otherObstacle.Explode();
            }
        }
    }
}
