using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.XR;

public class CameraMovementScript : MonoBehaviour
{
    public float moveSpeed;
    private GameObject bag;
    private GameObject scanner;
    private bool moving;
    private float animationStart = -1.0f;
    private float lastX;
    private float lastZ;
    private GameObject initialBagPosition;
    private GameObject initialScannerPosition;

    private InputDevice device;
    private Vector2 touchpadState;

    void Start()
    {
        bag = GameObject.Find("Bag");
        scanner = GameObject.Find("Scanner-Eward");
        initialBagPosition = new GameObject();
        initialBagPosition.transform.position = bag.transform.position;
        initialBagPosition.transform.parent = transform; 
        initialScannerPosition = new GameObject();
        initialScannerPosition.transform.position = scanner.transform.position;
        initialScannerPosition.transform.parent = transform;
        lastX = transform.position.x;
        lastZ = transform.position.z;

        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        moving = false;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out touchpadState);
        if (touchpadState.y < 0.0f)
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(transform.forward.x, 0.0f, transform.forward.z) * -moveSpeed;
            shakeCamera();
        }
        if (touchpadState.y > 0.0f)
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(transform.forward.x, 0.0f, transform.forward.z) * moveSpeed;
            shakeCamera();
        }
        if (touchpadState.x < 0.0f)
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(transform.right.x, 0.0f, transform.right.z) * -moveSpeed;
            shakeCamera();
        }
        if (touchpadState.x > 0.0f)
        {
            if (animationStart == -1.0f) animationStart = Time.time;
            moving = true;
            transform.position += new Vector3(transform.right.x, 0.0f, transform.right.z) * moveSpeed;
            shakeCamera();
        }
        if (!moving)
        {
            animationStart = -1.0f;
            if (bag.transform.position != initialBagPosition.transform.position && (lastX != transform.position.x || lastZ != transform.position.z))  
            {
                bag.transform.position = Vector3.MoveTowards(bag.transform.position, new Vector3(initialBagPosition.transform.position.x, initialBagPosition.transform.position.y, initialBagPosition.transform.position.z), 1.0f * Time.deltaTime);
                scanner.transform.position = Vector3.MoveTowards(scanner.transform.position, new Vector3(initialScannerPosition.transform.position.x, initialScannerPosition.transform.position.y, initialScannerPosition.transform.position.z), 1.0f * Time.deltaTime);
            }
            else
            {
                lastX = transform.position.x;
                lastZ = transform.position.z;
            }   
        }
    }

    void shakeCamera()
    {
        bag.transform.position = bag.transform.position + bag.transform.forward * 0.004f * Mathf.Sin(6 * (Time.time - animationStart));
        scanner.transform.position = scanner.transform.position + scanner.transform.up * 0.004f * Mathf.Sin(6 * (Time.time - animationStart));
    }
}
