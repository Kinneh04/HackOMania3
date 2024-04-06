using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;


public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] protected TMP_Text text_rank, text_name, text_leaves;

    // TODO: Highlight if this is you!
    public virtual void Initalize(PlayerLeaderboardEntry leaderboardEntry)
    {
        text_rank.text = "#" + (leaderboardEntry.Position + 1).ToString();
        text_name.text = leaderboardEntry.DisplayName;
        text_leaves.text = leaderboardEntry.StatValue.ToString();
    }
}
