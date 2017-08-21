using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReset : MonoBehaviour {
    public GameObject collectibleStarsParent;
    public GameStateManager gameState;
    private List<GameObject> stars;
    private Vector3 initialPosition;
    private Vector3 initialVelocity;
    private Vector3 initialAngularVelocity;
    private Rigidbody rigidBody;
	
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        initialVelocity = rigidBody.velocity;
        initialAngularVelocity = rigidBody.angularVelocity;
        initialPosition = gameObject.transform.position;

        stars = new List<GameObject>();
        foreach(Transform child in collectibleStarsParent.transform)
        {
            if (child.gameObject.CompareTag(Constants.ObjectsTags.COLLECTABLE))
            {
                stars.Add(child.gameObject);
            }
        }
    }
	
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.ObjectsTags.GROUND))
        {
            gameObject.transform.position = initialPosition;
            rigidBody.velocity = initialVelocity;
            rigidBody.angularVelocity = initialAngularVelocity;
            foreach(GameObject star in stars)
            {
                star.SetActive(true);
            }
            gameState.Edit();
        }
    }
}
