using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {
    private string GROUND_TAG = "Ground";
    private Vector3 initialPosition;
	
	void Start () {
        initialPosition = gameObject.transform.position;
	}
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            gameObject.transform.position = initialPosition;
        }
    }
}
