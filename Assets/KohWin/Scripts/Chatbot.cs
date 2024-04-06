using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuggingFace.API;
using TMPro;
using UnityEngine.UI;

using UnityEngine.Events;
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
    public string currentResponse;
    public string temptext, ContextedText;
    public GameObject ResponsePrefab, YourMessagePrefab, ResponseChallengePrefab;
    public Transform SpeechBubblesParent;

    [Header("ModularChatbotFuncs")]
    public Chatbot_ST CBM_SentenceTransform;
    public Chatbot_AT CBM_AutoTokenizer;

   


    public List<string> PastSuccessfulInputs, PastSuccessfulGenerations = new();
    public void OnClickSendMessage()
    {
        ContextedText = Context + InputHeader + TextInputfield.text + InputCloser + Seperator;
        
        temptext = TextInputfield.text;
        SendYourMessage(temptext);

        CBM_SentenceTransform.OnApply(temptext);
     
    }

    public void OnSTSuccess()
    {
        if(!CBM_SentenceTransform.FindHighestFactor())
        {
            // Respond Normally;
            HuggingFaceAPI.TextGeneration(ContextedText, OnSendMessageSuccess, OnSendMessageFailure);
        }
    }

    public void OnSendMessageSuccess(string response)
    {
        currentResponse = response;
        PastSuccessfulGenerations.Add(response);
        PastSuccessfulInputs.Add(temptext);
        SendBotMessage(response);
    }

    public void OnSendMessageFailure(string response)
    {
     
    }

    public string ExtractStringAfterSeparator(string s)
    {
        if (!s.Contains(Seperator)) return s;
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

    public void SendChallengeRecommendations(string s, Challenge C)
    {
        GameObject GO = GameObject.Instantiate(ResponseChallengePrefab);
        GO.transform.SetParent(SpeechBubblesParent, false);
        SpeechBubblePrefab BubblePrefab = GO.GetComponent<SpeechBubblePrefab>();
        BubblePrefab.image.sprite = C.ChallengeSprite;
        BubblePrefab.SavedChallenge = C;
        BubblePrefab.typewrite = true;
        BubblePrefab.OnDisplaySpeech(ExtractStringAfterSeparator(s));
    }

    public void SendBotMessage(string s)
    {
        GameObject GO = GameObject.Instantiate(ResponsePrefab);
        GO.transform.SetParent(SpeechBubblesParent, false);
        SpeechBubblePrefab BubblePrefab = GO.GetComponent<SpeechBubblePrefab>();
        BubblePrefab.typewrite = true;
        BubblePrefab.OnDisplaySpeech(ExtractStringAfterSeparator(s));
    }

    public void SendMessageWithCustomButton(string s, GameObject Buttonprefab, string buttonName, UnityAction action)
    {
        GameObject GO = GameObject.Instantiate(ResponsePrefab);
        GameObject GOprefab = GameObject.Instantiate(Buttonprefab);

        GO.transform.SetParent(SpeechBubblesParent, false);
        SpeechBubblePrefab BubblePrefab = GO.GetComponent<SpeechBubblePrefab>();
        GOprefab.transform.SetParent(BubblePrefab.BubbleCustomButtonprefabParent, false);

        CustomResponseButton responseButton = GOprefab.GetComponent<CustomResponseButton>();

        responseButton.btn.onClick.AddListener(action);
        responseButton.text.text = buttonName;
        GOprefab.transform.SetAsLastSibling();
        BubblePrefab.typewrite = true;
        BubblePrefab.OnDisplaySpeech(ExtractStringAfterSeparator(s));
    }
}

