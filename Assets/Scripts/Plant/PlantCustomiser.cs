using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Linq;


public class PlantCustomiser : MonoBehaviour
{
    [SerializeField] MeshFilter pot, plant;
    [SerializeField] List<GameObject> tabs = new() { };
    [SerializeField] List<CustomizableTypes> potTypes, plantTypes;

    [SerializeField] TMP_Text text_selected;
    [SerializeField] RectTransform contentContainer;
    [SerializeField] GameObject customiseButtonPrefab;
    int currTab = -1;

    int currPot = -1, currPlant = -1;
    int defaultPot = 0, defaultPlant = 0;
    CustomiseButton currButton = null;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);

        // Hide content for now
        /*loading.SetActive(false);
        shopContent.SetActive(false);*/

        // Add events for tabs
        for (int i = 0; i < tabs.Count; i++)
        {
            int tmp = i;
            tabs[i].GetComponent<Button>().onClick.AddListener(() => OnSwitchTabs(tmp));
        }

    }

    #region Interface

    private void OnEnable()
    {
        ResetTabs();
    }

    private void OnDisable()
    {
        currButton = null;
    }


    private void SetLoading(bool isLoading)
    {
       // TODO
    }

    public void OnSwitchTabs(int index)
    {
        if (currTab == index)
            return;

        SetLoading(true);

        // TODO: Update tab view
        for (int j = 0; j < tabs.Count; j++)
        {
            if (j == index)
            {
                // TODO: Turn tab selected
            }
            else
            {
                // TODO: Turn tab unselected
            }
        }

        // Update shop content
        UpdateButtons(index);
    }


    private void ResetTabs()
    {
        PlayFabClientAPI.GetUserData(
        new GetUserDataRequest(),
        result =>
        {
            if (result.Data != null && result.Data.ContainsKey("CurrPot"))
            {
                defaultPot = currPot = int.Parse(result.Data["CurrPot"].Value);
                pot.mesh = potTypes[currPot].Mesh;
            }

            if (result.Data != null && result.Data.ContainsKey("CurrPlant"))
            {
                defaultPlant = currPlant = int.Parse(result.Data["CurrPlant"].Value);
                plant.mesh = plantTypes[currPlant].Mesh;
            }

            OnSwitchTabs(0);
        },
        OnError);
    }
    #endregion

    #region Playfab
    private void UpdateButtons(int index)
    {
        // Clear current content
        text_selected.text = "";
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        switch (index)
        {
            case 0: // Pots
                foreach (var pot in potTypes)
                {
                    GameObject button = Instantiate(customiseButtonPrefab, contentContainer);
                    button.GetComponent<CustomiseButton>().InitializeButton(this, new Customizable()
                    {
                        Index = i,
                        Type = potTypes[i],
                        // TODO: Locked = leavesneeded < currleaves,
                        Selected = currPot == i
                    });

                    if (currPot == i)
                        currButton = button.GetComponent<CustomiseButton>();

                    i++;
                }

                break;


            case 1: // Plants
                foreach (var plant in plantTypes)
                {
                    GameObject button = Instantiate(customiseButtonPrefab, contentContainer);
                    button.GetComponent<CustomiseButton>().InitializeButton(this, new Customizable()
                    {
                        Index = i,
                        Type = plantTypes[i],
                        Selected = currPlant == i
                    });

                    if (currPlant == i)
                        currButton = button.GetComponent<CustomiseButton>();

                    i++;
                }

                break;

            default:
                break;
        }

        currTab = index;
        SetLoading(false);
    }

    public void OnCancelChanges()
    {
        ClosePanel();
    }

    public void OnApplyChanges()
    {
       PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                {"CurrPot", currPot.ToString() },
                {"CurrPlant", currPlant.ToString() }
            },
            Permission = UserDataPermission.Public
        },
        _ => {
            // Update other panels as well
            ClosePanel(false);
        },
        OnError);
    }

    private void OnError(PlayFabError e)
    {
        Debug.LogError(e.GenerateErrorReport());
        SetLoading(false);
    }
    #endregion

    public void OnPressButton(CustomiseButton button)
    {
        if (button.CustomizableInfo.Selected || button.CustomizableInfo.Locked)
            return;

        switch(currTab)
        {
            case 0:
                pot.mesh = potTypes[button.CustomizableInfo.Index].Mesh;
                currPot = button.CustomizableInfo.Index;
                break;

            case 1:
                plant.mesh = plantTypes[button.CustomizableInfo.Index].Mesh;
                currPlant = button.CustomizableInfo.Index;
                break;

            default:
                break;
        }

        if (currButton != null)
            currButton.UpdateSelected(false);
        button.UpdateSelected(true);
        text_selected.text = button.CustomizableInfo.Type.Name;
        currButton = button;
    }

    public void ClosePanel(bool resetPlant = true)
    {
        if (resetPlant)
        {
            pot.mesh = potTypes[defaultPot].Mesh;
            plant.mesh = plantTypes[defaultPlant].Mesh;
        }

        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class CustomizableTypes
{
    public int LeavesRequired = 0;
    public Sprite Thumbnail;
    public Mesh Mesh;
    public string Name;
}
