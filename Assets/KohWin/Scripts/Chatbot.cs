using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
using TMPro;
using UnityEngine.UI;
public class Chatbot : MonoBehaviour
{

    [Header("Context and conditioning")]
    [TextArea(5,5)]
    public string Context, FailedToGenerateText;
    [TextArea(2, 3)]
    public string InputHeader, InputCloser, Seperator;
    //I have a string var s that i want to get whatever is after the seperator var.

    [Header("Variables")]
    public TMP_InputField TextInputfield;

    [Header("Response Management")]
    private string temptext;
    public GameObject ResponsePrefab, YourMessagePrefab;
    public Transform SpeechBubblesParent;


    public List<string> PastSuccessfulInputs, PastSuccessfulGenerations = new();
    public void OnClickSendMessage()
    {
        string contextMessage = Context + InputHeader + TextInputfield.text + InputCloser + Seperator;
        temptext = TextInputfield.text;
        SendYourMessage(temptext);
        HuggingFaceAPI.TextGeneration(contextMessage, OnSendMessageSuccess, OnSendMessageFailure);
    }

    public void OnSendMessageSuccess(string response)
    {
        PastSuccessfulGenerations.Add(response);
        PastSuccessfulInputs.Add(temptext);
        SendBotMessage(response);
    }

    public void OnSendMessageFailure(string response)
    {
     
    }

    public string ExtractStringAfterSeparator(string s)
    {
        int separatorIndex = s.IndexOf(Seperator);

        // Check if the separator was found
        if (separatorIndex != -1)
        {
            // Extract and return the substring after the separator
            // Adding the length of the separator to skip over it
            return s.Substring(separatorIndex + Seperator.Length);
        }
        else
        {
            // Separator not found, return an empty string or the original string,
            // depending on your needs. Here, returning an empty string as an example.
            return string.Empty;
        }
    }

    public void SendYourMessage(string s)
    {
        GameObject GO = GameObject.Instantiate(YourMessagePrefab);
        GO.transform.SetParent(SpeechBubblesParent, false);
        SpeechBubblePrefab BubblePrefab = GO.GetComponent<SpeechBubblePrefab>();
        BubblePrefab.typewrite = false;
        BubblePrefab.OnDisplaySpeech(s);
    }

    public void SendBotMessage(string s, LinkedImage LI = null)
    {
        GameObject GO = GameObject.Instantiate(ResponsePrefab);
        GO.transform.SetParent(SpeechBubblesParent, false);
        SpeechBubblePrefab BubblePrefab = GO.GetComponent<SpeechBubblePrefab>();
        BubblePrefab.typewrite = true;
        BubblePrefab.OnDisplaySpeech(ExtractStringAfterSeparator(s));
    }
}

public class LinkedImage
{

}
