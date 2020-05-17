using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ScannerVRController : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> rightHandDevices = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDevice device;
    Quaternion deviceRotation;
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count == 1)
        {
            device = rightHandDevices[0];
            Debug.Log("It's alive");
        }
        else if (rightHandDevices.Count > 1)
        {
            Debug.Log("Found more than one left hand!");
        }
        else
        {
            Debug.Log("0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out deviceRotation);
        transform.rotation = deviceRotation;
    }
}
