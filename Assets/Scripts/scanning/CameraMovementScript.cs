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
    //Szymon
    private Rigidbody rb;
    float frontBlockZ;
    float backBlockZ;
    float leftBlockX;
    float rightBlockX;


    void Start()
    {
        bag = GameObject.Find("Bag");
        scanner = GameObject.Find("Scanner-Eward");
        initialBagPosition = new GameObject();
        initialBagPosition.transform.position = bag.transform.position;
        initialBagPosition.transform.parent = Camera.main.transform; 
        initialScannerPosition = new GameObject();
        initialScannerPosition.transform.position = scanner.transform.position;
        initialScannerPosition.transform.parent = Camera.main.transform;
        lastX = Camera.main.transform.position.x;
        lastZ = Camera.main.transform.position.z;
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);


        //Szymon
        rb = GameObject.Find("Main Camera").GetComponent<Rigidbody>();

        frontBlockZ = 140;
        backBlockZ = 80;
        leftBlockX = -21;
        rightBlockX = 17.0f;

    }

    // Update is called once per frame
    void Update()
    {

        blockMovement();
        //rb.AddForce(transform.position*0.02f);
        //UnityEngine.Debug.Log("FORCE"+transform.position * 2);

        moving = false;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out touchpadState);
        if (touchpadState.y < 0.0f || Input.GetKey(KeyCode.DownArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z) * -moveSpeed;
            shakeCamera();
        }
        if (touchpadState.y > 0.0f || Input.GetKey(KeyCode.UpArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z) * moveSpeed;
            shakeCamera();
        }
        if (touchpadState.x < 0.0f || Input.GetKey(KeyCode.LeftArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            transform.position += new Vector3(Camera.main.transform.right.x, 0.0f, Camera.main.transform.right.z) * -moveSpeed;
            shakeCamera();
        }
        if (touchpadState.x > 0.0f || Input.GetKey(KeyCode.RightArrow))
        {
            if (animationStart == -1.0f) animationStart = Time.time;
            moving = true;
            transform.position += new Vector3(Camera.main.transform.right.x, 0.0f, Camera.main.transform.right.z) * moveSpeed;
            shakeCamera();
        }
        if (!moving)
        {
            animationStart = -1.0f;
            if (bag.transform.position != initialBagPosition.transform.position && (lastX != Camera.main.transform.position.x || lastZ != Camera.main.transform.position.z))
            {
                bag.transform.position = Vector3.MoveTowards(bag.transform.position, new Vector3(initialBagPosition.transform.position.x, initialBagPosition.transform.position.y, initialBagPosition.transform.position.z), 1.0f * Time.deltaTime);
                scanner.transform.position = Vector3.MoveTowards(scanner.transform.position, new Vector3(initialScannerPosition.transform.position.x, initialScannerPosition.transform.position.y, initialScannerPosition.transform.position.z), 1.0f * Time.deltaTime);
            }
            else
            {
                lastX = Camera.main.transform.position.x;
                lastZ = Camera.main.transform.position.z;
            }
        }
    }

    void shakeCamera()
    {
        bag.transform.position = bag.transform.position + bag.transform.forward * 0.004f * Mathf.Sin(6 * (Time.time - animationStart));
        scanner.transform.position = scanner.transform.position + scanner.transform.up * 0.004f * Mathf.Sin(6 * (Time.time - animationStart));
    }

    void blockMovement()
    {

        Debug.Log("postionZ" + transform.position.z);
        Debug.Log("postionX" + transform.position.x);


        if (transform.position.z >= frontBlockZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, frontBlockZ);

        }
        if (transform.position.z <= backBlockZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, backBlockZ);

        }
        if (transform.position.x <= leftBlockX)
        {
            transform.position = new Vector3(leftBlockX, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= rightBlockX)
        {
            transform.position = new Vector3(rightBlockX, transform.position.y, transform.position.z);
        }

    }
}


