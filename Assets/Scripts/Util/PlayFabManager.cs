using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
    public MainMenuCamera mainMenuController;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccessfulLogin, OnError);
    }

    public bool IsPlayerLoggedIn()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }

    void OnSuccessfulLogin(LoginResult result)
    {
        Debug.Log("Successful login!");
        mainMenuController.SuccessfulLogin();
    }

    void OnLoginError(PlayFabError error)
    {
        Debug.Log("Error logging into Playfab: " + error.GenerateErrorReport());
        mainMenuController.LoginError();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error with Playfab: " + error.GenerateErrorReport());
    }

    public void SendLeaderboard(string scoreboard, int score)
    {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = scoreboard, 
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

    public void UpdateDisplayName(string display)
    {
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = display
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Display name updated.");
    }

    public void GetLeaderboard(string leaderboard)
    {
        var request = new GetLeaderboardRequest {
            StatisticName = leaderboard,
            StartPosition = 0,
            MaxResultsCount = 11
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        Debug.Log("Scoreboard was successsful gotten");
        GameObject.FindGameObjectWithTag("ScoreboardParent").GetComponent<ScoreboardUpdater>().UpdateScoreboard(result);
    }
}
