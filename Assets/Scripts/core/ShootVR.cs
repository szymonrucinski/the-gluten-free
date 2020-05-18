using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ShootVR : MonoBehaviour
{
    public GameObject laser;
    public InputDevice device;
    private HapticCapabilities capabilities;
    private bool supportsHaptics;
    private bool supportsTrigger;
    private IEnumerator laserCoroutine;
    


    // Start is called before the first frame update
    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        supportsHaptics = device.TryGetHapticCapabilities(out capabilities);
    }

    // Update is called once per frame
    void Update()
    {
        bool shooting = false;
        supportsTrigger = device.TryGetFeatureValue(CommonUsages.triggerButton, out shooting);
        if(!supportsTrigger && Input.GetButton("Fire1"))
        {
            shooting = true;
        }
        if (shooting)
        {
            Shoot();
        }
    }

    void Shoot()
    {

    }
}
