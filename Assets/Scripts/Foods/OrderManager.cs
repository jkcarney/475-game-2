using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{ 
    // NOTE! The number here reflects how many ingredients go BETWEEN the
    // top and bottom bun, since the top and bottom bun will ALWAYS show the
    // start and end of an order. 
    private int difficulty;

    private int ingredientCount;
    private bool currentOrderExists;
    private string[] ingredients;

    private OrderDetection orderDetector;

    private ScoreManager scoreManager;

    private List<string> currentOrder;

    private float lastTimeOrderChecked;
    Text PrintOrder;
    void Start()
    {
        orderDetector = GameObject.Find("OrderDetector").GetComponent<OrderDetection>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        ingredients = Enum.GetNames(typeof(FoodItem.Food));
        ingredientCount = ingredients.Length;
        currentOrderExists = false;
        currentOrder = new List<string>();
        difficulty = DifficultyStatic.difficulty;
        PrintOrder = GameObject.Find("PrintOrder").GetComponent<Text>();
    } 

    void Update()
    {
        // If theres no order, create one
        if(!currentOrderExists)
        {
            InitializeOrder();
            LogCurrentOrder();
        }
        
        PrintOrder.GetComponent<Text>().text = ""; //Clear the text
        string result = "Order: \n";
        foreach (string item in currentOrder){ //Add each item to the text
            result += item + " \n";
        }
        PrintOrder.GetComponent<Text>().text = result;

        // Order comparison
        if(Input.GetMouseButtonDown(0) && Time.time > lastTimeOrderChecked + 1.0f)
        {
            (int, int) orderCorrectness = CompareOrderToPan(orderDetector.GetWhatsOnPan());
            // Item1 = good items
            // Item2 = bad items
            scoreManager.AddScore(orderCorrectness.Item1, orderCorrectness.Item2);
            orderDetector.RemoveAllPanItems();
            currentOrderExists = false;
            lastTimeOrderChecked = Time.time;
        }
    
    }

    //print this to screen..all items are added to a list so print the list
    //items stored in current order
    void InitializeOrder()
    {
        // Clear out the current order list
        currentOrder.Clear();
        // All orders will start with a bottom bun
        currentOrder.Add(Enum.GetName(typeof(FoodItem.Food), 1));
        // Grab random ingredients and add to list
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
        int goodItems = 0, badItems = 0, goodItemsReversed = 0, badItemsReversed = 0;

        // If this is true, then there are items that are going to be incorrect regardless of comparison
        // We can add the absolute difference to the badItems count.
        if(pan.Count != currentOrder.Count)
        {
            int incorrectCount = Mathf.Abs(pan.Count - currentOrder.Count);
            badItems += incorrectCount;
            badItemsReversed += incorrectCount;
        }

        // This section iterates the pan from the BOTTOM UP, looking for items in the exact order

        List<string>.Enumerator panIter = pan.GetEnumerator();
        List<string>.Enumerator orderIter = currentOrder.GetEnumerator();
        
        
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

        // This section iterates the pan from the TOP DOWN, looking for items in the exact order.
   
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
                ++goodItemsReversed;
            }
            else
            {
                ++badItemsReversed;
            }
        }

        // Takes the max of the good and the minumum of the bad items from the reversed and
        // normal lists to get the proper amount of good and bad items.
        int returnedGood = Mathf.Max(goodItems, goodItemsReversed);
        int returnedBad = Mathf.Min(badItems, badItemsReversed);

        Debug.Log("FINAL GOOD: " + returnedGood + " FINAL BAD: " + returnedBad);

        return (returnedGood, returnedBad);
    }
}