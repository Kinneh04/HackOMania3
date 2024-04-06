using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;

public class LeaderboardPodiumItem : LeaderboardItem
{
    [SerializeField] Image profilePic;
    [SerializeField] TMP_Text text_nextLevel;

    public override void Initalize(PlayerLeaderboardEntry leaderboardEntry)
    {
        int currLevel = GetCurrLevel(leaderboardEntry.StatValue);

        text_name.text = leaderboardEntry.DisplayName;
        text_leaves.text = currLevel == 2
            ? leaderboardEntry.StatValue.ToString()
            : ((currLevel == 1 ? 400 : 150) - leaderboardEntry.StatValue).ToString();
        text_nextLevel.text = currLevel == 2 ? "" : " to " + (currLevel == 1 ? "Plant" : "Sprout");
    }

    public int GetCurrLevel(int currLeaves)
    {
        if (currLeaves >= 400)
            return 2; // Plant
        else if (currLeaves >= 150)
            return 1; // Sprout
        else
            return 0; // Seedling
    }
}
