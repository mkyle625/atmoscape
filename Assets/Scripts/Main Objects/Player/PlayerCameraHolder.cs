using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraHolder : MonoBehaviour
{
    [Header("Camera Attributes")]
    public float smoothFactor; // Change the lerp speed for the camera position

    [Header("Unity Setup")]
    public Transform playerTransform;

    ///*** Hidden public variables ***///
    [HideInInspector]
    public bool followPlayer = false; // Determines whether the camera should be following the player or not

    public void FixedUpdate()
    {
        if (playerTransform != null && followPlayer) // Check if the player is null
        {
            transform.position = Vector3.Lerp(transform.position, // Lerp the camera position to that of the player's
                new Vector3((playerTransform.position.x / 2f), playerTransform.position.y, transform.position.z), smoothFactor * Time.deltaTime);
        }
    }
}
