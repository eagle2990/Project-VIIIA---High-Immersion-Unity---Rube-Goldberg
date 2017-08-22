using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectStar : MonoBehaviour {
    public AudioClip clip;
    public GameStateManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE) && gameManager.IsStatePlaying())
        {
            gameObject.SetActive(false);
        }
    }
}
