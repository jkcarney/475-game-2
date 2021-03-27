using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//call method in score manager but do logic in score UI
public class ScoreManager : MonoBehaviour
{
    public static int currentScore;
    [Range(1, 100)]
    public int goodItemsFactor;
    
    public GameObject textPopup;

    public AudioClip badScore;
    public AudioClip goodScore;
    public AudioClip greatScore;
    public AudioClip amazingScore;
    
    Text ScoreZero;

    void Start()
    {
        currentScore = 0;
        ScoreZero = GameObject.Find("ScoreZero").GetComponent<Text>();
    }
    
    // "correctItems" is defined as the food items in the correct sequence 
    // "badItems" is defined as the amount of bad food items in the given burger
    public void AddScore(int correctItems, int badItems)
    {
        int calculatedScore = 0;
        calculatedScore = ((correctItems * goodItemsFactor) + (correctItems + badItems)) / (1 + badItems); 

        currentScore += calculatedScore;
        DisplayAddedScore(calculatedScore, correctItems, badItems);
        ScoreZero.GetComponent<Text>().text = currentScore + "";
    }

    private void DisplayAddedScore(int score, int good, int bad)
    {
        // Determine spawn location for text and instantiate
        Vector3 spawnLocation = GameObject.Find("PlateHand").transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, spawnLocation.z);
        GameObject text = Instantiate(textPopup, spawnLocation, Quaternion.identity);

        // Emit particle relative to amount of points earned, clamp it so not too many are emitted
        text.GetComponent<ParticleSystem>().Emit(Mathf.Clamp((good * 20), 0, 200));

        // Figure out which audio clip to play based on the score and how many bad items are there
        text.GetComponent<AudioSource>().clip = DetermineSoundToPlay(bad, score);
        text.GetComponent<AudioSource>().Play();

        // Determine color of text based on amount of bad items
        float redValue = 0.0f;
        float greenValue = 1.0f;
        TextMesh mesh = text.GetComponent<TextMesh>();
        // Each bad item increases the red by a factor of .25 and decreased green by a factor of .10
        for(int i = 0; i < bad; ++i)
        {
            redValue += 0.25f;
            greenValue -= 0.15f;
            mesh.color = new Color(redValue, greenValue, mesh.color.b, 1.0f);
        }

        // Append the text with the score
        mesh.text = "+" + score.ToString();
    }

    private AudioClip DetermineSoundToPlay(int bad, int score)
    {
        // Worst possible scores
        if(score <= 20)
        {
            return badScore;
        }
        // Perfect order
        if(bad == 0)
        {
            return amazingScore;
        }
        // Between 1 and 3 bad items
        else if(bad > 0 && bad < 4)
        {
            return greatScore;
        }
        // 4 or more bad items
        else
        {
            return goodScore;
        }
    }
}
