using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCollider : MonoBehaviour
{
    [Header("Unity Setup")]
    public Player player;

    private void OnCollisionEnter2D(Collision2D other)
    {
        ObstaclePrefab obstacle = other.collider.GetComponent<ObstaclePrefab>();

        if (obstacle != null)
        {
            player.Crash(other, obstacle, true);
        }
    }
}
