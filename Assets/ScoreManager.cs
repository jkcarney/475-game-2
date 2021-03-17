using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;

    void Start()
    {
        currentScore = 0;
    }

    // "correctItems" is defined as the food items in the correct sequence 
    // "badItems" is defined as the amount of bad food items in the given burger
    public void AddScore(int correctItems, int badItems)
    {
        currentScore += correctItems;
    }
}
