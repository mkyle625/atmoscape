using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [Header("Unity Setup")]
    public Player player;
    
    public void OnCollisionEnter2D(Collision2D other)
    {
        ObstaclePrefab obstacle = other.collider.GetComponent<ObstaclePrefab>();

        if (obstacle != null && !player.stunned)
        {
            player.Crash(other, obstacle, false);
        }
    }
}
