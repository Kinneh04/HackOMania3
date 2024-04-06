using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlantCustomiser : MonoBehaviour
{
    [SerializeField] MeshFilter pot, plant;
    [SerializeField] List<Mesh> potTypes, plantTypes;

    [SerializeField] TMP_Text text_selected;
    [SerializeField] RectTransform contentContainer;
    [SerializeField] GameObject customiseButtonPrefab;

    int currTab = -1, currPot = 0, currPlant = 0;
    CustomiseButton currButton = null;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Playfab stuff here!
        OnSwitchTab(0);

    }

    public void OnSwitchTab(int tabNo)
    {
        if (currTab == tabNo)
            return;

        text_selected.text = "";
        foreach (Transform child in contentContainer.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        switch (tabNo)
        {
            case 0: // Pots
                foreach (var pot in potTypes)
                {
                    GameObject button = Instantiate(customiseButtonPrefab, contentContainer);
                    button.GetComponent<CustomiseButton>().InitializeButton(this, new Customizable()
                    {
                        Index = i,
                        LeavesRequired = (i + 1) * 40,
                        Name = "Pot " + i
                    });
                    i++;
                }

                // todo
                OnPressButton(contentContainer.GetChild(0).GetComponent<CustomiseButton>());
                break;


            case 1: // Plants
                foreach (var plant in plantTypes)
                {
                    GameObject button = Instantiate(customiseButtonPrefab, contentContainer);
                    string name = i switch
                    {
                        0 => "Bluebells",
                        1 => "Yellowbells",
                        2 => "Fern",
                        3 => "Purple Flowers",
                        4 => "Red Flowers",
                        _ => "???"
                    };

                    button.GetComponent<CustomiseButton>().InitializeButton(this, new Customizable()
                    {
                        Index = i,
                        LeavesRequired = (i + 1) * 40,
                        Name = name
                    });
                    i++;
                }

                // todo
                OnPressButton(contentContainer.GetChild(0).GetComponent<CustomiseButton>());
                break;

            default:
                break;
        }

        currTab = tabNo;
    }

    public void OnPressButton(CustomiseButton button)
    {
        switch(currTab)
        {
            case 0:
                pot.mesh = potTypes[button.CustomizableInfo.Index];
                currPot = button.CustomizableInfo.Index;
                break;

            case 1:
                plant.mesh = plantTypes[button.CustomizableInfo.Index];
                currPlant = button.CustomizableInfo.Index;
                break;

            default:
                break;
        }

        if (currButton != null)
            currButton.CustomizableInfo.Selected = false;
        button.CustomizableInfo.Selected = true;
        text_selected.text = button.CustomizableInfo.Name;
        currButton = button;
    }
}
