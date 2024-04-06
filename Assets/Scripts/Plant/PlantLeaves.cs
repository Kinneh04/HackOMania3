using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;
using TMPro;

public class PlantLeaves : MonoBehaviour
{
    [SerializeField] TMP_Text text_leaves, text_level;
    [SerializeField] Slider progress_amount;
    int? currLeaves = null;

    private void OnEnable()
    {
        UpdateLeavesCount(true);
    }

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
               UpdateLeavesInterface();
       },
       OnError);
    }

    public void UpdateLeavesInterface()
    {
        var currLevel = GetCurrLevel();

        if (currLevel >= 2)
        {
            text_level.text = "Level: Plant";
            text_leaves.text = currLeaves.ToString() + " Leaves earned.";
            progress_amount.gameObject.SetActive(false);
        }

        else
        {
            int nextLeaves = currLevel == 1 ? 400 : 150;
            text_level.text = currLevel == 1 ? "Level: Sprout" : "Level: Seedling";
            text_leaves.text = (nextLeaves - currLeaves).ToString() + " more Leaves to level up!";
            progress_amount.gameObject.SetActive(true);
            progress_amount.value = currLeaves.Value;
            progress_amount.maxValue = nextLeaves;
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
