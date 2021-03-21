using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    public int timesClicked;

    // Start is called before the first frame update
    void Start()
    {
        timesClicked = 0;
        Login();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
       {
           ++timesClicked;
       } 

       if(Input.GetKeyDown("space"))
       {
           SendLeaderboard(timesClicked);
           timesClicked = 0;
       }
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error logging into Playfab: " + error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "TestLeaderboard", 
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard send!");
    }
}
