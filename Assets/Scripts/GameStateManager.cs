using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    private GameState currentGameState = GameState.EDIT;
    public Color playStateColor;
    public Color editStateColor;
    public Color cheatStateColor;

    public Outline safeArea;
    public Material ballPlayMaterial;
    public Material ballEditMaterial;
    public Material ballCheatMaterial;
    public GameObject playerBall;

    private List<GameObject> collectableStars;
    private int starsCount;

    private bool outsideSafeArea = false;
    private bool leftHandPlayerInside = true;
    private bool rightHandPlayerInside = true;
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (collectableStars == null)
        {
            collectableStars = new List<GameObject>(GameObject.FindGameObjectsWithTag(Constants.ObjectsTags.COLLECTABLE));
        }
    }

    public bool IsOutsideSafeArea()
    {
        return outsideSafeArea;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public bool IsStateEditing()
    {
        return currentGameState == GameState.EDIT;
    }

    public bool IsStatePlaying()
    {
        return currentGameState == GameState.PLAY;
    }

    public bool IsStateCheating()
    {
        return currentGameState == GameState.CHEATING;
    }

    public void Play()
    {
        currentGameState = GameState.PLAY;
        safeArea.effectColor = playStateColor;
        playerBall.GetComponent<Renderer>().material = ballPlayMaterial;

    }

    public void Edit()
    {
        currentGameState = GameState.EDIT;
        outsideSafeArea = false;
        safeArea.effectColor = editStateColor;
        playerBall.GetComponent<Renderer>().material = ballEditMaterial;
    }

    public void CheatAlert()
    {
        currentGameState = GameState.CHEATING;
        outsideSafeArea = true;
        safeArea.effectColor = cheatStateColor;
        playerBall.GetComponent<Renderer>().material = ballCheatMaterial;
    }

    public bool AreStarsCollected()
    {
        int collected = 0;
        foreach (GameObject star in collectableStars)
        {
            if(!star.activeSelf)
            {
                collected++;
            }
        }
        return collected == collectableStars.Count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ObjectsTags.PLAYER))
        {
            if (other.gameObject.name.Contains("left"))
            {
                leftHandPlayerInside = true;
            }

            if (other.gameObject.name.Contains("right"))
            {
                rightHandPlayerInside = true;
            }
        }

        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            Edit();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ObjectsTags.PLAYER))
        {
            if (other.gameObject.name.Contains("left"))
            {
                leftHandPlayerInside = false;
            }

            if (other.gameObject.name.Contains("right"))
            {
                rightHandPlayerInside = false;
            }
        }


        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            if (leftHandPlayerInside && rightHandPlayerInside && PlayerIsNotKinematic())
            {
                Play();
            }
            else
            {
                CheatAlert();
            }
            outsideSafeArea = true;
        }
    }

    private bool PlayerIsNotKinematic()
    {
        return !playerBall.GetComponent<Rigidbody>().isKinematic; ;
    }
}

public enum GameState
{
    PLAY,
    EDIT,
    CHEATING
}
