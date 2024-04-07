using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] List<LeaderboardPodiumItem> podiumItems = new() { };
    [SerializeField] List<GameObject> tabs = new() { };
    [SerializeField] GameObject leaderboardContent, itemPrefab, loading, leaderboardPanel;

    List<GameObject> currLeaderboardItems = new() { };
    int currTab = -1;

    void Start()
    {
        //Hide content for now
/*        loading.SetActive(false);
        leaderboardPanel.SetActive(false);*/

        //Add events for tabs
        for (int i = 0; i < tabs.Count; i++)
        {
            int tmp = i;
            tabs[i].GetComponent<Button>().onClick.AddListener(() => OnSwitchTabs(tmp));
        }
    }

    private void OnEnable()
    {
        ResetTabs();
    }

    private void OnDisable()
    {
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }
        currLeaderboardItems.Clear();
    }

    #region PlayFab
    private void GetGlobalLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            StatisticName = "Leaves",
            StartPosition = 0,
            MaxResultsCount = 50,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => {
            UpdateLeaderboardInterface(result.Leaderboard);
        },
        OnError);
    }

    private void GetFriendsLeaderboard()
    {
        PlayFabClientAPI.GetFriendLeaderboard(new GetFriendLeaderboardRequest()
        {
            StatisticName = "Highscore",
            MaxResultsCount = 15,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }
        },
        result => {
            UpdateLeaderboardInterface(result.Leaderboard);
        },
        OnError);
    }

    private void UpdatePodiumItem(PlayerLeaderboardEntry leaderboardEntry, int index)
    {
        var obj = podiumItems[index].gameObject;
        podiumItems[index].gameObject.SetActive(true);
        CloudScriptManager.Instance.ExecGetUserDataAnotherPlayer(
            leaderboardEntry.PlayFabId,
            new List<string> { "CurrPot", "CurrPlant" },
            result =>
            {
                var keys = ((PlayFab.Json.JsonObject)result.FunctionResult).Keys;
                var values = ((PlayFab.Json.JsonObject)result.FunctionResult).Values;

                Dictionary<string, string> userData = new() { };
                for (int i = 0; i < keys.Count; ++i)
                {
                    userData.Add(
                        keys.ElementAt(i),
                        ((PlayFab.Json.JsonObject)values.ElementAt(i)).Values.ElementAt(0).ToString()
                    ); ;
                }
                podiumItems[index].Initalize(leaderboardEntry, userData, this);
            },
        OnError
        );
    }

    private void GetUserDataAndSpawnItem(PlayerLeaderboardEntry leaderboardEntry)
    {
        GameObject obj = Instantiate(itemPrefab, leaderboardContent.transform);
        CloudScriptManager.Instance.ExecGetUserDataAnotherPlayer(
            leaderboardEntry.PlayFabId,
            new List<string> { "CurrPot", "CurrPlant"},
            result =>
            {
                var keys = ((PlayFab.Json.JsonObject)result.FunctionResult).Keys;
                var values = ((PlayFab.Json.JsonObject)result.FunctionResult).Values;

                Dictionary<string, string> userData = new() { };
                for (int i = 0; i < keys.Count; ++i)
                {
                    userData.Add(
                        keys.ElementAt(i),
                        ((PlayFab.Json.JsonObject)values.ElementAt(i)).Values.ElementAt(0).ToString()
                    ); ;
                }
                obj.GetComponent<LeaderboardItem>().Initalize(leaderboardEntry, userData, this);
            },
        OnError
        );
        currLeaderboardItems.Add(obj);
    }

    public void OnError(PlayFabError e)
    {
        Debug.LogError("Error: " + e.GenerateErrorReport());
    }
#endregion

    #region Interface
    private void SetLoading(bool isLoading)
    {
        loading.SetActive(isLoading);
        leaderboardPanel.SetActive(!isLoading);
    }

    private void OnSwitchTabs(int index)
    {
        if (currTab == index)
            return;

        SetLoading(true);

        // Update tab view
        for (int i = 0; i < tabs.Count; i++)
        {
            // TODO: Update Tab Inactive/Active State
        }
        currTab = index;

        // Update leaderboard content
        UpdateLeaderboard(index);
    }

    private void ResetTabs()
    {
        OnSwitchTabs(0);
    }

    private void UpdateLeaderboardInterface(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        int i = 0;
        foreach (var entry in leaderboardEntries)
        {
            if (i < 3)
            {
                UpdatePodiumItem(entry, i);
            }
            else
            {
                GetUserDataAndSpawnItem(entry);
            }
            i++;
        }

        if (leaderboardEntries.Count < 3)
            podiumItems[2].gameObject.SetActive(false);
        if (leaderboardEntries.Count < 2)
            podiumItems[1].gameObject.SetActive(false);
        if (leaderboardEntries.Count < 1)
            podiumItems[0].gameObject.SetActive(false);

        SetLoading(false);
        // Wait until all leaderboard items are loaded
        //yield return new WaitUntil(() => currLeaderboardItems.Count == leaderboardEntries.Count);
        //SortLeaderboardItems();
    }
    #endregion

    #region Helper Functions
    private void UpdateLeaderboard(int index)
    {
        // Clear current content
        foreach (Transform child in leaderboardContent.transform)
        {
            Destroy(child.gameObject);
        }
        currLeaderboardItems.Clear();

        // Begin getting new content
        if (index == 0)
            GetGlobalLeaderboard();
        else
            GetFriendsLeaderboard();
    }

    public void OnPressItem(LeaderboardItem item)
    {
        StaticPlantClass.OtherUserName = item.Name;
        StaticPlantClass.OtherUserData = item.UserData;
        StaticPlantClass.OtherLeaves = item.Leaves;

        SceneManager.LoadScene("OtherPlantScene");
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("PlantScene");
    }
    #endregion
}
