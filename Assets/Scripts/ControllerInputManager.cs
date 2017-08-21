using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR.InteractionSystem;

public class ControllerInputManager : MonoBehaviour
{
    [Header("HMD Devices")]
    private SteamVR_TrackedObject leftHand;
    private SteamVR_TrackedObject rightHand;
    //private SteamVR_ControllerManager controllerManager;
    public GameObject head;

    [Header("Teleport")]
    public LayerMask floorLayerMask;
    public float teleportMaxDistance;
    public GameObject teleportAimerObject;
    private TeleportMarker teleportMarkerScript;

    private SteamVR_Controller.Device leftHandDevice;
    private SteamVR_Controller.Device rightHandDevice;
    private GameObject leftHandPointer;
    private Laser laser;

    private bool isValidTeleport;
    private Vector3 startPoint;
    private Vector3 fwd;
    private Vector3 teleportLocation;

    [Header("Object Menu")]
    public GameObject objectMenu;
    private ObjectMenuManager objectMenuManager;
    private float swipeSum;
    private float touchLast;
    private float touchCurrent;
    private float swipeDistance;


    // Use this for initialization
    void Start()
    {
        SetupTeleport();

        SetupObjectMenu();
    }

    // Update is called once per frame
    void Update()
    {
        EnableTeleport();
        EnableObjectMenu();
    }

    #region Teleport Logic
    private void EnableTeleport()
    {
        if (HMDManager.IsLeftTouchpadHold())
        {
            ActivateTeleportMarker();
            SetupRaycast();
            RaycastHit hit;

            if (Physics.Raycast(startPoint, fwd, out hit, teleportMaxDistance, floorLayerMask))
            {
                teleportLocation = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                isValidTeleport = true;
                teleportMarkerScript.SetValid();
                laser.SetValid();
            }
            else
            {
                teleportLocation = new Vector3(fwd.x * teleportMaxDistance + startPoint.x, fwd.y * teleportMaxDistance + startPoint.y, fwd.z * teleportMaxDistance + startPoint.z);
                RaycastHit groundHit;
                if (Physics.Raycast(teleportLocation, Vector3.down, out groundHit, teleportMaxDistance, floorLayerMask))
                {
                    teleportLocation = new Vector3(teleportLocation.x, groundHit.point.y, teleportLocation.z);
                    isValidTeleport = true;
                    teleportMarkerScript.SetValid();
                    laser.SetValid();
                }
                else
                {
                    isValidTeleport = false;
                    teleportMarkerScript.SetInvalid();
                    laser.SetInvalid();
                }
            }
            laser.SetEnd(teleportLocation);
            teleportAimerObject.transform.position = teleportLocation;
        }

        if (HMDManager.IsLeftTouchpadReleased())
        {
            if (isValidTeleport)
            {
                Vector3 feetPositionGuess = gameObject.transform.position + Vector3.ProjectOnPlane(head.transform.position - gameObject.transform.position, gameObject.transform.up);
                Vector3 playerFeetOffset = gameObject.transform.position - feetPositionGuess;
                gameObject.transform.position = teleportLocation + playerFeetOffset;

            }
            DeactivateTeleportMarker();
        }
    }

    private void ActivateTeleportMarker()
    {
        ValidateTeleportSetup();
        teleportAimerObject.SetActive(true);
        leftHandPointer.SetActive(true);
    }

    private void DeactivateTeleportMarker()
    {
        ValidateTeleportSetup();
        leftHandPointer.SetActive(false);
        teleportAimerObject.SetActive(false);
    }

    private void SetupRaycast()
    {
        Transform leftControllerTransform = leftHand.transform;
        startPoint = leftControllerTransform.position;
        fwd = leftControllerTransform.forward;
        if (startPoint != null)
            laser.SetBegining(startPoint);
    }

    private void SetupTeleport()
    {
        leftHand = HMDManager.GetLeftHand();
        if (leftHand != null)
        {
            foreach (Transform child in leftHand.transform)
            {
                if (child.gameObject.CompareTag(Constants.ObjectsTags.POINTER))
                {
                    leftHandPointer = child.gameObject;
                }
            }
        }
        
        if (teleportMaxDistance <= 0)
        {
            teleportMaxDistance = 10f;
        }
        teleportAimerObject.SetActive(false);
        if (leftHandPointer != null)
        {
            leftHandPointer.SetActive(false);
            laser = leftHandPointer.GetComponent<Laser>();
        }
        
        isValidTeleport = false;
        teleportMarkerScript = teleportAimerObject.GetComponent<TeleportMarker>();
        
    }

    private void ValidateTeleportSetup()
    {
        if (leftHand == null || leftHandPointer == null || teleportMarkerScript == null || laser ==  null)
        {
            SetupTeleport();
        }
    }
    #endregion

    #region Object Menu Logic
    private void EnableObjectMenu()
    {
        if (HMDManager.IsRightTouchpadTouchDown())
        {
            objectMenu.SetActive(true);
            touchLast = HMDManager.GetRightTouchpadXAxisMovement();
        }

        if (HMDManager.IsRightTouchpadTouch())
        {
            objectMenu.SetActive(true);
            touchCurrent = HMDManager.GetRightTouchpadXAxisMovement();
            swipeDistance = touchCurrent - touchLast;
            touchLast = touchCurrent;
            swipeSum += swipeDistance;


            if (swipeSum > 0.4f)
            {
                swipeSum = 0;
                SwipeRight();
            }

            if (swipeSum < -0.4f)
            {
                swipeSum = 0;
                SwipeLeft();
            }
        }

        if(HMDManager.IsRightTouchpadTouchUp())
        {
            SwipeVarsReset();
        }

        if (HMDManager.IsRightTouchpadPressed())
        {
            if (objectMenu.activeSelf)
            {
                SpawnObject();
            }
            
        }
    }

    void SpawnObject()
    {
        ValidateObjectMenu();
        objectMenuManager.SpawnCurrentObject(rightHand.transform.position + (rightHand.transform.forward * 1.5f));
    }

    void SwipeLeft()
    {
        objectMenuManager.MenuLeft();
    }

    void SwipeRight()
    {
        objectMenuManager.MenuRight();
    }

    void SwipeVarsReset()
    {
        swipeSum = 0;
        touchCurrent = 0;
        touchLast = 0;
        objectMenu.SetActive(false);
    }

    private void SetupObjectMenu()
    {
        rightHand = HMDManager.GetRightHand();
        if (objectMenu != null)
        {
            objectMenuManager = objectMenu.GetComponent<ObjectMenuManager>();
        }
        objectMenu.SetActive(false);
    }

    private void ValidateObjectMenu()
    {
        if (rightHand == null || objectMenuManager == null)
        {
            SetupObjectMenu();
        }

        if (objectMenu == null)
        {
            Debug.LogWarning("You need to setup the Object Menu.");
        }
    }
    #endregion
}
