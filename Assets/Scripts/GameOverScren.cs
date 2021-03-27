using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScren : MonoBehaviour
{
    public Text getScore;
     void Start()
    {
        getScore = GameObject.Find("displayScore").GetComponent<Text>();
        getScore.GetComponent<Text>().text = ScoreManager.currentScore + "";
    }
    public void mainMenuButton(){
        SceneManager.LoadScene("MainMenu");
    }

    public void playAgainButton(){
        SceneManager.LoadScene("Main");
    }
}

