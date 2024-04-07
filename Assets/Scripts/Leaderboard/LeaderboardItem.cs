using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;

public class LeaderboardItem : MonoBehaviour
{
    [SerializeField] protected TMP_Text text_rank, text_name, text_leaves;
    public Dictionary<string, string> UserData;
    public string Name;
    public int Leaves;

    // TODO: Highlight if this is you!
    public virtual void Initalize(PlayerLeaderboardEntry leaderboardEntry, Dictionary<string, string> data, LeaderboardManager manager)
    {
        UserData = data;
        text_rank.text = "#" + (leaderboardEntry.Position + 1).ToString();
        text_name.text = Name = leaderboardEntry.DisplayName;
        text_leaves.text = leaderboardEntry.StatValue.ToString();
        Leaves = leaderboardEntry.StatValue;
        GetComponent<Button>().onClick.AddListener(() => manager.OnPressItem(this));
    }
}
