using UnityEngine;
using UnityEngine.XR;

public class ControlWeapon : MonoBehaviour
{
    private InputDevice device;
    private bool supportsRotation;

    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        supportsRotation = device.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);
        if (supportsRotation)
        {
            this.transform.localRotation = rotation;
        }

        var transformPosition = transform.position;

        var pos = transformPosition;
        pos.x = transformPosition.x + Input.GetAxis("Horizontal") / 10;
        transform.position = pos;
    }
}
