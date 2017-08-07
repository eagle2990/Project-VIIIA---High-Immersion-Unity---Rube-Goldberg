using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotation : MonoBehaviour
{
    public float initialSpeed = 0.0f;
    public float maxSpeed = 10.0f;
    public float acelerator = 1.0f;
    public string rotationAngle = "forward";

    private float currentSpeed;

    // Use this for initialization
    void Start()
    {
        currentSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationAngle.Contains("up"))
            transform.Rotate(Vector3.up, currentSpeed);
        else if (rotationAngle.Contains("forward"))
            transform.Rotate(Vector3.forward, currentSpeed);
        else
            transform.Rotate(Vector3.right, currentSpeed);

        if(currentSpeed <= maxSpeed)
        {
            currentSpeed += Time.deltaTime * acelerator;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}
