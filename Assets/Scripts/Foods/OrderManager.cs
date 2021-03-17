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

    private OrderDetection orderDetector;

    private ScoreManager scoreManager;

    private List<string> currentOrder;

    void Start()
    {
        orderDetector = GameObject.Find("OrderDetector").GetComponent<OrderDetection>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
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

        // Order comparison
        if(Input.GetMouseButtonDown(0))
        {
            (int, int) orderCorrectness = CompareOrderToPan(orderDetector.GetWhatsOnPan());
            // Item1 = good items
            // Item2 = bad items
            scoreManager.AddScore(orderCorrectness.Item1, orderCorrectness.Item2);
            currentOrderExists = false;
        }
    }

    void InitializeOrder()
    {
        // Clear out the current order list
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
        string printThis = "here's my order: ";
        foreach(string s in currentOrder)
            printThis += s + ", ";
        Debug.Log(printThis);
    }

    public List<string> GetCurrentOrder()
    {
        return currentOrder;
    }

    // First int is amount of good orders (in order)
    // Second int is amount of bad items (out of order)
    public (int, int) CompareOrderToPan(List<string> pan)
    {
        int goodItems = 0;
        int badItems = 0;

        // If this is true, then there are items that are going to be incorrect regardless of comparison
        // We can add the absolute difference to the badItems count.
        if(pan.Count != currentOrder.Count)
        {
            int incorrectCount = Mathf.Abs(pan.Count - currentOrder.Count);
            badItems += incorrectCount;
        }

        List<string>.Enumerator panIter = pan.GetEnumerator();
        List<string>.Enumerator orderIter = currentOrder.GetEnumerator();
        
        // This iterates the pan from the BOTTOM UP, looking for items in the exact order
        while(panIter.MoveNext() && orderIter.MoveNext())
        {
            string panItem = panIter.Current;
            string orderItem = orderIter.Current;

            if(panItem == orderItem)
            {
                ++goodItems;
            }
            else
            {
                ++badItems;
            }
        }
        // This iterates the pan from the TOP DOWN, looking for items in the exact order.
        // This subtracts from bad and adds to good if the item matches, and nothing otherwise
        //
        // This part is a little hard to explain, but bascially we wanna reward players if the order
        // is still decent from the top down and not solely from the bottom up. Otherwise if the top
        // half is all correct but the bottom ingredient is wrong, the whole thing is wrong.
        //
        // The algorithm doesnt have to perfectly account for whats right and wrong anyways, the score should
        // help abstract that 
        List<string> reversedPan = new List<string>(pan);
        List<string> reversedOrder = new List<string>(currentOrder);
        reversedPan.Reverse();
        reversedOrder.Reverse();

        panIter = reversedPan.GetEnumerator();
        orderIter = reversedOrder.GetEnumerator();

        while(panIter.MoveNext() && orderIter.MoveNext())
        {
            string panItem = panIter.Current;
            string orderItem = orderIter.Current;

            if(panItem == orderItem)
            {
                ++goodItems;
            }
        }

        Debug.Log("GOOD: " + goodItems + " BAD: " + badItems);
        return (goodItems, badItems);
    }
}
