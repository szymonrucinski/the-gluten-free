using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanReticle : MonoBehaviour
{
    private RectTransform reticle;
    public float restingSize=64;
    public float maxSize;
    public float speedOfScale;
    private float currentSize;


    void Start()
    {
        reticle = GetComponent<RectTransform>();
    }

    // Update is called once per frame
     void Update()
    {
            if (isMoving)
                {
                currentSize = Mathf.Lerp(currentSize,maxSize,Time.deltaTime*speedOfScale);
                }
            else
                currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speedOfScale);

                reticle.sizeDelta = new Vector2(currentSize, currentSize);

    }



    bool isMoving
    {
        get {
                    if (Input.GetAxis("Horizontal") != 0 ||
                  Input.GetAxis("Vertical") != 0 ||
                  Input.GetAxis("Mouse X") != 0 ||
                  Input.GetAxis("Mouse Y") != 0)
                        return true;

                    else return false;
        }


    }

  
   
}
