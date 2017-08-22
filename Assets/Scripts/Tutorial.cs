using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
    public GameObject NextPanel;
    public GameObject PreviousPanel;
    public GameObject ball;

    public bool disableLeftButton;
    public bool disableRightButton;

    public Button leftButton;
    public Button rightButton;
	// Use this for initialization
	void Start () {
        if (disableLeftButton)
        {
            leftButton.interactable = false;
        }

        if (disableRightButton)
        {
            rightButton.interactable = false;
        }
    }
	
	
    public void Previous()
    {
        PreviousPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Next()
    {
        NextPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void End()
    {
        ball.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
