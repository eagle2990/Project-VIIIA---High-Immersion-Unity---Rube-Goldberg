using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public float offsetPosition = 0.001500005f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            if (!body.isKinematic)
            {
                body.isKinematic = true;
            }
            other.gameObject.transform.position = transform.position + (transform.up * offsetPosition);
        }
    }
}
