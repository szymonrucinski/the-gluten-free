using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRCameraControll : MonoBehaviour
{
    private InputDevice device;
    private bool supportsRotation;

    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

    }

    // Update is called once per frame
    void Update()
    {
        supportsRotation = device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        if (supportsRotation)
            this.transform.localRotation = rotation;

        var pos = transform.position;
        pos.x = transform.position.x + Input.GetAxis("Horizontal") / 10;
        transform.position = pos;
    }
}
