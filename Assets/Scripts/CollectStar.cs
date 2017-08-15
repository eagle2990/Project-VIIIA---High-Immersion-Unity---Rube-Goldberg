using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStar : MonoBehaviour {
    public AudioClip clip;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            Destroy(gameObject);
        }
    }
}
