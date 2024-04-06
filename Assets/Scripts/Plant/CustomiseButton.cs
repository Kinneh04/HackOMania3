using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomiseButton : MonoBehaviour
{
    // TODO: Thumbnail
    public Customizable CustomizableInfo;

    public void InitializeButton(PlantCustomiser customiser, Customizable info)
    {
        CustomizableInfo = info;
        GetComponent<Button>().onClick.AddListener(() => customiser.OnPressButton(this));
    }
}

[System.Serializable]
public class Customizable
{
    public int Index;
    public int LeavesRequired = 0;
    public Sprite Thumbnail;
    public string Name;

    public bool Locked = true;
    public bool Selected = false;
}
