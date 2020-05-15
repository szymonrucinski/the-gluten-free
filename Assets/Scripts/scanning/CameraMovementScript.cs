using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public float moveSpeed;
    private GameObject bag;
    private GameObject scanner;

    void Start()
    {
        bag = GameObject.Find("Bag");
        scanner = GameObject.Find("Scanner-Eward");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position +=  new Vector3(0.0f, 0.0f, -moveSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position +=  new Vector3(0.0f, 0.0f, moveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position += new Vector3(-moveSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position += new Vector3(moveSpeed, 0.0f, 0.0f);
        }
    }

    void shakeCamera()
    {

    }
}
