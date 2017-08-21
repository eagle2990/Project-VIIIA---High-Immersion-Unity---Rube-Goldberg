using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class HMDManager : MonoBehaviour
{

    private static bool HmdChoosen;

    [Header("Vive Setup")]
    public GameObject ViveRig;
    private SteamVR_ControllerManager controllerManager;
    private static SteamVR_TrackedObject leftHand;
    private static SteamVR_TrackedObject rightHand;
    private static SteamVR_Controller.Device leftHandDevice;
    private static SteamVR_Controller.Device rightHandDevice;

    [Header("Oculus Setup")]
    public GameObject OculusRig;

    void Start()
    {
        SetHMDType();
    }

    void Update()
    {
        if (!HmdChoosen)
        {
            SetHMDType();

        }
        else if (IsViveHeadset())
        {
            AreDevicesRegistered();
        }


    }

    #region Vive Controller.Device generic registration and usage

    public static SteamVR_Controller.Device GetDevice(SteamVR_TrackedObject trackedObject)
    {
        return SteamVR_Controller.Input((int)trackedObject.index);
    }

    private static bool IsDeviceValid(SteamVR_Controller.Device device)
    {
        return device != null;
    }

    public static bool IsButtonHoldFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetPress(button);
        }
        return false;
    }

    public static bool IsButtonPressedFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetPressDown(button);
        }
        return false;
    }

    public static bool IsButtonReleasedFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetPressUp(button);
        }
        return false;
    }

    public static bool IsButtonTouchDownFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetTouchDown(button);
        }
        return false;
    }

    public static bool IsButtonTouchFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetTouch(button);
        }
        return false;
    }

    public static bool IsButtonTouchUpFromDevice(SteamVR_Controller.Device device, ulong button)
    {
        if (IsViveHeadset())
        {
            if (IsDeviceValid(device)) return device.GetTouchUp(button);
        }
        return false;
    }


    #endregion

    #region Vive Controller.Device propetary registration and usage

    private static void RegisterLeftHandDevice()
    {
        if (IsViveHeadset())
        {
            leftHandDevice = SteamVR_Controller.Input((int)leftHand.index);
        }
    }

    private static void RegisterRightHandDevice()
    {
        if (IsViveHeadset())
        {
            rightHandDevice = SteamVR_Controller.Input((int)rightHand.index);
        }
    }

    private static void AreDevicesRegistered()
    {
        if (!IsDeviceValid(leftHandDevice))
        {
            RegisterLeftHandDevice();
        }
        if (!IsDeviceValid(rightHandDevice))
        {
            RegisterRightHandDevice();
        }
    }

    private static void ValidateSetup()
    {
        if (IsViveHeadset())
        {
            AreDevicesRegistered();
        }
    }

    public static bool IsLeftTouchpadHold()
    {
        ValidateSetup();
        return IsButtonHoldFromDevice(leftHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static bool IsLeftTouchpadReleased()
    {
        ValidateSetup();
        return IsButtonReleasedFromDevice(leftHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static bool IsRightTouchpadTouchDown()
    {
        ValidateSetup();
        return IsButtonTouchDownFromDevice(rightHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static bool IsRightTouchpadTouch()
    {
        ValidateSetup();
        return IsButtonTouchFromDevice(rightHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static bool IsRightTouchpadTouchUp()
    {
        ValidateSetup();
        return IsButtonTouchUpFromDevice(rightHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static bool IsRightTouchpadPressed()
    {
        ValidateSetup();
        return IsButtonPressedFromDevice(rightHandDevice, SteamVR_Controller.ButtonMask.Touchpad);
    }

    public static float GetRightTouchpadXAxisMovement()
    {
        ValidateSetup();
        return rightHandDevice.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x;
    }
    #endregion

    #region Headset Methods
    public static bool IsViveHeadset()
    {
        return VRDevice.model.Contains("Vive");
    }

    public static bool IsOculusHeadset()
    {
        return VRDevice.model.Contains("oculus");
    }

    private void SetHMDType()
    {
        if (IsViveHeadset())
        {
            ActivateVive();
        }
        else if (IsOculusHeadset())
        {
            ActivateOculus();
        }
        else
        {
            HmdChoosen = false;
        }
    }

    private void ActivateVive()
    {
        controllerManager = ViveRig.GetComponent<SteamVR_ControllerManager>();
        leftHand = controllerManager.left.GetComponent<SteamVR_TrackedObject>();
        rightHand = controllerManager.right.GetComponent<SteamVR_TrackedObject>();
        ViveRig.SetActive(true);
        OculusRig.SetActive(false);
        HmdChoosen = true;

    }

    private void ActivateOculus()
    {
        ViveRig.SetActive(false);
        OculusRig.SetActive(true);
        HmdChoosen = true;
    }

    public static SteamVR_TrackedObject GetLeftHand()
    {
        return leftHand;
    }

    public static SteamVR_TrackedObject GetRightHand()
    {
        return rightHand;
    }
    #endregion
}
