using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public float moveSpeed;
    private GameObject bag;
    private GameObject scanner;
    private bool moving;
    private float animationStart = -1.0f;
    private float lastX;
    private float lastZ;
    private Quaternion lastRotation;
    private GameObject initialBagPosition;
    private GameObject initialScannerPosition;

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
    }

    // Update is called once per frame
    void Update()
    {
        moving = false;
        if(Input.GetKey(KeyCode.DownArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            Camera.main.transform.position += new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z) * -moveSpeed;
            shakeCamera();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            Camera.main.transform.position += new Vector3(Camera.main.transform.forward.x, 0.0f, Camera.main.transform.forward.z) * moveSpeed;
            shakeCamera();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moving = true;
            if (animationStart == -1.0f) animationStart = Time.time;
            Camera.main.transform.position += new Vector3(Camera.main.transform.right.x, 0.0f, Camera.main.transform.right.z) * -moveSpeed;
            shakeCamera();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (animationStart == -1.0f) animationStart = Time.time;
            moving = true;
            Camera.main.transform.position += new Vector3(Camera.main.transform.right.x, 0.0f, Camera.main.transform.right.z) * moveSpeed;
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
        //bag.transform.position = new Vector3(bag.transform.position.x, initialBagY + 0.2f * Math.Abs(Mathf.Sin(4 * (Time.time - animationStart))), bag.transform.position.z);
        //scanner.transform.position = new Vector3(scanner.transform.position.x, initialScannerY + 0.2f * Math.Abs(Mathf.Sin(4 * (Time.time - animationStart))), scanner.transform.position.z);
        bag.transform.position = bag.transform.position + bag.transform.forward * 0.003f * Mathf.Sin(6 * (Time.time - animationStart));
        scanner.transform.position = scanner.transform.position + scanner.transform.up * 0.003f * Mathf.Sin(6 * (Time.time - animationStart));
    }
}
