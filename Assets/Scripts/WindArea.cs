using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour {

    private List<Rigidbody> rigidbodies = new List<Rigidbody>();
    private Vector3 windDirection;
    public float windStrength = 5;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();

        if (rigidBody != null)
            rigidbodies.Add(rigidBody);
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rigidBody = other.gameObject.GetComponent<Rigidbody>();

        if (rigidBody != null)
            rigidbodies.Remove(rigidBody);
    }

    private void FixedUpdate()
    {
        windDirection = transform.forward;
        if (rigidbodies.Count > 0)
        {
            foreach (Rigidbody rigid in rigidbodies)
            {
                rigid.AddForce(windDirection * windStrength);
            }
        }
    }
}
