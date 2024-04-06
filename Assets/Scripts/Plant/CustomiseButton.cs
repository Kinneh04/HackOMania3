using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomiseButton : MonoBehaviour
{
    // TODO: Leaves
    [SerializeField] Image thumbnail;
    [SerializeField] GameObject selectedBG;
    public Customizable CustomizableInfo;

    public void InitializeButton(PlantCustomiser customiser, Customizable info)
    {
        CustomizableInfo = info;
        thumbnail.sprite = info.Type.Thumbnail;
        UpdateSelected(info.Selected);
        GetComponent<Button>().onClick.AddListener(() => customiser.OnPressButton(this));
    }

    public void UpdateSelected(bool selected)
    {
        selectedBG.SetActive(selected);
        GetComponent<Button>().enabled = !selected;
        CustomizableInfo.Selected = selected;
    }
}

[System.Serializable]
public class Customizable
{
    public int Index;
    public CustomizableTypes Type;

    public bool Locked = false;
    public bool Selected = false;
}
