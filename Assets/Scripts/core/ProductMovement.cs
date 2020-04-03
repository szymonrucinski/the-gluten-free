using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductMovement : MonoBehaviour
{
    public float Speed = 0.01f;
    private GameObject Food;

    private Vector3 Move;
    // Start is called before the first frame update
    void Start()
    {
        Move = new Vector3(0, 0, Speed);
        Food = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= 4)
        {
            transform.position += Move;
        }
        else {
            Destroy(Food); 
        }
    }
}
