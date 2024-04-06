using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedButtonsManager : MonoBehaviour
{

    public List<GeneratedButton> ButtonsToGenerate = new();

    public GameObject Buttonprefab;
    public Transform ButtonPrefabParent;

    public Chatbot MainChatbot;

    private void Start()
    {
        SpawnGeneratedButtons();
    }
    public void SpawnGeneratedButtons()
    {
        foreach(GeneratedButton GB in ButtonsToGenerate)
        {
            GameObject Go = Instantiate(Buttonprefab);
            Go.transform.SetParent(ButtonPrefabParent, false);
           PreGenButtonPrefab btnPrefab = Go.GetComponent<PreGenButtonPrefab>();
            btnPrefab.StoredGB = GB;
            btnPrefab.btn.onClick.AddListener(delegate { OnClickButton(btnPrefab.StoredGB.QueryText); Destroy(btnPrefab); });
            btnPrefab.text.text = GB.ButtonTitle;
        }
    }
    public void OnClickButton(string stringToReplace)
    {

        MainChatbot.TextInputfield.text = stringToReplace;
    }
}

[System.Serializable]
public class GeneratedButton
{
    public string ButtonTitle;
    public string QueryText;
}
