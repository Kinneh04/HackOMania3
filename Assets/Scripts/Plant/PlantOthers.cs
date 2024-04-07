using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlantOthers : MonoBehaviour
{
    [SerializeField] MeshFilter pot, plant;
    [SerializeField] List<CustomizableTypes> potTypes, plantTypes;

    [SerializeField] TMP_Text text_name, text_leaves, text_level, text_nextLeaves;
    [SerializeField] Slider progress_amount;
    int? currLeaves = null;

    private void Start()
    {
        UpdatePlant();
        UpdateLeaves();
    }

    private void UpdatePlant()
    {
        text_name.text = StaticPlantClass.OtherUserName + "'s Pot";
        pot.mesh = potTypes[int.Parse(StaticPlantClass.OtherUserData["CurrPot"])].Mesh;
        plant.mesh = plantTypes[int.Parse(StaticPlantClass.OtherUserData["CurrPlant"])].Mesh;
    }

    int GetCurrLevel()
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


    private void UpdateLeaves()
    {
        currLeaves = StaticPlantClass.OtherLeaves;
        var currLevel = GetCurrLevel();

        if (currLevel >= 2)
        {
            text_level.text = "Level: Plant";
            text_leaves.text = currLeaves.ToString();
            text_nextLeaves.text = "earned this cycle.";
            progress_amount.gameObject.SetActive(false);
        }

        else
        {
            int nextLeaves = currLevel == 1 ? 400 : 150;
            text_level.text = currLevel == 1 ? "Level: Sprout" : "Level: Seedling";
            text_leaves.text = (nextLeaves - currLeaves).ToString();
            text_nextLeaves.text = "more to reach " + (currLevel == 1 ? "Plant" : "Sprout");
            progress_amount.gameObject.SetActive(true);
            progress_amount.maxValue = nextLeaves;
            progress_amount.value = currLeaves.Value;
        }
    }

    public void OnClickBack()
    {
        StaticPlantClass.OtherUserName = "";
        StaticPlantClass.OtherUserData = null;
        StaticPlantClass.OtherLeaves = 0;
        SceneManager.LoadScene("Leaderboard");
    }
}
