using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrow : MonoBehaviour
{
    private SteamVR_TrackedObject hand;
    private SteamVR_Controller.Device device;

    private Transform previousParent;

    // Use this for initialization
    void Start()
    {
        hand = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        RegisterDevice();
    }

    private void FixedUpdate()
    {
        RegisterDevice();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.ObjectsTags.THROWABLE))
        {
            if (HMDManager.IsButtonReleasedFromDevice(device, SteamVR_Controller.ButtonMask.Trigger))
            {
                ThrowObject(other);
            }
            else if (HMDManager.IsButtonPressedFromDevice(device, SteamVR_Controller.ButtonMask.Trigger))
            {
                GrabObject(other);
            }
        }
    }

    private void RegisterDevice()
    {
        if (device == null)
        {
            device = HMDManager.GetDevice(hand);
        }
    }

    void GrabObject(Collider other)
    {
        //previousParent = other.transform.parent;
        other.transform.SetParent(transform);
        other.GetComponent<Rigidbody>().isKinematic = true;
        device.TriggerHapticPulse(2000);
        Debug.Log("You are touching down the trigger on " + other.name);
    }

    void ThrowObject(Collider other)
    {
        other.transform.SetParent(null);
        Rigidbody rigidBody = other.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.velocity = device.velocity * Constants.THROW_FORCE;
        rigidBody.angularVelocity = device.angularVelocity;
    }
}
