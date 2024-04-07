using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class PlantLeaves : MonoBehaviour
{
    [SerializeField] TMP_Text text_leaves, text_level, text_nextLeaves;
    [SerializeField] Slider progress_amount;
    int? currLeaves = null;

    public int GetCurrLevel()
    {
        if (!currLeaves.HasValue)
            return -1;

        if (currLeaves >= 400)
            return 2; // Plant
        else if (currLeaves >= 150)
            return 1; // Sprout
        else
            return 0; // Seedling
    }

    public void UpdateLeavesCount(bool updateInterface = false)
    {
        text_leaves.text = "...";
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "Leaves",
            MaxResultsCount = 1,
        },
       result => {
           currLeaves = result.Leaderboard[0].StatValue;
           if (updateInterface)
               UpdateLeavesInterface(result.Leaderboard[0].StatValue);
       },
       OnError);
    }

    public void UpdateLeavesInterface(int? updatedLeaves = null)
    {
        int leaves = updatedLeaves.HasValue ? updatedLeaves.Value : currLeaves.Value;
        var currLevel = GetCurrLevel();

        if (currLevel >= 2)
        {
            text_level.text = "Level: Plant";
            text_leaves.text = leaves.ToString();
            text_nextLeaves.text = "earned this cycle.";
            progress_amount.gameObject.SetActive(false);
        }

        else
        {
            int nextLeaves = currLevel == 1 ? 400 : 150;
            text_level.text = currLevel == 1 ? "Level: Sprout" : "Level: Seedling";
            text_leaves.text = (nextLeaves - leaves).ToString();
            text_nextLeaves.text = "more to reach " + (currLevel == 1 ? "Plant" : "Sprout");
            progress_amount.gameObject.SetActive(true);
            progress_amount.maxValue = nextLeaves;
            progress_amount.value = leaves;
        }
    }

    public void AddLeaves(int amount)
    {
        if ((currLeaves += amount) < 0)
            amount = -1 * currLeaves.Value;

        // Send new highscore to leaderboard
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Leaves",
                    Value = amount
                }
            }
        }, _ => {
            currLeaves += amount;
            UpdateLeavesInterface();
        }, OnError);
    }

    private void OnError(PlayFabError e)
    {
        Debug.LogError(e.GenerateErrorReport());
    }
}
