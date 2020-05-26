using System;
using UnityEngine;
using TMPro;

public class ProductMovement : MonoBehaviour
{
    private float Speed = 0.02f;
    private float thrust = 7;
    private GameObject Food;

    private Vector3 Move;
    private bool MoveForwardBool = true;
    private bool MoveRightBool = false;
    private bool bought = false;

    private Rigidbody body;

    private TextMeshProUGUI shoppingList;
    private TextMeshProUGUI[] ugiu;
    private ShoppingList shoppingListScript = null;
    private ScoreAction scoreController;
    private GameController gameController = null;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        Speed = Speed * ((GameObject.Find("ShoppingList").GetComponent<ShoppingList>().numberOfProductsBought + 1));
        Move = new Vector3(Speed, 0, 0);
        body = this.GetComponent<Rigidbody>();
        Food = this.gameObject;
        ugiu = FindObjectsOfType<TextMeshProUGUI>();

        int i;
        for (i = 0; i < ugiu.Length; i++)
        {
            if (ugiu[i].name.Contains("ShoppingList"))
            {
                shoppingList = ugiu[i];
                break;
            }
            else
            {
                print("Wrong UGIU");
            }
        }
        shoppingListScript = FindObjectOfType<ShoppingList>().GetComponent<ShoppingList>();
        scoreController = ScoreController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bought && GameController.Instance.currentGameState == GameState.InGame)
        {
            if (transform.position.x <= 4)
            {
                MoveForwardBool = true;
            }

            if (transform.position.x >= 3)
            {
                Destroy(Food);
            }
            if (Input.GetMouseButtonDown(0) && transform.position.x > -0.83 && transform.position.x < 0.5)
            {
                Buying();
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

        if (Input.GetMouseButtonDown(1) && bought)
        {
            Destroy(Food);
        }

        if (gameController.currentGameState.Equals(GameState.GameOver))
        {
            Destroy(Food);
        }
    }

    private void UpdateShoppingList(string tag)
    {
        string objectname = this.name.Substring(0, this.name.IndexOf('('));
        if (shoppingList.text.Contains(objectname))
        {
            int line = (shoppingList.text.Substring(0, shoppingList.text.IndexOf(objectname))).Split('\n').Length - 1;
            shoppingListScript.Toggle(line - 1);
            UpdateScore(tag, true);
        }
        else
        {
            UpdateScore(tag, false);
        }

    }

    private void UpdateScore(string tag, bool onShoppingList)
    {
        if (tag.Contains("good") && onShoppingList)
        {
            scoreController.scoreAction(true, true, transform.position);
        }
        if (tag.Contains("good") && !onShoppingList)
        {
            scoreController.scoreAction(true, false, transform.position);
        }
        if (tag.Contains("bad"))
        {
            scoreController.scoreAction(false, false, transform.position);
        }
    }

    private void MoveRight()
    {
        body.AddForce(0, 0, -thrust, ForceMode.Impulse);
        MoveRightBool = false;
    }

    private void MoveForward()
    {
        if (MoveForwardBool)
        {
            transform.position += Move;
        }

    }

    public void Buying()
    {
        UpdateShoppingList(tag);
        bought = true;
        MoveForwardBool = false;
        MoveRightBool = true;
    }
}

