using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayspaceWall : MonoBehaviour
{
    [Header("Settings")]
    public float pushbackForce; // Force applied toward the center of the playspace

    [Header("Unity Setup")]
    public Player player; // Player script reference

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.inPlayspace = false;

            player.rb.AddForce(new Vector2(pushbackForce, 0), ForceMode2D.Force);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.inPlayspace = true;
        }
    }
}
