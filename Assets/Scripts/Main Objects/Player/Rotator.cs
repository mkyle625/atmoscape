using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed;
    public bool randomizeDirection;

    private int rand;

    public void Start()
    {
        rand = Random.Range(1, 3);
    }

    public void Update()
    {
        if (randomizeDirection)
        {
            if (rand == 1)
                transform.Rotate(new Vector3(0, 0, rotationSpeed * -1 * Time.deltaTime));
            else
                transform.Rotate(new Vector3(0, 0, rotationSpeed * 1 * Time.deltaTime));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }
    }
}
