using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {
    private Vector3 initialPosition;
    private Vector3 initialVelocity;
    private Vector3 initialAngularVelocity;
    private Rigidbody rigidBody;
	
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        initialVelocity = rigidBody.velocity;
        initialAngularVelocity = rigidBody.angularVelocity;
        initialPosition = gameObject.transform.position;
    }
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.ObjectsTags.GROUND))
        {
            gameObject.transform.position = initialPosition;
            rigidBody.velocity = initialVelocity;
            rigidBody.angularVelocity = initialAngularVelocity;
        }
    }
}
