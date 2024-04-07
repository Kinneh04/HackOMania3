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

    public override void Initalize(PlayerLeaderboardEntry leaderboardEntry, Dictionary<string, string> data, LeaderboardManager manager)
    {
        int currLevel = GetCurrLevel(leaderboardEntry.StatValue);

        UserData = data;
        text_name.text = Name = leaderboardEntry.DisplayName;
        text_leaves.text = currLevel == 2
            ? leaderboardEntry.StatValue.ToString()
            : ((currLevel == 1 ? 400 : 150) - leaderboardEntry.StatValue).ToString();
        text_nextLevel.text = currLevel == 2 ? "" : " to " + (currLevel == 1 ? "Plant" : "Sprout");
        Leaves = leaderboardEntry.StatValue;

        GetComponent<Button>().onClick.AddListener(() => manager.OnPressItem(this));
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
