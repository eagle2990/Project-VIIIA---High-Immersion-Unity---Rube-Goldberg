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
    private SteamVR_ControllerManager controllerManager;
    public GameObject head;

    [Header("Teleport")]
    public LayerMask floorLayerMask;
    public float teleportMaxDistance;
    public GameObject teleportAimerObject;
    private TeleportMarker teleportMarkerScript;

    private SteamVR_Controller.Device leftHandDevice;
    private SteamVR_Controller.Device rightHandDevice;
    private GameObject leftHandPointer;
    //private GameObject rightHandPointer;
    private Laser laser;

    private bool isValidTeleport;
    private Vector3 startPoint;
    private Vector3 fwd;
    private Vector3 teleportLocation;
    // Use this for initialization
    void Start()
    {
        controllerManager = GetComponent<SteamVR_ControllerManager>();
        leftHand = controllerManager.left.GetComponent<SteamVR_TrackedObject>();
        rightHand = controllerManager.right.GetComponent<SteamVR_TrackedObject>();

        foreach (Transform child in leftHand.transform)
        {
            if (child.tag == "Pointer")
            {
                leftHandPointer = child.gameObject;
            }
        }

        //foreach (Transform child in rightHand.transform)
        //{
        //    if (child.tag == "Pointer")
        //    {
        //        rightHandPointer = child.gameObject;
        //    }
        //}

        if (teleportMaxDistance <= 0)
        {
            teleportMaxDistance = 10f;
        }
        teleportAimerObject.SetActive(false);
        leftHandPointer.SetActive(false);
        isValidTeleport = false;
        teleportMarkerScript = teleportAimerObject.GetComponent<TeleportMarker>();
        laser = leftHandPointer.GetComponent<Laser>();
    }

    // Update is called once per frame
    void Update()
    {
        RegisterDevicesIDs();
        EnableTeleport();
        
    }

    void RegisterDevicesIDs()
    {
        leftHandDevice = SteamVR_Controller.Input((int)leftHand.index);
        rightHandDevice = SteamVR_Controller.Input((int)rightHand.index);
    }

    bool IsTouchpadHold()
    {
        if (IsViveHeadset())
        {
            return leftHandDevice.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
        }
        return false;
    }

    bool IsTouchpadReleased()
    {
        if (IsViveHeadset())
        {
            return leftHandDevice.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
        }
        return false;
    }

    bool IsViveHeadset()
    {
        return VRDevice.model.Contains("Vive");
    }

    private Vector3 getMaxDistance()
    {
        return new Vector3(0, 0, 0);
    }

    private void EnableTeleport()
    {
        if (IsTouchpadHold())
        {
            teleportAimerObject.SetActive(true);
            leftHandPointer.SetActive(true);
            Transform leftControllerTransform = controllerManager.left.transform;
            startPoint = leftControllerTransform.position;
            fwd = leftControllerTransform.forward;
            laser.SetBegining(startPoint);
            RaycastHit hit;

            if (Physics.Raycast(startPoint, fwd, out hit, teleportMaxDistance, floorLayerMask))
            {
                teleportLocation = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                teleportAimerObject.transform.position = teleportLocation;
                laser.SetEnd(teleportLocation);
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
                laser.SetEnd(teleportLocation);
                teleportAimerObject.transform.position = teleportLocation;
            }
            //laser.SetEnd(teleportLocation);
            //teleportAimerObject.transform.position = teleportLocation;
        }

        if (IsTouchpadReleased())
        {
            if (isValidTeleport)
            {
                Vector3 feetPositionGuess = gameObject.transform.position + Vector3.ProjectOnPlane(head.transform.position - gameObject.transform.position, gameObject.transform.up);
                Vector3 playerFeetOffset = gameObject.transform.position - feetPositionGuess;
                gameObject.transform.position = teleportLocation + playerFeetOffset;

            }
            leftHandPointer.SetActive(false);
            teleportAimerObject.SetActive(false);
        }
    }
}
