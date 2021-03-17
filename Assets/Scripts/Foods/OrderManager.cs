using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OrderManager : MonoBehaviour
{ 
    // NOTE! The number here reflects how many ingredients go BETWEEN the
    // top and bottom bun, since the top and bottom bun will ALWAYS show the
    // start and end of an order. 
    [Header("Higher difficulty = more order components, aka harder")]
    public int difficulty = 2;

    private int ingredientCount;
    private bool currentOrderExists;
    private string[] ingredients;

    private List<string> currentOrder;

    void Start()
    {
        ingredients = Enum.GetNames(typeof(FoodItem.Food));
        ingredientCount = ingredients.Length;
        currentOrderExists = false;
        currentOrder = new List<string>();
    } 

    void Update()
    {
        // If theres no order, create one
        if(!currentOrderExists)
        {
            InitializeOrder();
            LogCurrentOrder();
        }
    }

    void InitializeOrder()
    {
        // Clear out the current order list just in case
        currentOrder.Clear();
        // All orders will start with a bottom bun
        currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), 1));
        // Grab random ingreidients and add to list
        for(int i = 0; i < difficulty; ++i)
        {
            int index = UnityEngine.Random.Range(2, ingredientCount);
            currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), index));
        }
        // All orders end with a top bun
        currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), 0));
        currentOrderExists = true;
    }

    void LogCurrentOrder()
    {
        foreach(string s in currentOrder)
            Debug.Log(s);
    }

    public List<string> GetCurrentOrder()
    {
        return currentOrder;
    }
}
