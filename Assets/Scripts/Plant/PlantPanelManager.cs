using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class PlantPanelManager : MonoBehaviour
{
    [SerializeField] RectTransform plantViewport; // To change height
    [SerializeField] GameObject infoPanel, extraInfo;
    [SerializeField] TMP_Text title;
    [Header("Panel Switching")]
    [SerializeField] List<PlantPanel> _panels = new List<PlantPanel>();
    [SerializeField] PlantPanelType _startPanel = PlantPanelType.GreenUp;
    public Button buttonBackGreenup;

    private PlantPanel _currPanel;
    public PlantPanel CurrPanel { get => _currPanel; }

    bool isMidSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        FindPanelOfType(PlantPanelType.GreenUp).panelObj.GetComponent<PlantLeaves>().UpdateLeavesCount(true);
        // Set all panels except for starting panel to inactive
        foreach (var panel in _panels)
        {
            if (panel.type == _startPanel)
            {
                panel.panelObj.SetActive(true);
                _currPanel = panel;
            }
            else
            {
                panel.panelObj.SetActive(false);
            }
        }
    }

    private PlantPanel FindPanelOfType(PlantPanelType type)
    {
        foreach (var panel in _panels)
        {
            if (panel.type == type)
                return panel;
        }

        return null;
    }

    public void OnSwitchPanel(string type)
    {
        // button params do not accept enums :( have to convert
        var currSize = plantViewport.sizeDelta;
        var newPanel = FindPanelOfType((PlantPanelType)System.Enum.Parse(typeof(PlantPanelType), type));

        if (isMidSwitch || newPanel == _currPanel)
            return;

        if (newPanel != null)
        {
            isMidSwitch = true;
            switch (newPanel.type)
            {
                case PlantPanelType.GreenUp:
                    plantViewport.DOSizeDelta(new Vector2(currSize.x, 1100f), 0.5f).OnComplete(() => isMidSwitch = false);
                    infoPanel.SetActive(true);
                    extraInfo.SetActive(false);
                    buttonBackGreenup.gameObject.SetActive(true);
                    title.text = "GreenUp";
                    break;

                case PlantPanelType.MyPlant:
                    plantViewport.DOSizeDelta(new Vector2(currSize.x, 1700f), 0.5f).OnComplete(() => isMidSwitch = false);
                    infoPanel.SetActive(true);
                    extraInfo.SetActive(true);
                    buttonBackGreenup.gameObject.SetActive(false);
                    title.text = "My Plant";
                    break;

                case PlantPanelType.Customisation:
                    plantViewport.DOSizeDelta(new Vector2(currSize.x, 1100f), 0.5f).OnComplete(() => isMidSwitch = false);
                    infoPanel.SetActive(false);
                    title.text = "Plant Customisation";
                    buttonBackGreenup.gameObject.SetActive(false);
                    break;

                default:
                    isMidSwitch = false;
                    break;
            }

            var seq = DOTween.Sequence()
                .Append(_currPanel.panelObj.GetComponent<CanvasGroup>().DOFade(0, 0.25f).OnComplete(() => { _currPanel.panelObj.SetActive(false); newPanel.panelObj.SetActive(true); }))
                .Append(newPanel.panelObj.GetComponent<CanvasGroup>().DOFade(1.0f, 0.25f))
                .AppendCallback(() =>
                {
                    _currPanel = newPanel;
                });
        }
    }

    public void GoMyPlantFromGreenUp()
    {
        if (_currPanel == null)
            return;

        if (_currPanel.type != PlantPanelType.GreenUp)
            return;

        OnSwitchPanel("MyPlant");
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void GoToHome()
    {
        SceneManager.LoadScene("HomePage");
    }
}

[System.Serializable]
public class PlantPanel
{
    public PlantPanelType type;
    public GameObject panelObj;
}

[System.Serializable]
public enum PlantPanelType
{
    GreenUp,
    MyPlant,
    Customisation
}
