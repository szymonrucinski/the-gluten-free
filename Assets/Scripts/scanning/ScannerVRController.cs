using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.XR;

public class ScannerVRController : MonoBehaviour
{
    InputDevice device;
    Quaternion deviceRotation;
    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        if(device.TryGetFeatureValue(CommonUsages.deviceRotation, out deviceRotation)) transform.localRotation = deviceRotation;
    }
}
