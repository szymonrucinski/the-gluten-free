using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class XRInputLoader : MonoBehaviour
{
    private StandaloneInputModule sim;
    private OVRInputModule oim;

    private void Awake()
    {
        sim = GetComponent<StandaloneInputModule>();
        oim = GetComponent<OVRInputModule>();
        
        if (XRDevice.isPresent)
        {
            sim.enabled = false;
            oim.enabled = true;
        }
        else
        {
            sim.enabled = true;
            oim.enabled = false;
        }
    }
}
