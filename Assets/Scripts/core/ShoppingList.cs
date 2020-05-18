using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour
{
    public List<Toggle> checkboxes;
    private TextMeshProUGUI products;
    private string[] productNames = new string[] { "bread", "broccoli", "cabbage", "cake", "carrot", "melon", "pepper", "pizza", "cheese", "cookie", "egg", "icecream", "orange", "pastry" };
    public string[] goodProductNames = new string[] { "broccoli", "cabbage", "carrot", "melon", "pepper", "cheese", "egg", "icecream", "orange" };
    public string[] randomProducts;
    public int numberOfProductsBought = 0;
    private GameController gameController;
    private GameOverController gameOverController;

    private void Awake()
    {
        products = GetComponent<TextMeshProUGUI>();
        RandomProducts();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.Instance;
        gameOverController = FindObjectOfType<GameOverController>();
        gameOverController.gameOverReason(true);
    }

    void Update()
    {
        numberOfProductsBought = 0;
        foreach (var box in checkboxes)
        {
            if (box.isOn)
            {
                numberOfProductsBought++;
            }
        }
        if(numberOfProductsBought == 5)
        {
            gameOverController.gameOverReason(false);
            gameController.GameOver();
        }

    }

    private void RandomProducts()
    {
        randomProducts = new string[5];
        var rand = new System.Random();
        var randomNumbers = Enumerable.Range(0, 8)
            .OrderBy(x => rand.Next())
            .Take(5)
            .ToList();

        for (var i = 0; i < 5; i++)
        {
            randomProducts[i] = goodProductNames[randomNumbers[i]];
        }

        products.text =
            $"Shopping List: \n {randomProducts[0].ToString()} \n {randomProducts[1].ToString()} \n {randomProducts[2].ToString()} " +
            $"\n {randomProducts[3].ToString()} \n {randomProducts[4].ToString()}";
    }

    public void Toggle(int line)
    {
        checkboxes[line].isOn = true;
    }
}
