using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ShoppingList : MonoBehaviour
{
    public List<Toggle> checkboxes;
    private TextMeshProUGUI products;
    private string[] productNames = new string[] {"bread", "broccoli", "cabbage", "cake", "carrot", "melon", "pepper", "pizza"};
    private List<string> productList = new List<string>(4);

    // Start is called before the first frame update
    void Start()
    {
        products = GetComponent<TextMeshProUGUI>();
        products.text = "hello";
        RandomProducts();
    }

    private void RandomProducts()
    {
        //TODO Make random List of Products
        products.text = "Shopping List: \n bread \n cabbage \n melon";
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    internal void Toggle(int line)
    {
        checkboxes[line].isOn = true;
    }
}
