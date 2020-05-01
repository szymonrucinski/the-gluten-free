using System;
using UnityEngine;
using UnityEngine.XR;

public class Pointer : MonoBehaviour
{
    public LayerMask layerMask;

    private InputDevice device;
    private LineRenderer rend;

    private void Awake()
    {
        GameController.OnGameStateChanged += HandleStateChange;
    }

    private void OnDestroy()
    {
        GameController.OnGameStateChanged -= HandleStateChange;
    }

    void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        rend = GetComponent<LineRenderer>();
        rend.enabled = true;
    }

    void Update()
    {
        if(rend.enabled) AlignLineRenderer(rend);
    }

    private void AlignLineRenderer(LineRenderer lineRend)
    {
        var supportsRotation = device.TryGetFeatureValue(CommonUsages.deviceRotation, out var rotation);
        var supportsPosition = device.TryGetFeatureValue(CommonUsages.devicePosition,out var position);
        var supportsTrigger = device.TryGetFeatureValue(CommonUsages.triggerButton, out var trigger);

        if (!supportsTrigger) return;

        if (supportsRotation)
        {
            this.transform.localRotation = rotation;
        }

        if (supportsPosition)
        {
            this.transform.localPosition = position;
        }

        var ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out var hit, layerMask))
        {
            lineRend.startColor = Color.red;
            lineRend.endColor = Color.red;
            lineRend.SetPosition(1, transform.forward + new Vector3(0, 0, hit.distance));
        }
        else
        {
            lineRend.startColor = Color.green;
            lineRend.endColor = Color.green;
            lineRend.SetPosition(1, transform.forward + new Vector3(0, 0, 20));
        }

        lineRend.material.color = lineRend.startColor;
    }

    private void HandleStateChange(GameState state)
    {
        //Enable only if not in game
        rend.enabled = (state != GameState.InGame);
    }
}
