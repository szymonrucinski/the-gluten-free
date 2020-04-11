using System;
using UnityEngine;
using TMPro;

public class ProductMovement : MonoBehaviour
{
    private float Speed = 0.04f;
    private float thrust = 8;
    private GameObject Food;

    private Vector3 Move;
    private bool MoveForwardBool = true;
    private bool MoveRightBool = false;
    private bool bought = false;

    private Rigidbody body;

    private TextMeshProUGUI shoppingList;
    private ShoppingList shoppingListScript;
    private ScoreAction scoreController;

    // Start is called before the first frame update
    void Start()
    {
        Move = new Vector3(0, 0, Speed);
        body = this.GetComponent<Rigidbody>();
        Food = this.gameObject;
        shoppingList = FindObjectOfType<TextMeshProUGUI>();
        shoppingListScript = FindObjectOfType<ShoppingList>(); 
        scoreController = ScoreController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bought)
        {
            if (transform.position.z <= 4)
            {
                MoveForwardBool = true;
            }

            if (transform.position.z >= 3)
            {
                Destroy(Food);
            }

            if (Input.GetMouseButtonDown(0) && transform.position.z > -1.32 && transform.position.z < 0.5)
            {
                UpdateShoppingList(tag);
                bought = true;
                MoveForwardBool = false;
                MoveRightBool = true;
            }

            if (MoveForwardBool)
            {
                MoveForward();
            }

            if (MoveRightBool)
            {
                MoveRight();
            }
        }
    }

    private void UpdateShoppingList(string tag)
    {
        string objectname = this.name.Substring(0, this.name.IndexOf('('));
        if (shoppingList.text.Contains(objectname))
        {
            int line = (shoppingList.text.Substring(0, shoppingList.text.IndexOf(objectname))).Split('\n').Length - 1;
            shoppingListScript.Toggle(line-1);
            //UpdateScore(tag);
        }
    }

    private void UpdateScore(string tag)
    {
        //TODO Update Score in UI
        scoreController.scoreAction(true, true, transform.position);
    }

    private void MoveRight()
    {
        body.AddForce(thrust,0,0,ForceMode.Impulse);
        MoveRightBool = false;
    }

    private void MoveForward()
    {
        if (MoveForwardBool)
        {
            transform.position += Move;
        }
        
    }
}
