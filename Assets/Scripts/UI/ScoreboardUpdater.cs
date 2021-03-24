using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class ScoreboardUpdater : MonoBehaviour
{
    public GameObject rowPrefab;
    public Transform rowParent;
    public GameObject loadingIcon;

    public void UpdateScoreboard(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            GameObject newRow = Instantiate(rowPrefab, rowParent);
            Text[] texts = newRow.GetComponentsInChildren<Text>();
            texts[0].text = item.DisplayName;
            texts[1].text = item.StatValue.ToString();
        }

        loadingIcon.SetActive(false);
    }
}
